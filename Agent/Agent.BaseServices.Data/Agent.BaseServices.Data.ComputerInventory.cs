using System;
using System.Collections.Specialized;
using System.ComponentModel;
using ServiceStack.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using ATAP.Utilities.ComputerInventory;
using ATAP.Utilities.ComputerInventory.Enumerations;

namespace Ace.Agent.BaseServices {

    public partial class BaseServicesData {
        ComputerInventory ComputerInventory { get; set; }
    }
}


