using System;
using System.Collections.Specialized;
using System.ComponentModel;
using ServiceStack.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using ATAP.Utilities.ComputerInventory;
using ATAP.Utilities.ComputerHardware.Enumerations;

namespace Ace.Agent.BaseServices {

    public partial class BaseServicesData {
        #region Properties:ConfigurationData
        public ComputerInventory ComputerInventory { get; set; }
        #endregion

        #region IndirectConstructors
        void ConstructComputerInventory() {
            ComputerInventory=new ATAP.Utilities.ComputerInventory.ComputerInventory().FromComputerName("localhost");
        }
        #endregion
    }	



}


