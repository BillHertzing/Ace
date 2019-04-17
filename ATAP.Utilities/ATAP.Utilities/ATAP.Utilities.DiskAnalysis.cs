using ATAP.Utilities.ComputerInventory;
using ATAP.Utilities.ComputerInventory.Enumerations;
using ATAP.Utilities.Database.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using ServiceStack.Logging;
using ServiceStack.Text;

namespace ATAP.Utilities.DiskAnalysis {


    public class DiskAnalysis {

        public DiskAnalysis(ILog log) {
            Log=log??throw new ArgumentNullException(nameof(log));
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

        public async Task DiskDriveManyToDBGraphAsync(DiskDrivePartitionDriveLetterIdentifier diskDrivePartitionDriveLetterIdentifier, ComputerInventory.ComputerInventory computerInventory, CrudType cRUD, Action<string> recordRoot = null) {
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



        #region Properties
        #region Properties:class logger
        public ILog Log;

        #endregion
        #endregion
    }
}
