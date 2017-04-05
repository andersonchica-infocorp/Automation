﻿using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml;

namespace Sofka.Automation.Provider
{
    public class DynamicProxyFactorySofka
    {
        public void Webservicecall(string contryname)
        {

            WebRequest webRequest = WebRequest.Create("http://localhost/Sofka.Automation.Dummy.Wcf/Loan.svc?singleWsdl");
            WebResponse webResponse = webRequest.GetResponse();
            // Stream stream = webResponse.GetResponseStream(); 
            ServiceDescription description = new ServiceDescription();
            using (Stream stream = webResponse.GetResponseStream())
            {
                description = ServiceDescription.Read(stream);
            }
            ServiceDescriptionImporter importer = new ServiceDescriptionImporter();


            importer.ProtocolName = "Soap12";//' Use SOAP 1.2. 
            importer.AddServiceDescription(description, null, null);
            importer.Style = ServiceDescriptionImportStyle.Client;

            //'--Generate properties to represent primitive values. 
            importer.CodeGenerationOptions = System.Xml.Serialization.CodeGenerationOptions.GenerateProperties;
            //'Initialize a Code-DOM tree into which we will import the service. 

            CodeNamespace nmspace = new CodeNamespace();
            CodeCompileUnit unit1 = new CodeCompileUnit();
            unit1.Namespaces.Add(nmspace);
            ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit1);
            CodeDomProvider provider1 = CodeDomProvider.CreateProvider("C#");

            //'--Compile the assembly proxy with the // appropriate references 
            String[] assemblyReferences;
            assemblyReferences = new String[] { "System.dll", "System.Web.Services.dll", "System.Web.dll", "System.Xml.dll", "System.Data.dll" };

            CompilerParameters parms = new CompilerParameters(assemblyReferences);
            parms.GenerateInMemory = true;
            CompilerResults results = provider1.CompileAssemblyFromDom(parms, unit1);
            if (results.Errors.Count > 0)
            {
            }


            Type foundType = null;
            Type[] types = results.CompiledAssembly.GetTypes();
            foreach (Type type1 in types)
            {
                if (type1.BaseType == typeof(SoapHttpClientProtocol))
                {
                    foundType = type1;
                }
            }


            if (!String.IsNullOrEmpty(contryname))
            {
                Object[] args = new Object[1];
                args[0] = contryname;
                Object wsvcClass = results.CompiledAssembly.CreateInstance(foundType.ToString());
                MethodInfo mi = wsvcClass.GetType().GetMethod("Prueba");
                var returnValue = mi.Invoke(wsvcClass, null);
                DataSet ds = new DataSet();
                //grdcountrydata.DataSource = ConvertXMLToDataSet(returnValue.ToString());
                //grdcountrydata.DataBind();
            }
            else
            {
                Object wsvcClass = results.CompiledAssembly.CreateInstance(foundType.ToString());
                MethodInfo mi = wsvcClass.GetType().GetMethod("Prueba");
                var returnValue = mi.Invoke(wsvcClass, null);
                DataSet ds = new DataSet();
                //grdcountry.DataSource = ConvertXMLToDataSet(returnValue.ToString());
                //grdcountry.DataBind();
            }





        }

        public DataSet ConvertXMLToDataSet(string xmlData)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmlData);
                // Load the XmlTextReader from the stream 
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }
       
    }
}
