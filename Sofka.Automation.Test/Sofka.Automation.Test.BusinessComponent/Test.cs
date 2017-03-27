using Sofka.Automation.CrossCutting.Utils;
using Sofka.Automation.Entities.Test;
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

        public Test()
        {
            testDataAccess = new DataAccess.Test();
            loanClient = new LoanClient();
        }

        public bool RunTest(int idTestCase)
        {
            //LoanRequestRequest request2 = new LoanRequestRequest
            //{
            //    AmmountRequested = 1000,
            //    Id = 1,
            //    Type = LoanType.Mortgages,
            //};

            //string s =  XmlHelper.SerializeObject(request2);


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

            return success;
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

            if (!string.IsNullOrEmpty(message))
            {
                testDataAccess.SaveResultTestCase(idTestCase, message);
            }

            return string.IsNullOrEmpty(message);
        }
    }
}
