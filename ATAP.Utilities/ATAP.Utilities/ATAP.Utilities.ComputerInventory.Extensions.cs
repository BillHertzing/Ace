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
using ATAP.Utilities.Filesystem;
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

    }

 
}
/*
 * 
  // Created on demand to match a specific computerName
        public ComputerHardware(string computerName) {
            if (!computerName.Trim().ToLowerInvariant().Equals("localhost"))
                throw new NotImplementedException("ComputerName other than localhost is not supported");
            // ToDo: Query WMI or Configuration data for real details
            // Temp: hardcode for laptop
            // Drive 0 for laptop
            DiskDriveInfoEx DDIE0 = new DiskDriveInfoEx() {
                DiskDriveId=new Id<DiskDriveInfoEx>(Guid.NewGuid()),
            DiskDriveDbId =new Id<DiskDriveInfoEx>(Guid.Empty) ,
            };
            var diskDriveInfoExs = new DiskDriveInfoExs() { new DiskDriveInfoEx(), new DiskDriveInfoEx(), new DiskDriveInfoEx() };
            new ComputerHardware(new MainBoard(MainBoardMaker.Generic, CPUSocket.Generic), new List<CPUMaker> { CPUMaker.Intel }, diskInfoExs, new TimeBlock(DateTime.UtcNow, true));
        }
 * */
