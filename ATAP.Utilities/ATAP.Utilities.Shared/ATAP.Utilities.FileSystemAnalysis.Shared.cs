using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.DiskDrive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ATAP.Utilities.ConcurrentObservableCollections;

namespace ATAP.Utilities.FileSystem {
    /// <summary>
    /// Data Structure to hold the results of an AnalyzerFileSystem method call
    /// </summary>
    public interface IAnalyzeFileSystemResult {
        IDirectoryInfoExs DirectoryInfoExs { get; set; }
        IFileInfoExs FileInfoExs { get; set; }
        IList<Exception> Exceptions { get; set; }
        //   IEdgeFileInfoExDirectoryInfoExs EdgeFileInfoExDirectoryInfoExs { get; set; }
        //   IEdgeDirectoryInfoExDirectoryInfoExs EdgeDirectoryInfoExDirectoryInfoExs { get; set; }


        //      ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo> LookupFileSystemAnalysisResultsCOD { get; set; }
        //       ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, DirectoryInfoExs> DirectoryInfoExCOD { get; set; }
        //      ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, FileInfoExs> FileInfoExCOD { get; set; }
        //      ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, (Id<DirectoryInfoEx>, Id<FileInfoEx>)> EdgeFileInfoExDirectoryInfoExCOD { get; set; }
    }

    #region AnalyzeFileSystem
    public class AnalyzeFileSystemResult : IAnalyzeFileSystemResult {
        public AnalyzeFileSystemResult() : this(new DirectoryInfoExs(), new FileInfoExs(), new List<Exception>()) { }

        public AnalyzeFileSystemResult(IDirectoryInfoExs directoryInfoExs, IFileInfoExs fileInfoExs, IList<Exception> exceptions) {
            DirectoryInfoExs=directoryInfoExs;
            FileInfoExs=fileInfoExs;
            Exceptions=exceptions;
        }

        public IDirectoryInfoExs DirectoryInfoExs { get; set; }
        public IFileInfoExs FileInfoExs { get; set; }
        public IList<Exception> Exceptions { get; set; }
    }

    public interface IAnalyzeFileSystemResults {
        ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, IAnalyzeFileSystemResult> AnalyzeFileSystemResultsCOD { get; set; }
    }

    public class AnalyzeFileSystemResults : IAnalyzeFileSystemResults {
        public AnalyzeFileSystemResults() : this(new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, IAnalyzeFileSystemResult>() { { new Id<LongRunningTaskInfo>(), new AnalyzeFileSystemResult() } }) { }

        public AnalyzeFileSystemResults(ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, IAnalyzeFileSystemResult> AnalyzeFileSystemResultsCOD) {
            this.AnalyzeFileSystemResultsCOD=AnalyzeFileSystemResultsCOD;
        }

        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, IAnalyzeFileSystemResult> AnalyzeFileSystemResultsCOD { get; set; }
    }

    public interface IAnalyzeFileSystemProgress {
        bool Completed { get; set; }
        int DeepestDirectoryTree { get; set; }
        IList<Exception> Exceptions { get; set; }
        long LargestFile { get; set; }
        int NumberOfDirectories { get; set; }
        int NumberOfFiles { get; set; }
    }


    public class AnalyzeFileSystemProgress : IAnalyzeFileSystemProgress {
        public AnalyzeFileSystemProgress() : this(false, -1, -1, -1, -1, new List<Exception>()) { }

        public AnalyzeFileSystemProgress(bool completed, int numberOfDirectories, int numberOfFiles, int deepestDirectoryTree, long largestFile, IList<Exception> exceptions) {
            Completed=completed;
            NumberOfDirectories=numberOfDirectories;
            NumberOfFiles=numberOfFiles;
            DeepestDirectoryTree=deepestDirectoryTree;
            LargestFile=largestFile;
            Exceptions=exceptions;
        }
        public bool Completed { get; set; }
        public int NumberOfDirectories { get; set; }
        public int NumberOfFiles { get; set; }
        public int DeepestDirectoryTree { get; set; }
        public long LargestFile { get; set; }
        public IList<Exception> Exceptions { get; set; }
    }
    #endregion
}
