using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack.Logging;
using ServiceStack.Text;
using ATAP.Utilities.ComputerInventory;
using ATAP.Utilities.ComputerHardware.Enumerations;
using ATAP.Utilities.Database.Enumerations;
using ATAP.Utilities.DiskDrive;
using ATAP.Utilities.TypedGuids;

namespace ATAP.Utilities.DiskDriveAnalysis {


    public class DiskDriveAnalysis {
        public static class ExceptionErrorMessages {
            // ToDo: eventually localize these
            public const string Placeholder = "placeholder";
        }
        public DiskDriveAnalysis(ILog log) {
            Log=log??throw new ArgumentNullException(nameof(log));
        }

        // All of these async method will periodically execute instructions to the underlying database
        //  For that reasons, they all take some optional action parameters


        // populate a list of DiskDriveInfoEx with information about the actual drives connected to the computer
        // pass in an Action that will populate a storage location with the information
        public async Task PopulateDiskInfoExs(IEnumerable<int?> diskNumbers, CrudType cRUD, Action<IEnumerable<DiskDriveInfoEx>> storeDiskInfoExs, Action<DiskDriveInfoEx> storePartitionInfoExs) {
            Log.Debug($"starting PopulateDiskInfoExs: cRUD = {cRUD.ToString()}, diskNumbers = {diskNumbers.Dump()}");
            var diskInfoExs = new List<DiskDriveInfoEx>();
            foreach (var d in diskNumbers) {
                var diskInfoEx = new DiskDriveInfoEx();
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
            // Task< DiskDriveInfoEx> t = await DBFetch.Invoke(cRUD, diskInfoEx);
            // diskInfoEx = await DBFetch.Invoke(cRUD, diskInfoEx);
        }

        // populate a list of PartitionInfoEx with information about the actual partitions on a DiskDrive
        // pass in an Action that will populate a storage location with the information
        public async Task PopulatePartitionInfoExs(CrudType cRUD, DiskDriveInfoEx diskInfoEx, Action<DiskDriveInfoEx> storePartitionInfoExs) {

            // ToDo: Get the list of partitions from the Disk hardware
            await new Task(() => Thread.Sleep(500));
            // Until real partitions are available, mock up the current laptop configuration as of 4/15/19
            // No partitions on drives 0 and 1, and one partition on drive 2, one drive letter E
            var partitionInfoExs = new List<PartitionInfoEx>();
            switch (diskInfoEx.DriveNumber) {
                case 2: {
                    var partitionInfoEx = new PartitionInfoEx() {
                        PartitionDbId=new Id<PartitionInfoEx>(),
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
        }

        // C/R/U/D a list of DiskDriveInfoEx with data in a DB
        // pass in an Action that will interact with the DB
        public async Task DiskInfoExsToDB(List<DiskDriveInfoEx> diskInfoExs, CrudType cRUD, Func<CrudType, DiskDriveInfoEx, Task> interact) {
            foreach (var diskInfoEx in diskInfoExs) {
                // invoke the Action delegate to C/R/U/D each DiskDriveInfoEx to the DB
                await interact.Invoke(cRUD, diskInfoEx);
            }
        }
        public async Task AnalyzeDiskDrive(IDiskDriveSpecifier diskDriveSpecifier, IAnalyzeDiskDriveResult diskDriveAnalysisResult, IAnalyzeDiskDriveProgress diskDriveAnalysisProgress, CancellationToken cancellationToken, Action<CrudType, string> recordDiskInfoEx = null, Action<CrudType, string[]> recordPartitionInfosEx = null) {
            // ToDo: Add validation to ensure the diskDriveSpecifier corresponds to a valid member of the DiskInfoExs 
            Task task = new Task(() => { Thread.Sleep(1); }, cancellationToken);
        }

        /*
        public async Task WalkDiskDrive(DiskDrivePartitionIdentifier diskDrivePartitionIdentifier, IDiskDriveInfoExs diskInfoExsContainer, WalkDiskDriveResultContainer walkDiskDriveResultContainer,  Action<CrudType , string> recordDiskInfoEx = null, Action<CrudType, string[]> recordPartitionInfosEx = null) {
           // await PopulateDiskInfoExs().ContinueWith((c) => ReadDiskAsync(diskInfoEx));
        }
        */

        // ToDo: Add an optional parameter for a Func delegate that will query/update the DB for the FSEntities based on the value of cRUD
        //  The instance of the CRUD specifier returns different Actions based on a case of the CRUD enumeration variable



        #region Properties
        #region Properties:class logger
        public ILog Log;

        #endregion
        #endregion
    }
}
