using Sofka.Automation.Entities.Wsdl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml.Schema;


namespace Sofka.Automation.Provider
{
    public class WebServiceInfo
    {
        WebMethodInfoCollection _webMethods =
                              new WebMethodInfoCollection();
        private XmlSchemaSet e = new XmlSchemaSet();
        Uri _url;
        string _serviceName = "";
        static Dictionary<string, WebServiceInfo> _webServiceInfos =
                                   new System.Collections.Generic.Dictionary<string, WebServiceInfo>();

        /// 

        /// Constructor creates the web service info from the given url.
        /// 

        /// 
        private WebServiceInfo(Uri url)
        {
            if (url == null)
                throw new ArgumentNullException("url");
            _url = url;
            _webMethods = GetWebServiceDescription(url);
        }

        /// 

        /// Factory method for WebServiceInfo. Maintains a 
        /// hashtable WebServiceInfo objects
        /// keyed by url in order to cache previously accessed wsdl files.
        /// 

        /// 
        /// 
        public static WebServiceInfo OpenWsdl(Uri url)
        {

            WebServiceInfo webServiceInfo;
            if (!_webServiceInfos.TryGetValue(url.ToString(), out webServiceInfo))
            {

                webServiceInfo = new WebServiceInfo(url);
                _webServiceInfos.Add(url.ToString(), webServiceInfo);
                //_webServiceInfos.Add(url.ToString(), null);
            }
            return webServiceInfo;
        }

        /// 

        /// Convenience overload that takes a string url
        /// 

        /// 
        /// 
        public static WebServiceInfo OpenWsdl(string url)
        {
            Uri uri = new Uri(url);
            return OpenWsdl(uri);
        }

        /// 

        /// Load the WSDL file from the given url.
        /// Use the ServiceDescription class to walk the wsdl and 
        ///   create the WebServiceInfo
        /// instance.
        /// 

        /// 
        private WebMethodInfoCollection GetWebServiceDescription(Uri url)
        {
            UriBuilder uriBuilder = new UriBuilder(url);
            uriBuilder.Query = "WSDL";

            WebMethodInfoCollection webMethodInfos = new WebMethodInfoCollection();

            HttpWebRequest webRequest =
                    (HttpWebRequest)WebRequest.Create(uriBuilder.Uri);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Method = "GET";
            webRequest.Accept = "text/xml";

            ServiceDescription serviceDescription;

            using (System.Net.WebResponse response = webRequest.GetResponse())
            using (System.IO.Stream stream = response.GetResponseStream())
            {
                serviceDescription = ServiceDescription.Read(stream);
            }
            _serviceName = serviceDescription.Name;
            desc(serviceDescription, url);

            foreach (PortType portType in serviceDescription.PortTypes)
            {
                foreach (Operation operation in portType.Operations)
                {
                    string operationName = operation.Name;
                    string inputMessageName = operation.Messages.Input.Message.Name;
                    string outputMessageName = operation.Messages.Output.Message.Name;

                    // get the message part
                    string inputMessagePartName =
                        serviceDescription.Messages[inputMessageName].Parts[0].Element.Name;
                    string outputMessagePartName =
                        serviceDescription.Messages[outputMessageName].Parts[0].Element.Name;

                    // get the parameter name and type
                    Parameter[] inputParameters = GetParameters(inputMessagePartName);
                    Parameter[] outputParameters = GetParameters(outputMessagePartName);

                    WebMethodInfo webMethodInfo = new WebMethodInfo(
                        operation.Name, inputParameters, outputParameters);
                    webMethodInfos.Add(webMethodInfo);
                }
            }

            return webMethodInfos;
        }

        private Parameter[] GetParameters(string messagePartName)
        {


            List<Parameter> parameters = new List<Parameter>();

            //Types types = serviceDescription.Types;
            //System.Xml.Schema.XmlSchema xmlSchema = types.Schemas[0];

            foreach (XmlSchemaElement schemaElement in e.GlobalElements.Values)
            {

                //}
                //foreach (object item in xmlSchema.Items)
                //{
                //    System.Xml.Schema.XmlSchemaElement schemaElement = item as System.Xml.Schema.XmlSchemaElement;
                if (schemaElement != null)
                {
                    if (schemaElement.Name == messagePartName)
                    {
                        System.Xml.Schema.XmlSchemaType schemaType = schemaElement.SchemaType;
                        System.Xml.Schema.XmlSchemaComplexType complexType =
                      schemaType as System.Xml.Schema.XmlSchemaComplexType;
                        if (complexType != null)
                        {
                            System.Xml.Schema.XmlSchemaParticle particle = complexType.Particle;
                            System.Xml.Schema.XmlSchemaSequence sequence = particle as System.Xml.Schema.XmlSchemaSequence;
                            if (sequence != null)
                            {
                                foreach (System.Xml.Schema.XmlSchemaElement childElement in sequence.Items)
                                {
                                    string parameterName = childElement.Name;
                                    string parameterType = childElement.SchemaTypeName.Name;
                                    parameters.Add(new Parameter(parameterName, parameterType, null, null));
                                }
                            }
                        }
                        break;
                    }
                }
            }
            return parameters.ToArray();
        }

        private void desc(ServiceDescription serviceDescription, Uri baseUri)
        {
            //Uri baseUri = new Uri(A_0);
            foreach (XmlSchema schema in serviceDescription.Types.Schemas)
            {
                if (schema.Items.Count > 0)
                {
                    e.Add(schema);
                }
                foreach (XmlSchemaObject obj2 in schema.Includes)
                {
                    if (obj2 is XmlSchemaImport)
                    {
                        try
                        {
                            string schemaUri = new Uri(baseUri, ((XmlSchemaImport)obj2).SchemaLocation).AbsoluteUri;
                            e.Add(((XmlSchemaImport)obj2).Namespace, schemaUri);
                            continue;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }
            e.CompilationSettings.EnableUpaCheck = false;
            e.Compile();
        }

        public WebMethodInfoCollection WebMethods
        {
            get { return _webMethods; }
        }

        /// 

        /// Url
        /// 

        public Uri Url
        {
            get { return _url; }
            set { _url = value; }
        }
        public string ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }
    }
}