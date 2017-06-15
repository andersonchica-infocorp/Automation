using Microsoft.Xml.XMLGen;
using Sofka.Automation.Entities.Wsdl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Sofka.Automation.Provider
{
    public class XsdGenerator
    {
        private int count = 0;

        public XsdGenerator()
        {
            this.GetStructure("http://www.webservicex.net/country.asmx?wsdl");
            //this.GetStructure("https://www.w3schools.com/xml/tempconvert.asmx?wsdl");
            //this.GetStructure("http://sofkatestservice.azurewebsites.net/Services/ComplexService.asmx?wsdl");
        }


        public string GetStructure(string url)
        {
            ServiceDescription serviceDescription = this.GetServiceDescription(url);
            XmlSchemaSet xmlSchemaSet = this.GetSchemasService(serviceDescription);

            Dictionary<string, dynamic> operations = this.GetOperations(serviceDescription);
            operations = this.GetParametersOperations(operations, xmlSchemaSet, serviceDescription);

            foreach (KeyValuePair<string, dynamic> operation in operations)
            {
                this.GenerateXml(operation);
            }

            return string.Empty;
        }

        private XmlDocument GenerateXml(KeyValuePair<string, dynamic> operation)
        {
            XmlDocument xmlDocument = new XmlDocument();
            List<Parameter> a = new List<Parameter>();
            if (IsPropertyExist(operation.Value, "ParameterInput"))
            {
                a = operation.Value.ParameterInput;
            }

            XmlElement request = this.GetXmlParameters(a, xmlDocument, operation.Value.Operation.Name);
            request.SetAttribute("xmlns", operation.Value.Input.Message.Namespace);

            return xmlDocument;
        }

        private XmlElement GetXmlParameters(List<Parameter> parameters, XmlDocument xmlDocument, string elementName)
        {
            XmlElement xmlElementCreated = xmlDocument.CreateElement(elementName);
            Dictionary<string, string> nameSpaceInserted = new Dictionary<string, string>();

            foreach (Parameter parameter in parameters)
            {
                if (parameter.Value != null)
                {
                    xmlElementCreated.AppendChild(GetXmlParameters((List<Parameter>)parameter.Value, xmlDocument, parameter.Name));
                }
                else
                {
                    string aliasNameSpaceCurrentElement = string.Empty;
                    string alias = string.Empty;

                    if (!string.IsNullOrEmpty(parameter.NameSpace))
                    {
                        if (!nameSpaceInserted.ContainsKey(parameter.NameSpace))
                        {
                            alias = string.Format("sofka{0}", this.count);
                            aliasNameSpaceCurrentElement = string.Format("xmlns:{0}", alias);

                            xmlElementCreated.SetAttribute(aliasNameSpaceCurrentElement, parameter.NameSpace);
                            nameSpaceInserted.Add(parameter.NameSpace, alias);
                            this.count++;
                        }
                        else
                        {
                            var aliasNameSpace = nameSpaceInserted.FirstOrDefault(c => c.Key == parameter.NameSpace);
                            alias = aliasNameSpace.Value;
                            aliasNameSpaceCurrentElement = aliasNameSpace.Key;
                        }
                    }

                    if (!string.IsNullOrEmpty(alias))
                    {
                        alias = string.Format("{0}:", alias);
                    }

                    xmlElementCreated.AppendChild(xmlDocument.CreateElement(string.Format("{0}{1}", alias, parameter.Name), parameter.NameSpace));
                }
            }

            return xmlElementCreated;
        }

        private Dictionary<string, dynamic> GetParametersOperations(Dictionary<string, dynamic> operations, XmlSchemaSet xmlSchemaSet, ServiceDescription serviceDescription)
        {
            foreach (var operation in operations)
            {
                Operation operationAux = ((Operation)operation.Value.Operation);
                operation.Value.Input = operationAux.Messages.Input;
                operation.Value.OutPut = operationAux.Messages.Output;

                foreach (Message message in serviceDescription.Messages)
                {
                    if (message.Name == operation.Value.Input.Message.Name)
                    {
                        foreach (MessagePart messagePart in message.Parts)
                        {
                            if (messagePart.Name == "parameters")
                            {
                                string nameSpace = messagePart.Element.Namespace;
                                string name = messagePart.Element.Name;

                                operation.Value.ParameterInput = this.GetParameters(nameSpace, name, xmlSchemaSet);
                            }
                        }
                    }
                }
            }

            return operations;
        }

        private dynamic GetParameters(string nameSpace, string name, XmlSchemaSet xmlSchemaSet)
        {
            List<Parameter> parameters = new List<Parameter>();
            if (xmlSchemaSet.Contains(nameSpace))
            {
                foreach (XmlSchema xmlSchema in xmlSchemaSet.Schemas(nameSpace))
                {
                    foreach (dynamic xmlElement in xmlSchema.Items)
                    {
                        if (xmlElement.Name == name)
                        {
                            XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
                            if (IsPropertyExist(xmlElement, "ElementSchemaType"))
                            {
                                xmlSchemaComplexType = xmlElement.ElementSchemaType;
                            }
                            else
                            {
                                xmlSchemaComplexType = xmlElement as XmlSchemaComplexType;
                            }

                            XmlSchemaParticle particle = xmlSchemaComplexType.Particle;
                            XmlSchemaSequence sequence = particle as System.Xml.Schema.XmlSchemaSequence;

                            if (sequence != null)
                            {
                                foreach (XmlSchemaElement element in sequence.Items)
                                {
                                    if (!this.ExistParameterName(element.Name, parameters))
                                    {
                                        if (element.ElementSchemaType is XmlSchemaComplexType)
                                        {
                                            parameters.Add(new Parameter(element.Name, element.ElementSchemaType.Name,
                                                this.GetParameters(element.ElementSchemaType.QualifiedName.Namespace,
                                                element.ElementSchemaType.Name,
                                                xmlSchemaSet), element.QualifiedName.Namespace));
                                        }
                                        else
                                        {
                                            parameters.Add(new Parameter(element.Name, element.SchemaTypeName.Name, null, xmlSchema.TargetNamespace));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return parameters;
        }

        private bool ExistParameterName(string name, List<Parameter> parameters)
        {
            return parameters.Any(c => c.Name == name);
        }

        private Dictionary<string, dynamic> GetOperations(ServiceDescription serviceDescription)
        {
            Dictionary<string, dynamic> operations = new Dictionary<string, dynamic>();
            foreach (PortType portType in serviceDescription.PortTypes)
            {
                foreach (Operation operation in portType.Operations)
                {
                    dynamic expandoObject = new ExpandoObject();
                    expandoObject.Operation = operation;
                    operations.Add(string.Format("{0}-{1}", portType.Name, operation.Name), expandoObject);
                }
            }

            return operations;
        }

        private XmlSchemaSet GetSchemasService(ServiceDescription serviceDescription)
        {
            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            foreach (XmlSchema item in serviceDescription.Types.Schemas)
            {
                xmlSchemaSet.Add(item);
            }

            xmlSchemaSet = this.ValidateSchemaLocation(xmlSchemaSet);
            xmlSchemaSet.Compile();

            return xmlSchemaSet;
        }

        private XmlSchemaSet ValidateSchemaLocation(XmlSchemaSet xmlSchemaSet)
        {
            XmlSchemaSet xmlSchemaSetAux = xmlSchemaSet;
            foreach (XmlSchema schemaLocation in xmlSchemaSetAux.Schemas())
            {
                foreach (XmlSchemaImport include in schemaLocation.Includes)
                {
                    if (!string.IsNullOrEmpty(include.SchemaLocation))
                    {
                        xmlSchemaSet.Add(include.Schema);
                    }
                }
            }

            return xmlSchemaSet;
        }

        private ServiceDescription GetServiceDescription(string url)
        {
            WebClient http = new WebClient();
            ServiceDescription serviceDescription = null;

            serviceDescription = ServiceDescription.Read(http.OpenRead(url));

            return serviceDescription;
        }

        public static bool IsPropertyExist(dynamic settings, string name)
        {
            if (settings is System.Dynamic.ExpandoObject)
                return ((IDictionary<string, object>)settings).ContainsKey(name);

            return settings.GetType().GetProperty(name) != null;
        }
    }
}