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
        public ComputerInventory ComputerInventory { get; set; }
    }

    public static class BaseServicesDataComputerInventoryExtensions {
        public static ComputerInventory FromComputerName(this ComputerInventory me, string computerName) {
            if (computerName==string.Empty)
                throw new ArgumentNullException(nameof(computerName));
            ComputerHardware lcH;
            ComputerSoftware lcS;
            ComputerProcesses lcP;



            switch (computerName.Trim().ToLowerInvariant()) {
                //ToDo: read actual inventory, currently specific for laptop
                case "localhost":
                    // Get the hardware
                    lcH=new ComputerHardware().FromComputerName(computerName);
                    // ToDo: add "FromComputerName" extension method to ComputerSoftware and ComputerProcesses
                    lcS=me.ComputerSoftware;
                    lcP=me.ComputerProcesses;
                    break;

                default:
                    throw new NotImplementedException($"cannot populate ComputerInventory object from computerName = {computerName}");
            }

            ComputerInventory lCI = new ComputerInventory() { ComputerName=computerName, ComputerHardware=lcH, ComputerSoftware=lcS, ComputerProcesses=lcP };

            return lCI;

        }

    }
}


