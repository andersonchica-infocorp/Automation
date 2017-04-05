using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.Services.Description;
using System.Net;
using System.Xml.Schema;

namespace Sofka.Automation.Provider
{
    public class WebMethodInfoCollection : System.Collections.ObjectModel.KeyedCollection<string, WebMethodInfo>
    {
        public WebMethodInfoCollection() : base() { }

        protected override string GetKeyForItem(WebMethodInfo item)
        {
            return item.Name;
        }
    }
}
