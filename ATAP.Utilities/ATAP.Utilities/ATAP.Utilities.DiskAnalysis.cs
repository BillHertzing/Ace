using ATAP.Utilities.ComputerInventory;
using ATAP.Utilities.ComputerInventory.Enumerations;
using ATAP.Utilities.Database.Enumerations;
using ServiceStack.Logging;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ATAP.Utilities.DiskAnalysis {

    public class DiskAnalysis {
        public DiskAnalysis() { }

        public async Task DiskDriveManyToDBGraphAsync(DiskDrivePartitionDriveLetterIdentifier diskDrivePartitionDriveLetterIdentifier, ComputerInventory.ConmputerInventory computerInventory, CrudType cRUD, Action<string> recordRoot = null) {
            Log.Debug($"starting DiskDriveToDBGraphAsync: diskDrivePartitionDriveLetterIdentifier = {diskDrivePartitionDriveLetterIdentifier.Dump()} cRUD = {cRUD.ToString()}");

            await PopulateDiskInfoExs(cRUD).ContinueWith((c) => ReadDiskAsync(cRUD, diskInfoEx));
            Log.Debug($"leaving DiskDriveToDBGraphAsync outer");
        }

        // ToDo: Add an optional parameter for a Func delegate that will query/update the DB for the FSEntities based on the value of cRUD
        public async Task DiskDriveSingleToDBGraphAsync(string cRUD, DiskInfoEx diskInfoEx, Action<string> recordRoot = null) {
            Log.Debug($"starting ReadDiskAsync: cRUD = {cRUD} diskInfoEx = {diskInfoEx}");
            //ToDo: Add some validation to ensure the diskInfoEx has "good" data
            AnalyzeOneDiskDriveResult readDiskResult = new AnalyzeOneDiskDriveResult();

            foreach (var p in readDiskResult.PartitionInfoExs) {
                foreach (var driveLetter in p.DriveLetters) {
                    string root = $"{driveLetter}:/";
                    // ToDo: Pass in a Func that will query/update the DB based on the value of cRUD
                    AnalyzeOneDiskDriveResult readDiskResultInner = await GetFSEntities(root, recordRoot);
                    readDiskResult.NodeInserts.AddRange(readDiskResultInner.NodeInserts);
                    //readDiskResult.EdgeInserts = readDiskResultInner.EdgeInserts;
                    readDiskResult.Exceptions.AddRange(readDiskResultInner.Exceptions);
                }
            }
            Log.Debug($"leaving ReadDiskAsync inner");
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


        #region Properties
        #region Properties:class logger
        public ILog Log;
        #endregion
        #endregion
    }
}
