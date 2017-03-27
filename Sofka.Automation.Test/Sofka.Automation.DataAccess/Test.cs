using Sofka.Automation.Entities.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofka.Automation.DataAccess
{
    public class Test
    {
        private DataAccess.Model.TestDataContext context = null;

        public Test()
        {
            context = new Model.TestDataContext();
        }

        public TestCase GetTestCase(int idTestCase)
        {
            var testCase = (from item in context.TestCases
                            where item.IdTestCase == idTestCase
                            select new TestCase
                            {
                                IdTestCase = item.IdTestCase,
                                Input = item.Input,
                                Description = item.Description,
                            }).FirstOrDefault();

            return testCase;
        }

        public List<ResultTestCase> GetResultsTestCase(int idTestCase)
        {
            var resultsTestCase = (from item in context.ResultsTestCases
                                   where item.IdTestCase == idTestCase
                                   select new ResultTestCase
                                   {
                                       IdResult = item.IdResult,
                                       IdTestCase = item.IdTestCase,
                                   }).ToList();

            return resultsTestCase;
        }

        public List<TestCaseValue> GetTestCaseValue(int idTestCase)
        {
            var testCaseValues = (from item in context.TestCaseValues
                                  where item.IdTestCase == idTestCase
                                  select new TestCaseValue
                                  {
                                      IdTestCase = item.IdTestCase,
                                      IdTestCaseValue = item.IdTestCaseValue,
                                      Property = item.Property,
                                      Value = item.Value
                                  }).ToList();

            return testCaseValues;
        }

        public void SaveResultTestCase(int idTestCase, string message)
        {
            Model.ResultsTestCase resultTestCase = new Model.ResultsTestCase
            {
                IdTestCase = idTestCase,
                Message = message,
                Success = string.IsNullOrEmpty(message),
            };

            context.ResultsTestCases.InsertOnSubmit(resultTestCase);

            try
            {
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                //throw new Exception("cant save current result test case");
            }
        }
    }
}
