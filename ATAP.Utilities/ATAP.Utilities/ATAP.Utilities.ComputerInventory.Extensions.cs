using ATAP.Utilities.ComputerHardware.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using Itenso.TimePeriod;
using System.Threading.Tasks;
using System.Threading;
using ATAP.Utilities.Database.Enumerations;
// for .Dump utility
using ServiceStack.Text;
// ToDo: figure out logging for the ATAP libraries, this is only temporary
using ServiceStack.Logging;
using ATAP.Utilities.ComputerHardware;
using ATAP.Utilities.DiskDrive;
using ATAP.Utilities.FileSystem;
using ATAP.Utilities.TypedGuids;

namespace ATAP.Utilities.ComputerInventory {

    public static class ComputerInventoryExtensions {
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

        //ToDo: make async versions
        public static ComputerInventory FromConfigurationFile(this ComputerInventory me, string path) {
            if (path==string.Empty)
                throw new ArgumentNullException(nameof(path));
            string computerName;
            ComputerHardware lcH;
            ComputerSoftware lcS;
            ComputerProcesses lcP;

            // Use JSON as the default structure
            //var computerInventory = FromJson<ComputerInventory>();
            // Just read the file into a 
            // If using XML, 
            // open the path as a stream, setup async buffered read, read into a structured format, catch exceptions
            // Get the hardware by selecting a node range in the structured format 
            // lcH=new ComputerHardware().FromStructuredFormat(path);
            // ToDo: add "FromComputerName" extension method to ComputerSoftware and ComputerProcesses
            lcH=me.ComputerHardware;
            lcS=me.ComputerSoftware;
            lcP=me.ComputerProcesses;
            computerName=me.ComputerName;
            return new ComputerInventory(lcH, lcS, lcP, computerName);
        }
    }
 
}

