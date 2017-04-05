using Microsoft.Xml.XMLGen;
using System;
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
            this.GenerateWsdlProxyClass("http://sofkatestservice.azurewebsites.net/Services/ComplexService.asmx?wsdl");

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
