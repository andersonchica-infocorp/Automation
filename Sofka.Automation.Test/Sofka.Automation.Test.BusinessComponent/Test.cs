using Sofka.Automation.CrossCutting.Utils;
using Sofka.Automation.Entities.Test;
using Sofka.Automation.Entities.Wsdl;
using Sofka.Automation.Provider;
using Sofka.Automation.Test.BusinessComponent.LoanService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Sofka.Automation.Test.BusinessComponent
{
    public class Test
    {
        private DataAccess.Test testDataAccess = null;

        private LoanClient loanClient = null;

        static Provider.TestConnector client = null;

        public Test()
        {
            string url = "http://localhost/Sofka.Automation.Dummy.Wcf/Loan.svc?wsdl";

            WebServiceInfo webServiceInfo;

            WebRequest.DefaultWebProxy = WebRequest.GetSystemWebProxy();
            try
            {
                webServiceInfo = WebServiceInfo.OpenWsdl(new Uri(url));
            }
            catch
            {
                //NetworkCredential credentials =
                //   new NetworkCredential("userId", "password");
                //WebProxy proxy = new WebProxy("xx.xx.xx.xx:8080",
                //    true, null, credentials);
                //WebRequest.DefaultWebProxy = proxy;
                webServiceInfo = WebServiceInfo.OpenWsdl(new Uri(url));
            }

            StringBuilder sb = new StringBuilder();

            int i = 1;
            string strInputParameter = string.Empty;
            string strParameter = "";
            foreach (WebMethodInfo method in webServiceInfo.WebMethods)
            {
                sb.Append(i.ToString() + ". " + string.Format("{0}",
                        method.Name));

                foreach (Parameter parameter in method.InputParameters)
                {
                    strInputParameter += string.Format("{0} {1},", parameter.Type,
                              parameter.Name);
                }
                if (strInputParameter != "")
                {
                    strInputParameter = strInputParameter.Substring(0,
                                     strInputParameter.Length - 1);
                    sb.Append("(" + strInputParameter + ")");
                    strInputParameter = "";
                }
                else
                {
                    sb.Append("()");
                }
                sb.Append("\r\n");
                i++;
                //foreach (Parameter parameter in method.OutputParameters)
                //{
                //    sb.Append(
                //        string.Format("\t\t\t{0} {1}", parameter.Name, 
                //parameter.Type));
                //}
            }




            //Provider.XsdGenerator xs = new Provider.XsdGenerator();

            //List<Provider.TestConnector.Parameter> lstParameters = new List<Provider.TestConnector.Parameter>();
            //lstParameters.Add(new Provider.TestConnector.Parameter
            //{
            //    Name = "CustomerId",
            //    Value = "ABC123"
            //});

            //client = new Provider.TestConnector
            //{
            //    Parameters = lstParameters,
            //    Url = "http://localhost/Sofka.Automation.Dummy.Wcf/Loan.svc",
            //    WSServiceType = Provider.TestConnector.ServiceType.WCF,
            //    WCFContractName = "ILoan",
            //    WebMethod = "Prueba"
            //};


            //client.BeginInvokeService(InvokeCompleted);

            //testDataAccess = new DataAccess.Test();
            //loanClient = new LoanClient();
        }

        public static void InvokeCompleted(IAsyncResult result)
        {
            string returnFromService = client.EndInvokeService(result);
        }

        public void RunTest(List<int> idTestCases)
        {
            foreach (var idTestCase in idTestCases)
            {
                TestCase testCase = testDataAccess.GetTestCase(idTestCase);
                bool success = false;

                if (testCase != null && testCase.IdTestCase > 0)
                {
                    LoanRequestRequest request = (LoanRequestRequest)XmlHelper.XmlDeserializeFromString(testCase.Input, typeof(LoanRequestRequest));
                    LoanRequestResponse response = loanClient.LoanRequest(request);

                    success = ValidateTestCase(idTestCase, response);
                }
                else
                {
                    //throw new Exception(string.Format("There is no a test case with the parameter idTestCase = {0}", testCase));
                    success = false;
                }
            }
        }

        private bool ValidateTestCase(int idTestCase, object response)
        {
            List<TestCaseValue> testCaseValues = testDataAccess.GetTestCaseValue(idTestCase);
            string message = string.Empty;

            if (testCaseValues.Count == 0)
            {
                //throw new Exception("Not found test cases values.");
                message = "Not found test cases values.";
            }

            foreach (TestCaseValue testCaseValue in testCaseValues)
            {
                object objResponseValue = response.GetType().GetProperty(testCaseValue.Property).GetValue(response);
                if (objResponseValue != null)
                {
                    string valueResponse = objResponseValue.ToString();

                    if (valueResponse != testCaseValue.Value)
                    {
                        message += string.Format("- Values, test value {0} and value obteined from service {1} do not match for the property {2}, Id Test Value {3}.\r\n",
                            valueResponse,
                            testCaseValue.Value,
                            testCaseValue.Property,
                            testCaseValue.IdTestCase);
                    }
                }
            }

            testDataAccess.SaveResultTestCase(idTestCase, message);

            return string.IsNullOrEmpty(message);
        }
    }
}
