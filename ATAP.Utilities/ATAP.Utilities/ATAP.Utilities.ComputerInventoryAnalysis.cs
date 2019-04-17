using ATAP.Utilities.ComputerInventory.Enumerations;
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
// ToDo: figure out logging for the ATAP libraires, this is only temporary
using ServiceStack.Logging;
namespace ATAP.Utilities.ComputerInventory {

    public class ComputerInventoryAnalysis {
        public ComputerInventoryAnalysis(ILog log = null) {
            Log=log;
        }

        // populate a list of DiskInfoEx with information about the actual drives connected to the computer
        // pass in an Action that will populate a storage location with the information
        public async Task PopulateDiskInfoExs(IEnumerable<int> diskNumbers, CrudType cRUD, Action<IEnumerable<DiskInfoEx>> storeDiskInfoExs, Action<DiskInfoEx> storePartitionInfoExs) {
            Log.Debug($"starting PopulateDiskInfoExs: cRUD = {cRUD.ToString()}, diskNumbers = {diskNumbers.Dump()}");
            var diskInfoExs = new List<DiskInfoEx>();
            foreach (var d in diskNumbers) {
                var diskInfoEx = new DiskInfoEx();
                // ToDo: get from current ComputerInventory
                diskInfoEx.DriveNumber=d;
                diskInfoEx.DiskDriveMaker=DiskDriveMaker.Generic; // ToDo: read disk diskDriveMaker via WMI
                diskInfoEx.DiskDriveType=DiskDriveType.Generic; // ToDo: read disk diskDriveType via WMI
                diskInfoEx.SerialNumber="DummyDiskSerialNumber"; // ToDo: read disk serial number via WMI
                await PopulatePartitionInfoExs(cRUD, diskInfoEx, storePartitionInfoExs); // wait for the 
                diskInfoExs.Add(diskInfoEx);
            }

            // Store the information using the Action delegate
            storeDiskInfoExs.Invoke(diskInfoExs);
            // Todo: see if the DiskDriveMaker and SerialNumber already exist in the DB
            // async (cRUD, diskInfoEx) => { await Task.Yield(); }
            // Task< DiskInfoEx> t = await DBFetch.Invoke(cRUD, diskInfoEx);
            // diskInfoEx = await DBFetch.Invoke(cRUD, diskInfoEx);
            Log.Debug($"leaving PopulateDiskInfoExs");
        }

        // populate a list of PartitionInfoEx with information about the actual partitions on a DiskDrive
        // pass in an Action that will populate a storage location with the information
        public async Task PopulatePartitionInfoExs(CrudType cRUD, DiskInfoEx diskInfoEx, Action<DiskInfoEx> storePartitionInfoExs) {
            Log.Debug($"starting PopulatePartitionInfoExs: cRUD = {cRUD.ToString()}, diskInfoEx = {diskInfoEx.Dump()}");

            // ToDo: Get the list of partitions from the Disk hardware
            await new Task(() => Thread.Sleep(500));
            // Until real partitions are available, mock up the current laptop configuration as of 4/15/19
            // No partitions on drives 0 and 1, and one partition on drive 2, one drive letter E
            var partitionInfoExs = new List<PartitionInfoEx>();
            switch (diskInfoEx.DriveNumber) {
                case 2: {
                    var partitionInfoEx = new PartitionInfoEx() {
                        PartitionIdentityId=0,
                        PartitionGuid=Guid.NewGuid(),
                        DriveLetters=new List<string>() { "E" }
                    };
                    partitionInfoExs.Add(partitionInfoEx);
                    break;
                }
                default:
                    break;
            }
            storePartitionInfoExs.Invoke(diskInfoEx);
            // ToDo: see if the disk already has partitions in the DB
            var dBPartitions = new List<PartitionInfoEx>();

            Log.Debug($"leaving PopulatePartitionInfoExs");
        }

        #region Properties
        #region Properties:class logger
        public ILog Log;
        #endregion
        #endregion
    }

}
