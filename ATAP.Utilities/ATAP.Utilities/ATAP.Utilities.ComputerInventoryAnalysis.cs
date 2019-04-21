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
using ATAP.Utilities.DiskDrive;
using ATAP.Utilities.Filesystem;
using ATAP.Utilities.FileSystem;

namespace ATAP.Utilities.ComputerInventory {

    public class ComputerInventoryAnalysis {
        public ComputerInventoryAnalysis(ILog log = null) {
            Log=log;
        }

        public async Task WalkDiskDriveAndFilesystemAsync(int diskNumber, int asyncFileReadBlocksize, IDiskInfoExsContainer diskInfoExsContainer, WalkDiskDriveResultContainer walkDiskDriveResultContainer, IWalkFilesystemResultContainer walkFilesystemResultContainer, Action<string> recordRoot = null, Action<string[]> recordSubdir = null) {
            Log.Debug($"starting WalkDiskDriveAndFilesystemAsync: diskNumber = {diskNumber}");
            //ToDo: Add some validation to ensure the diskInfoEx has "good" data
            //
            DiskAnalysis.DiskAnalysis dda = new DiskAnalysis.DiskAnalysis(Log);
            await dda.WalkDiskDrive(diskNumber, diskInfoExsContainer, walkDiskDriveResultContainer).ConfigureAwait(false);
            FilesystemAnalysis fsa = new FilesystemAnalysis(Log, asyncFileReadBlocksize);
            foreach (var d in diskInfoExsContainer.DiskInfoExs) {
                foreach (var p in d.PartitionInfoExs) {
                    foreach (var driveLetter in p.DriveLetters) {
                        string root = $"{driveLetter}:/";
                        await fsa.WalkFileSystem(root, walkFilesystemResultContainer, recordRoot, recordSubdir);
                    }
                }
            }
            Log.Debug($"leaving WalkDiskDriveAndFilesystemAsync: diskNumber = {diskNumber}");
        }


        #region Properties
        #region Properties:class logger
        public ILog Log;
        #endregion
        #endregion
    }

}
