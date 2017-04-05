using Sofka.Automation.Entities.Wsdl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofka.Automation.Provider
{
    public class WebMethodInfo
    {
        string _name;
        Parameter[] _inputParameters;
        Parameter[] _outputParameters;

        /// 

        /// OperationInfo
        /// 

        public WebMethodInfo(string name, Parameter[] inputParameters,
             Parameter[] outputParameters)
        {
            _name = name;
            _inputParameters = inputParameters;
            _outputParameters = outputParameters;
        }

        /// 

        /// Name
        /// 

        public string Name
        {
            get { return _name; }
        }

        /// 

        /// InputParameters
        /// 

        public Parameter[] InputParameters
        {
            get { return _inputParameters; }
        }

        /// 

        /// OutputParameters
        /// 

        public Parameter[] OutputParameters
        {
            get { return _outputParameters; }
        }
    }
}
