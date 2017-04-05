using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofka.Automation.Entities.Wsdl
{
    public struct Parameter
    {
        /// 

        /// constructor
        /// 

        /// 
        /// 
        public Parameter(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }

        /// 

        /// Name
        /// 

        public string Name;
        /// 

        /// Type
        /// 

        public string Type;
    }
}
