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
using ATAP.Utilities.DiskDrive;
using ATAP.Utilities.FileSystem;

namespace ATAP.Utilities.ComputerInventory {

    // Extension methods to populate a ComputerInventory object

    public class ComputerInventoryAnalysis {
        public ComputerInventoryAnalysis(ILog log = null) {
            Log=log;
        }
        /*
        public async Task WalkDiskDriveAndFilesystemAsync(int diskNumber, int asyncFileReadBlocksize, IDiskDriveInfoExs diskInfoExsContainer, WalkDiskDriveResultContainer walkDiskDriveResultContainer, IWalkFilesystemResultContainer walkFilesystemResultContainer, Action<string> recordRoot = null, Action<string[]> recordSubdir = null) {
            Log.Debug($"starting WalkDiskDriveAndFilesystemAsync: diskNumber = {diskNumber}");
            //ToDo: Add some validation to ensure the diskInfoEx has "good" data
            //
            DiskDriveAnalysis.DiskDriveAnalysis dda = new DiskDriveAnalysis.DiskDriveAnalysis(Log);
            await dda.WalkDiskDrive(diskNumber, diskInfoExsContainer, walkDiskDriveResultContainer).ConfigureAwait(false);
            FileSystemAnalysis fsa = new FileSystemAnalysis(Log, asyncFileReadBlocksize);
            foreach (var d in diskInfoExsContainer.DiskDriveInfoExCOD.Keys) {
                foreach (var p in diskInfoExsContainer.DiskDriveInfoExCOD[d].PartitionInfoExs.PartitionInfoExCOD.Keys) {
                    foreach (var driveLetter in diskInfoExsContainer.DiskDriveInfoExCOD[d].PartitionInfoExs.PartitionInfoExCOD[p].DriveLetters) {
                        string root = $"{driveLetter}:/";
                        await fsa.AnalyzeFileSystem(root, walkFilesystemResultContainer, recordRoot, recordSubdir);
                    }
                }
            }
            Log.Debug($"leaving WalkDiskDriveAndFilesystemAsync: diskNumber = {diskNumber}");
        }
        */

        #region Properties
        #region Properties:class logger
        public ILog Log;
        #endregion
        #endregion
    }

}
