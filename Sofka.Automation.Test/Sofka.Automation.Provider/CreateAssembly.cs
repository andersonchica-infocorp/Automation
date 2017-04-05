using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Sofka.Automation.Provider
{
    public class CreateAssembly
    {
        private Assembly assem;
        private string serviceName;
        public void GenerateProxyClass()
        {
            EndpointAddress metadataAddress = new EndpointAddress("http://localhost/Sofka.Automation.Dummy.Wcf/Loan.svc?wsdl");
            //string  = "http://localhost/Sofka.Automation.Dummy.Wcf/Loan.svc";
            string outputFile = @"d:\dev\Sofka\Test\TempFiles";
            MetadataExchangeClient mexClient = new MetadataExchangeClient(metadataAddress);
            mexClient.ResolveMetadataReferences = false;
            MetadataSet metaDocs = mexClient.GetMetadata();

            WsdlImporter importer = new WsdlImporter(metaDocs);
            ServiceContractGenerator generator = new ServiceContractGenerator();

            // Add our custom DCAnnotationSurrogate 
            // to write XSD annotations into the comments.
            object dataContractImporter;
            XsdDataContractImporter xsdDCImporter;
            if (!importer.State.TryGetValue(typeof(XsdDataContractImporter), out dataContractImporter))
            {
                Console.WriteLine("Couldn't find the XsdDataContractImporter! Adding custom importer.");
                xsdDCImporter = new XsdDataContractImporter();
                xsdDCImporter.Options = new ImportOptions();
                importer.State.Add(typeof(XsdDataContractImporter), xsdDCImporter);
            }
            else
            {
                xsdDCImporter = (XsdDataContractImporter)dataContractImporter;
                if (xsdDCImporter.Options == null)
                {
                    Console.WriteLine("There were no ImportOptions on the importer.");
                    xsdDCImporter.Options = new ImportOptions();
                }
            }
            //xsdDCImporter.Options.DataContractSurrogate = new DCAnnotationSurrogate();

            // Uncomment the following code if you are going to do your work programmatically rather than add 
            // the WsdlDocumentationImporters through a configuration file. 
            /*
            // The following code inserts a custom WsdlImporter without removing the other 
            // importers already in the collection.
            System.Collections.Generic.IEnumerable<IWsdlImportExtension> exts = importer.WsdlImportExtensions;
            System.Collections.Generic.List<IWsdlImportExtension> newExts 
              = new System.Collections.Generic.List<IWsdlImportExtension>();
            foreach (IWsdlImportExtension ext in exts)
            {
              Console.WriteLine("Default WSDL import extensions: {0}", ext.GetType().Name);
              newExts.Add(ext);
            }
            newExts.Add(new WsdlDocumentationImporter());
            System.Collections.Generic.IEnumerable<IPolicyImportExtension> polExts = importer.PolicyImportExtensions;
            importer = new WsdlImporter(metaDocs, polExts, newExts);
            */

            System.Collections.ObjectModel.Collection<ContractDescription> contracts
              = importer.ImportAllContracts();
            importer.ImportAllEndpoints();
            foreach (ContractDescription contract in contracts)
            {
                generator.GenerateServiceContractType(contract);
            }
            if (generator.Errors.Count != 0)
                throw new Exception("There were errors during code compilation.");

            // Write the code dom
            System.CodeDom.Compiler.CodeGeneratorOptions options
              = new System.CodeDom.Compiler.CodeGeneratorOptions();
            options.BracingStyle = "C";
            System.CodeDom.Compiler.CodeDomProvider codeDomProvider
              = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("C#");
            System.CodeDom.Compiler.IndentedTextWriter textWriter
              = new System.CodeDom.Compiler.IndentedTextWriter(new System.IO.StreamWriter(outputFile));
            codeDomProvider.GenerateCodeFromCompileUnit(
              generator.TargetCompileUnit, textWriter, options
            );
            textWriter.Close();
        }
    }
}
