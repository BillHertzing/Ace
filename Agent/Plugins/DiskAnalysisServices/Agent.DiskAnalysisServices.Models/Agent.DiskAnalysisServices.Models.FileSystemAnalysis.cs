using System;
using ServiceStack;
using ServiceStack.Text;
using ATAP.Utilities.ComputerInventory;
using System.Collections.Generic;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.FileSystem;
using Ace.Agent.BaseServices;
using Newtonsoft.Json;
using System.IO;

namespace Ace.Plugin.DiskAnalysisServices {

    #region AnalyzeFileSystemRequest, AnalyzeFileSystemResponse, and Route for AnalyzeFileSystem
    [Route("/AnalyzeFileSystem")]
    //[Route("/AnalyzeFileSystem/{root}")]
    public class AnalyzeFileSystemRequest : IReturn<AnalyzeFileSystemResponse> {
        public AnalyzeFileSystemRequest() : this(new AnalyzeFileSystemRequestPayload()) { }
        public AnalyzeFileSystemRequest(AnalyzeFileSystemRequestPayload analyzeFileSystemRequestPayload) {
            AnalyzeFileSystemRequestPayload=analyzeFileSystemRequestPayload;
        }

        public AnalyzeFileSystemRequestPayload AnalyzeFileSystemRequestPayload { get; set; }
    }
    public class AnalyzeFileSystemRequestPayload : IEquatable<AnalyzeFileSystemRequestPayload> {
        public AnalyzeFileSystemRequestPayload() :this (string.Empty, 4096) {}

        public AnalyzeFileSystemRequestPayload(string root, int asyncFileReadBlockSize) {
            Root=root??throw new ArgumentNullException(nameof(root));
            AsyncFileReadBlockSize=asyncFileReadBlockSize;
        }

        public string Root { get; set; }
        public int AsyncFileReadBlockSize { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as AnalyzeFileSystemRequestPayload);
        }

        public bool Equals(AnalyzeFileSystemRequestPayload other) {
            return other!=null&&
                   Root==other.Root&&
                   AsyncFileReadBlockSize==other.AsyncFileReadBlockSize;
        }

        public override int GetHashCode() {
            var hashCode = 715771032;
            hashCode=hashCode*-1521134295+EqualityComparer<string>.Default.GetHashCode(Root);
            hashCode=hashCode*-1521134295+AsyncFileReadBlockSize.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(AnalyzeFileSystemRequestPayload left, AnalyzeFileSystemRequestPayload right) {
            return EqualityComparer<AnalyzeFileSystemRequestPayload>.Default.Equals(left, right);
        }

        public static bool operator !=(AnalyzeFileSystemRequestPayload left, AnalyzeFileSystemRequestPayload right) {
            return !(left==right);
        }
    }
    public class AnalyzeFileSystemResponse {
        public AnalyzeFileSystemResponse() : this(new AnalyzeFileSystemResponsePayload()) { }
        public AnalyzeFileSystemResponse(AnalyzeFileSystemResponsePayload analyzeFileSystemResponsePayload) {
            AnalyzeFileSystemResponsePayload=analyzeFileSystemResponsePayload;
        }

        public AnalyzeFileSystemResponsePayload AnalyzeFileSystemResponsePayload { get; set; }
    }

    public class AnalyzeFileSystemResponsePayload : IEquatable<AnalyzeFileSystemResponsePayload> {
        public AnalyzeFileSystemResponsePayload() :this (new List<Id<LongRunningTaskInfo>>() ) {}

        public AnalyzeFileSystemResponsePayload(List<Id<LongRunningTaskInfo>> longRunningTaskIds) {
            LongRunningTaskIds=longRunningTaskIds??throw new ArgumentNullException(nameof(longRunningTaskIds));
        }
        [JsonConverter(typeof(Id<LongRunningTaskInfo>))]
        public List<Id<LongRunningTaskInfo>> LongRunningTaskIds { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as AnalyzeFileSystemResponsePayload);
        }

        public bool Equals(AnalyzeFileSystemResponsePayload other) {
            return other!=null&&
                   EqualityComparer<List<Id<LongRunningTaskInfo>>>.Default.Equals(LongRunningTaskIds, other.LongRunningTaskIds);
        }

        public override int GetHashCode() {
            return 1230356731+EqualityComparer<List<Id<LongRunningTaskInfo>>>.Default.GetHashCode(LongRunningTaskIds);
        }

        public static bool operator ==(AnalyzeFileSystemResponsePayload left, AnalyzeFileSystemResponsePayload right) {
            return EqualityComparer<AnalyzeFileSystemResponsePayload>.Default.Equals(left, right);
        }

        public static bool operator !=(AnalyzeFileSystemResponsePayload left, AnalyzeFileSystemResponsePayload right) {
            return !(left==right);
        }
    }
    #endregion

    /*
    #region Monitor Data Structures
    [Route("/MonitorDiskAnalysisServicesDataStructures")]
      public class MonitorDiskAnalysisServicesDataStructuresRequest : IReturn<MonitorDiskAnalysisServicesDataStructuresResponse>
    {
      public string Filters { get; set; }
    }
    public class MonitorDiskAnalysisServicesDataStructuresResponse
    {
      public string[] Result { get; set; }
      public Operation Kind { get; set; }
    }
    #endregion
    */

}



