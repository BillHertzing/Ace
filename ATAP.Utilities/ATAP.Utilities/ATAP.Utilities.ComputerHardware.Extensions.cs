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

    public static class ComputerHardwareExtensions {
        // Extension methods to populate a ComputerHardware object
        public static ATAP.Utilities.ComputerInventory.ComputerHardware FromComputerName(this ATAP.Utilities.ComputerInventory.ComputerHardware me, string computerName) {

            ATAP.Utilities.ComputerInventory.ComputerHardware lCH;
            switch (computerName.Trim().ToLowerInvariant()) {
                //ToDo: read actual HW, currently specific for laptop
                case "localhost":
                    // create the partitions COD for drive 2
                    //Log.Debug("in ComputerHardware.FromComputerName initializer");
                    var partitionInfoExs = new PartitionInfoExs();
                    PartitionInfoEx PIE1 = new PartitionInfoEx() { DriveLetters=new List<string>() { "E" }, Exceptions=new List<Exception>(), PartitionDbId=new Id<PartitionInfoEx>(Guid.Empty), PartitionId=new Id<PartitionInfoEx>(Guid.NewGuid()), Size=1000000000000 };
                    partitionInfoExs.PartitionInfoExCOD.Add(PIE1.PartitionId, PIE1);
                    DiskDriveInfoEx DDIE0 = new DiskDriveInfoEx() {
                        DiskDriveId=new Id<DiskDriveInfoEx>(Guid.NewGuid()),
                        DiskDriveDbId=new Id<DiskDriveInfoEx>(Guid.Empty),
                        DiskDriveMaker=DiskDriveMaker.Generic,
                        DiskDriveType=DiskDriveType.SSD,
                        SerialNumber="123",
                        PartitionInfoExs=partitionInfoExs,
                        Exceptions=new List<Exception>()
                    };
                    var diskDriveInfoExs = new DiskDriveInfoExs();
                    diskDriveInfoExs.DiskDriveInfoExCOD.Add(DDIE0.DiskDriveId, DDIE0);
                    lCH=new ComputerHardware(new MainBoard(MainBoardMaker.Generic, CPUSocket.Generic), new List<CPUMaker> { CPUMaker.Intel }, diskDriveInfoExs.DiskDriveInfoExCOD, new TimeBlock(DateTime.UtcNow, true));
                    break;

                default:
                    throw new NotImplementedException($"cannot populate ComputerHardware object from computerName = {computerName}");
            }

            return lCH;
        }
    }
}

