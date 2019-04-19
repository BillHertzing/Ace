using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack.Logging;
using ServiceStack.Text;
using ATAP.Utilities.ComputerInventory;
using ATAP.Utilities.ComputerInventory.Enumerations;
using ATAP.Utilities.Database.Enumerations;
using ATAP.Utilities.DiskDrive;
using ATAP.Utilities.DiskDrive.Enumerations;
using ATAP.Utilities.TypedGuids;

namespace ATAP.Utilities.DiskAnalysis {


    public class DiskAnalysis {

        public DiskAnalysis(ILog log) {
            Log=log??throw new ArgumentNullException(nameof(log));
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
                        PartitionDbId = new Id<PartitionInfoEx>(),
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


        // C/R/U/D a list of DiskInfoEx with data in a DB
        // pass in an Action that will interact with the DB
        public async Task DiskInfoExsToDB(List<DiskInfoEx> diskInfoExs, CrudType cRUD, Func<CrudType, DiskInfoEx, Task> interact) {
            Log.Debug($"starting DiskInfoExsToDB: cRUD = {cRUD.ToString()}, diskInfoExs = {diskInfoExs.Dump()}");
            foreach (var diskInfoEx in diskInfoExs) {
                // invoke the Action delegate to C/R/U/D each DiskInfoEx to the DB
                await interact.Invoke(cRUD, diskInfoEx);
            }
            Log.Debug($"leaving DiskInfoExsToDB");
        }

        public async Task WalkDiskDrive(int diskDriveNumber, IDiskInfoExsContainer diskInfoExsContainer, WalkDiskDriveResultContainer walkDiskDriveResultContainer, Action<CrudType, string> recordDiskInfoEx = null, Action<CrudType,string[]> recordPartitionInfosEx = null) {
            Log.Debug($"starting WalkDiskDrive: DiskDriveNumber = {diskDriveNumber}");
            Log.Debug($"leaving WalkDiskDrive: DiskDriveNumber = {diskDriveNumber}");
        }

        public async Task WalkDiskDrive(string computerName, IDiskInfoExsContainer diskInfoExsContainer, WalkDiskDriveResultContainer walkDiskDriveResultContainer, Action<CrudType, string> recordDiskInfoEx = null, Action<CrudType, string[]> recordPartitionInfosEx = null) {
            Log.Debug($"starting WalkDiskDrive: ComputerName = {computerName}");
            Log.Debug($"leaving WalkDiskDrive: ComputerName = {computerName}");
        }

        public async Task WalkDiskDrive(DiskDrivePartitionIdentifier diskDrivePartitionIdentifier, IDiskInfoExsContainer diskInfoExsContainer, WalkDiskDriveResultContainer walkDiskDriveResultContainer,  Action<CrudType , string> recordDiskInfoEx = null, Action<CrudType, string[]> recordPartitionInfosEx = null) {
            Log.Debug($"starting WalkDiskDrive: diskDrivePartitionDriveLetterIdentifier = {diskDrivePartitionIdentifier.Dump()}");
            // ToDo: Add validation to ensure the diskDrivePartitionIdentifier correponds to a valid member of the DiskInfoExs 
           // await PopulateDiskInfoExs().ContinueWith((c) => ReadDiskAsync(diskInfoEx));
            Log.Debug($"leaving WalkDiskDrive: diskDrivePartitionDriveLetterIdentifier = {diskDrivePartitionIdentifier.Dump()}");
        }

        // ToDo: Add an optional parameter for a Func delegate that will query/update the DB for the FSEntities based on the value of cRUD
       




        #region Properties
        #region Properties:class logger
        public ILog Log;

        #endregion
        #endregion
    }
}