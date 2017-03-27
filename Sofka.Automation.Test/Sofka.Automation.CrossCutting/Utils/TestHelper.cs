using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofka.Automation.CrossCutting.Utils
{
    public static class TestHelper
    {
        public static object GetPropertyValue(object entity, string propertyName)
        {
            return entity.GetType().GetProperty(propertyName).GetValue(entity, null);
        }
    }
}
