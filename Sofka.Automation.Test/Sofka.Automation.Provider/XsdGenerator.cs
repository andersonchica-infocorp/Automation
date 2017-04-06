using Microsoft.Xml.XMLGen;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public XsdGenerator()
        {
            this.GenerateWsdlProxyClass("http://localhost/Sofka.Automation.Dummy.Wcf/Loan.svc?singleWsdl");

            XmlTextWriter textWriter = new XmlTextWriter(@"d:\dev\Sofka\Test\TempFiles\po3.xml", null);
            textWriter.Formatting = Formatting.Indented;
            XmlQualifiedName qname = new XmlQualifiedName("GetComplexObject");
            XmlSampleGenerator generator = new XmlSampleGenerator("http://sofkatestservice.azurewebsites.net/Services/ComplexService.asmx?wsdl", qname);
            generator.WriteXml(textWriter);


        }

        public bool GenerateWsdlProxyClass(string wsdlUrl)
        {
            // erase the source file
            //if (File.Exists(generatedSourceFilename))
            //    File.Delete(generatedSourceFilename);

            // download the WSDL content into a service description
            WebClient http = new WebClient();
            ServiceDescription sd = null;

            //if (!string.IsNullOrEmpty(username))
            //    http.Credentials = new NetworkCredential(username, password);

            try
            {
                sd = ServiceDescription.Read(http.OpenRead(wsdlUrl));
            }
            catch (Exception ex)
            {
                //this.ErrorMessage = "Wsdl Download failed: " + ex.Message;
                return false;
            }
                        
            foreach (PortType portType in sd.PortTypes)
            {
                foreach (Operation operation in portType.Operations)
                {
                    string opInput = operation.Messages.Input.Message.Name;
                    string opOutPut = operation.Messages.Output.Message.Name;

                    foreach (Message message in sd.Messages)
                    {
                        if (message.Name == opInput)
                        {
                            MessagePart mp = message.FindPartByName("parameters");
                            string nameParameter = mp.Element.Name;
                            foreach (XmlSchema schema in sd.Types.Schemas)
                            {
                                foreach (XmlSchemaElement element in schema.Items)
                                {
                                    if (element.Name == nameParameter)
                                    {
                                        XmlSchemaType schemaType = element.SchemaType;
                                        XmlSchemaComplexType complexType = schemaType as XmlSchemaComplexType;
                                        if (complexType != null)
                                        {
                                           XmlSchemaParticle particle = complexType.Particle;
                                           XmlSchemaSequence sequence = particle as System.Xml.Schema.XmlSchemaSequence;
                                            if (sequence != null)
                                            {
                                                foreach (System.Xml.Schema.XmlSchemaElement childElement in sequence.Items)
                                                {
                                                    string parameterName = childElement.Name;
                                                    string parameterType = childElement.SchemaTypeName.Name;

                                                    foreach (XmlSchema schemaGlobal in sd.Types.Schemas)
                                                    {
                                                        foreach (DictionaryEntry item in schemaGlobal.SchemaTypes)
                                                        {
                                                            if(((XmlQualifiedName)item.Key) == childElement.SchemaTypeName)
                                                            {
                                                                XmlSchemaParticle particu = ((XmlSchemaComplexType)item.Value).Particle;
                                                                XmlSchemaSequence seq = particu as System.Xml.Schema.XmlSchemaSequence;

                                                                foreach (XmlSchemaElement elementito in seq.Items)
                                                                {
                                                                    string parameterNameu = elementito.Name;
                                                                    string parameterTypeu = elementito.SchemaTypeName.Name;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    

                                                    //XmlTextWriter textWriter = new XmlTextWriter(@"d:\dev\Sofka\Test\TempFiles\po3.xml", null);
                                                    //textWriter.Formatting = Formatting.Indented;
                                                    //XmlSampleGenerator generator = new XmlSampleGenerator(set, childElement.QualifiedName);
                                                    //generator.WriteXml(textWriter);

                                                    //XmlTextWriter textWriter2 = new XmlTextWriter(@"d:\dev\Sofka\Test\TempFiles\po4.xml", null);
                                                    //textWriter.Formatting = Formatting.Indented;
                                                    //XmlSampleGenerator generator2 = new XmlSampleGenerator(set, childElement.SchemaTypeName);
                                                    //generator.WriteXml(textWriter);

                                                    //parameters.Add(new Parameter(parameterName, parameterType));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }



            // create an importer and associate with the ServiceDescription
            ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
            importer.ProtocolName = "SOAP";
            importer.CodeGenerationOptions = CodeGenerationOptions.None;
            importer.AddServiceDescription(sd, null, null);

            // Download and inject any imported schemas (ie. WCF generated WSDL)            
            foreach (XmlSchema wsdlSchema in sd.Types.Schemas)
            {
                // Loop through all detected imports in the main schema
                foreach (XmlSchemaObject externalSchema in wsdlSchema.Includes)
                {
                    // Read each external schema into a schema object and add to importer
                    if (externalSchema is XmlSchemaImport)
                    {
                        Uri baseUri = new Uri(wsdlUrl);
                        Uri schemaUri = new Uri(baseUri, ((XmlSchemaExternal)externalSchema).SchemaLocation);

                        Stream schemaStream = http.OpenRead(schemaUri);
                        System.Xml.Schema.XmlSchema schema = XmlSchema.Read(schemaStream, null);
                        importer.Schemas.Add(schema);
                    }
                }
            }

            return false;
        }
    }
}
