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
            LoanRequestRequest e = new LoanRequestRequest() {
                AmmountRequested = 324234,
                Direccion = new Direccion {
                    Carrera = "sdfsdf",
                    Latitude = new Latitude {
                        Longitud = 23123
                    }
                },
                Id = 12,
                Type = LoanType.Car
            };

            var x = CrossCutting.Utils.XmlHelper.SerializeObject(e);

            //var a = e.GetType();
            //Activator.CreateInstance(a.AssemblyQualifiedName, a.q)

            //CrossCutting.Utils.XmlHelper.SerializeObject<>
            //using (var client = new WebClient())
            //{
            //    Automation.DataAccess.Test testDataAccess = new Automation.DataAccess.Test();
            //    TestCase testCase = testDataAccess.GetTestCase(6);

            //    XmlDocument xml = new XmlDocument();
            //    xml.LoadXml(testCase.Input);
            //    //xml.FirstChild.Value.GetType();


            //    // read the raw SOAP request message from a file
            //    //var data = File.ReadAllText(testCase.Input);
            //    // the Content-Type needs to be set to XML
            //    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
            //    // The SOAPAction header indicates which method you would like to invoke
            //    // and could be seen in the WSDL: <soap:operation soapAction="..." /> element
            //    client.Headers.Add("SOAPAction", "http://tempuri.org/ILoan/LoanRequest");
            //    var response = client.UploadString("http://localhost/Sofka.Automation.Dummy.Wcf/Loan.svc", testCase.Input);
            //    Console.WriteLine(response);
            //}

            Provider.XsdGenerator asas = new XsdGenerator();

            //Automation.DataAccess.Test testDataAccess = new Automation.DataAccess.Test();
            //TestCase testCase = testDataAccess.GetTestCase(6);

            //client = new Provider.TestConnector
            //{
            //    Request = testCase.Input,
            //    Url = "http://localhost/Sofka.Automation.Dummy.Wcf/Loan.svc",
            //    WSServiceType = Provider.TestConnector.ServiceType.WCF,
            //    WCFContractName = "ILoan",
            //    WebMethod = "LoanRequest"
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
