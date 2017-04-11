using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofka.Automation.Entities.Wsdl
{
    public struct Parameter
    {       
        public Parameter(string name, string type, object value, string NameSpace)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
            this.NameSpace = NameSpace;
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public object Value { get; set; }

        public string NameSpace { get; set; }
    }
}
