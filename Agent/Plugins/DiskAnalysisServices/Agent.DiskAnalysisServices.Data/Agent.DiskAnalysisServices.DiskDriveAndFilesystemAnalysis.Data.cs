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
using ATAP.Utilities.FileSystem;

namespace Ace.Agent.DiskAnalysisServices {

    public partial class DiskAnalysisServicesData : IAnalyzeDiskDriveResults,  IAnalyzeFileSystemResults {

        // ToDo: Make the creation of All of the COD's herein Lazy

        #region DiskDriveAnalysis Results
        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo> LookupDiskDriveAnalysisResultsCOD { get; set; }
        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, IAnalyzeDiskDriveResult> AnalyzeDiskDriveResultsCOD { get; set; }
        void ConstructDiskDriveAnalysisData() {
            LookupDiskDriveAnalysisResultsCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo>();
            AnalyzeDiskDriveResultsCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, IAnalyzeDiskDriveResult>();
        }
        public void TearDownDiskDriveAnalysisData() {
            //PluginRootCOD.CollectionChanged-=this.onPluginRootCODCollectionChanged;
            //PluginRootCOD.PropertyChanged-=this.onPluginRootCODPropertyChanged;
        }

        #endregion

        #region FileSystemAnalysis Results
        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo> LookupFileSystemAnalysisResultsCOD { get; set; }
        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, IAnalyzeFileSystemResult> AnalyzeFileSystemResultsCOD { get; set; }
        //public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, FileInfoExs> FileInfoExCOD { get; set; }
        //public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, (Id<DirectoryInfoEx>, Id<FileInfoEx>)> EdgeFileInfoExDirectoryInfoExCOD { get; set; }
        //public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, (Id<DirectoryInfoEx>, Id<DirectoryInfoEx>)> EdgeDirectoryInfoExDirectoryInfoExCOD { get; set; }

        void ConstructFileSystemAnalysisData() {
            LookupFileSystemAnalysisResultsCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo>();
            AnalyzeFileSystemResultsCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, IAnalyzeFileSystemResult>();
            //DirectoryInfoExCOD =new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, DirectoryInfoExs>();
            //FileInfoExCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, FileInfoExs>();
            //EdgeFileInfoExDirectoryInfoExCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, (Id<DirectoryInfoEx>, Id<FileInfoEx>)>();
            //EdgeDirectoryInfoExDirectoryInfoExCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, (Id<DirectoryInfoEx>, Id<DirectoryInfoEx>)>();
        }
        public void TearDownFileSystemAnalysisData() {
            //EdgeDirectoryInfoExDirectoryInfoExCOD.CollectionChanged-=this.onEdgeDirectoryInfoExDirectoryInfoExCODCollectionChanged;
            //EdgeDirectoryInfoExDirectoryInfoExCOD.PropertyChanged-=this.onEdgeDirectoryInfoExDirectoryInfoExCODPropertyChanged;
        }
        #endregion
    }
}


