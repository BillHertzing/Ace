using System;
using System.Collections.Specialized;
using System.ComponentModel;
using ServiceStack.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using ATAP.Utilities.ComputerInventory;
using ATAP.Utilities.ComputerHardware.Enumerations;
using ATAP.Utilities.DiskDrive;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.LongRunningTasks;
using Swordfish.NET.Collections;
using ATAP.Utilities.Filesystem;

namespace Ace.Agent.DiskAnalysisServices {

    public partial class DiskAnalysisServicesData :IDiskDriveAnalysisResult {

        // ToDo: Make the creation of All of the COD's herein Lazy

        #region DiskDriveAnalysis Results
        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo> LookupDiskDriveAnalysisResultsCOD { get; set; }

         public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, DiskDriveInfoExs> DiskDriveInfoExCOD { get; set; }
        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, PartitionInfoExs> PartitionInfoExCOD { get; set; }

        public IList<Exception> DiskDriveAnalysisExceptions { get; set; }
        //public DiskDriveInfoExs DiskDriveAnalysisDiskDriveInfoExs { get; set; }

        #endregion

        #region FilesystemAnalysis Results
        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo> LookupFilesystemAnalysisResultsCOD { get; set; }
        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, DirectoryInfoExs> DirectoryInfoExCOD { get; set; }
        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, FileInfoExs> FileInfoExCOD { get; set; }
        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, (Id<DirectoryInfoEx>, Id<FileInfoEx>)> EdgeFileInfoExDirectoryInfoExCOD { get; set; }

        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, (Id<DirectoryInfoEx>, Id<DirectoryInfoEx>)> EdgeDirectoryInfoExDirectoryInfoExCOD { get; set; }
        #endregion
        void ConstructFilesystemAnalysisData() {
            LookupFilesystemAnalysisResultsCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo>();
            DirectoryInfoExCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, DirectoryInfoExs>();
            FileInfoExCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, FileInfoExs>();
            EdgeFileInfoExDirectoryInfoExCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, (Id<DirectoryInfoEx>, Id<FileInfoEx>)>();
            EdgeDirectoryInfoExDirectoryInfoExCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, (Id<DirectoryInfoEx>, Id<DirectoryInfoEx>)>();

        }
        void ConstructDiskDriveAnalysisData() {
            LookupDiskDriveAnalysisResultsCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo>();
            DiskDriveInfoExCOD = new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, DiskDriveInfoExs>();
            PartitionInfoExCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, PartitionInfoExs>();

        }

        public void TearDownDiskDriveAnalysisData() {
            //PluginRootCOD.CollectionChanged-=this.onPluginRootCODCollectionChanged;
            //PluginRootCOD.PropertyChanged-=this.onPluginRootCODPropertyChanged;
        }

        public void TearDownFilesystemAnalysisData() {
            //EdgeDirectoryInfoExDirectoryInfoExCOD.CollectionChanged-=this.onEdgeDirectoryInfoExDirectoryInfoExCODCollectionChanged;
            //EdgeDirectoryInfoExDirectoryInfoExCOD.PropertyChanged-=this.onEdgeDirectoryInfoExDirectoryInfoExCODPropertyChanged;
        }
    }
}


