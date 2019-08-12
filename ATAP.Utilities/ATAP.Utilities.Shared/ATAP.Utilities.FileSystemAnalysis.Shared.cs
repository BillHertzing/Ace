using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.DiskDrive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ATAP.Utilities.ConcurrentObservableCollections;
using ServiceStack;

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



    #region Base of the Message and Persistent. the POCOs used in Service DTOs and ORM dBCmds
    public interface IRecordRootBuilder<TReq, TRsp> where TReq : IRecordRootReqPOCO where TRsp : IRecordRootRspPOCO {
        IRecordRootBuilder<TReq, TRsp> AddLambda(Func<TReq, TRsp> lambda);
        IRecordRootBuilder<TReq, TRsp> AddORMInstance(string oRMInstance);
        Func<TReq, TRsp> Build();
    }

    public class RecordRootBuilder<TReq, TRsp> : IRecordRootBuilder<TReq, TRsp> where TReq : IRecordRootReqPOCO where TRsp : IRecordRootRspPOCO  {
        public RecordRootBuilder(Type reqType, Type rspType) {
            ReqType=reqType;
            RspType=rspType;
        }
        string ORMInstance { get; set; }
        Type ReqType { get; set; }
        Type RspType { get; set; }
        Func<TReq, TRsp> Lambda { get; set; }
        public IRecordRootBuilder<TReq, TRsp> AddORMInstance(string oRMInstance) {
            this.ORMInstance=oRMInstance;
            return this;
        }
        public IRecordRootBuilder<TReq, TRsp> AddLambda(Func<TReq, TRsp> lambda) {
            this.Lambda=lambda;
            return this;
        }
        public Func<TReq, TRsp> Build() {
            return this.Lambda;
        }
    }

    public interface IRecordFSEntitiesBuilder<TReq, TRsp> where TReq : IRecordFSEntitiesReqPOCO where TRsp : IRecordFSEntitiesRspPOCO {
        IRecordFSEntitiesBuilder<TReq, TRsp> AddLambda(Func<TReq, TRsp> lambda);

        IRecordFSEntitiesBuilder<TReq, TRsp> AddORMInstance(string oRMInstance);
        Func<TReq, TRsp> Build();
    }

    public class RecordFSEntitiesBuilder<TReq, TRsp> : IRecordFSEntitiesBuilder<TReq, TRsp> where TReq : IRecordFSEntitiesReqPOCO where TRsp : IRecordFSEntitiesRspPOCO {
        public RecordFSEntitiesBuilder(Type reqType, Type rspType) {
            ReqType=reqType;
            RspType=rspType;
        }
        string ORMInstance { get; set; }
        Type ReqType { get; set; }
        Type RspType { get; set; }
        Func<TReq, TRsp> Lambda { get; set; }
        public IRecordFSEntitiesBuilder<TReq, TRsp> AddORMInstance(string oRMInstance) {
            this.ORMInstance=oRMInstance;
            return this;
        }
        public IRecordFSEntitiesBuilder<TReq, TRsp> AddLambda(Func<TReq, TRsp> lambda) {
            this.Lambda=lambda;
            return this;
        }
        public Func<TReq, TRsp> Build() {
            return this.Lambda;
        }
    }

    //public interface IFileSystemAnalysis {
    //    Func<TReq, TRsp> RecordRoot { get; set; }
    //    Func<TReq, TRsp> RecordFSEntities { get; set; }
    //}

    #endregion

    #region interface definitions for the RecordRoot POCO and recordFSEntities POCO, and derivitives for ORM and for Message derived types
    public interface IRecordRootReqPOCO {
        DirectoryInfoEx DirectoryInfoEx { get; set; }
    }

    public interface IRecordRootReqDTO : IRecordRootReqPOCO {
    }

    public interface IRecordRootReqDTOMessage : IRecordRootReqDTO {
        // ToDo: Message-Specific attributes to properties
    }

    public interface IRecordRootReqDTOORM : IRecordRootReqDTO {
        // ToDo: ORM-Specific attributes to properties
    }

    public interface IRecordRootRspPOCO {
        string Result { get; set; }
        bool Success { get; set; }
        string TransactionID { get; set; }
    }

    public interface IRecordRootRspDTO : IRecordRootRspPOCO {
    }

    public interface IRecordRootRspDTOMessage : IRecordRootRspDTO {
        // ToDo: Message-Specific attributes to properties
    }

    public interface IRecordRootRspDTOORM : IRecordRootRspDTO {
        // ToDo: ORM-Specific attributes to properties
    }

    public interface IRecordFSEntitiesReqPOCO {
        public DirectoryInfoEx ParentDirectoryInfoEx { get; set; }
        public DirectoryInfoExs DirectoryInfoExs { get; set; }
        public FileInfoExs FileInfoExs { get; set; }
        string TransactionID { get; set; }
    }

    public interface IRecordFSEntitiesReqDTO : IRecordFSEntitiesReqPOCO {
    }

    public interface IRecordFSEntitiesReqDTOMessage : IRecordFSEntitiesReqDTO {
        // ToDo: Message-Specific attributes to properties
    }

    public interface IRecordFSEntitiesReqDTOORM : IRecordFSEntitiesReqDTO {
        // ToDo: ORM-Specific attributes to properties
    }

    public interface IRecordFSEntitiesRspPOCO {
        string Result { get; set; }
        bool Success { get; set; }
    }

    public interface IRecordFSEntitiesRspDTO : IRecordFSEntitiesRspPOCO {
    }

    public interface IRecordFSEntitiesRspDTOMessage : IRecordFSEntitiesRspDTO {
        // ToDo: Message-Specific attributes to properties
    }

    public interface IRecordFSEntitiesRspDTOORM : IRecordFSEntitiesRspDTO {
        // ToDo: ORM-Specific attributes to properties
    }

    #endregion

    #region class definitions for the RecordRoot POCO, and derivitives for ORM and for Message derived types
    public class RecordRootReqPOCO : IRecordRootReqPOCO {
        public DirectoryInfoEx DirectoryInfoEx { get; set; }
    }

    public class RecordRootReqDTO : RecordRootReqPOCO, IRecordRootReqDTO {
    }

    public class RecordRootReqDTOMessage : RecordRootReqDTO, IRecordRootReqDTOMessage {
        // ToDo: Message-Specific attributes to properties
    }

    public class RecordRootReqDTOORM : RecordRootReqDTO, IRecordRootReqDTOORM {
        // ToDo: ORM-Specific attributes to properties
    }

    public class RecordRootRspPOCO : IRecordRootRspPOCO {
        public string Result { get; set; }
        public bool Success { get; set; }
        public string TransactionID { get; set; }
    }

    public class RecordRootRspDTO : RecordRootRspPOCO, IRecordRootRspDTO {
    }

    public class RecordRootRspDTOMessage : RecordRootRspDTO, IRecordRootRspDTOMessage {
        // ToDo: Message-Specific attributes to properties
    }

    public class RecordRootRspDTOORM : RecordRootRspDTO, IRecordRootRspDTOORM {
        // ToDo: ORM-Specific attributes to properties
    }

    public class RecordFSEntitiesReqPOCO {
        public DirectoryInfoEx ParentDirectoryInfoEx { get; set; }
        public DirectoryInfoExs DirectoryInfoExs { get; set; }
        public FileInfoExs FileInfoExs { get; set; }
        public string TransactionID { get; set; }
    }

    public class RecordFSEntitiesReqDTO : RecordFSEntitiesReqPOCO, IRecordFSEntitiesReqDTO {
    }

    public class RecordFSEntitiesReqDTOMessage : RecordFSEntitiesReqDTO, IRecordFSEntitiesReqDTOMessage {
        // ToDo: Message-Specific attributes to properties
    }

    public class RecordFSEntitiesReqDTOORM : RecordFSEntitiesReqDTO, IRecordFSEntitiesReqDTOORM {
        // ToDo: ORM-Specific attributes to properties
    }

    public class RecordFSEntitiesRspPOCO : IRecordFSEntitiesRspPOCO {
        public string Result { get; set; }
        public bool Success { get; set; }
    }

    public class RecordFSEntitiesRspDTO : RecordFSEntitiesRspPOCO, IRecordFSEntitiesRspDTO {
    }

    public class RecordFSEntitiesRspDTOMessage : RecordFSEntitiesRspDTO, IRecordFSEntitiesRspDTOMessage {
        // ToDo: Message-Specific attributes to properties
    }

    public class RecordFSEntitiesRspDTOORM : RecordFSEntitiesRspDTO, IRecordFSEntitiesRspDTOORM {
        // ToDo: ORM-Specific attributes to properties
    }
    #endregion

}
