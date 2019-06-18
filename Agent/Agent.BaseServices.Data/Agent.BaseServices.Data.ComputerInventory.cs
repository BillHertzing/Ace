using System;
using System.Collections.Specialized;
using System.ComponentModel;
using Serilog;
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
            Log.Debug("in BaseServicesData .ctor: ConstructComputerInventory");
            // Look at the BaseServices configuration settings for options in setting up the initial ComputerInventory
            if (true) {
                ComputerInventory=new ATAP.Utilities.ComputerInventory.ComputerInventory().FromComputerName("localhost");
            } else {
                ComputerInventory=new ATAP.Utilities.ComputerInventory.ComputerInventory().FromConfigurationFile("./ncat-lt02");
            }
        }
        #endregion
    }



}


