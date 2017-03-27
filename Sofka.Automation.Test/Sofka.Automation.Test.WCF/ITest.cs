﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Sofka.Automation.Test.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITest" in both code and config file together.
    [ServiceContract]
    public interface ITest
    {
        [OperationContract]
        bool RunTest(int idTestCase);
    }
}
