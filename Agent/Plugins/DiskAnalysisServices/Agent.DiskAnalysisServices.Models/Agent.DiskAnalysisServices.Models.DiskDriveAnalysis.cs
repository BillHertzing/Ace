using System;
using ServiceStack;
using ServiceStack.Text;
using ATAP.Utilities.ComputerInventory;
using System.Collections.Generic;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.DiskDrive;
using Ace.Agent.BaseServices;
using System.Threading;

namespace Ace.Agent.DiskAnalysisServices {

    #region AnalyzeDiskDriveRequest, AnalyzeDiskDriveResponse, and Route for AnalyzeDiskDrive
    [Route("/AnalyzeDiskDrive")]
    //[Route("/AnalyzeDiskDrive/{DiskDriveNumber}")]
    //[Route("/AnalyzeDiskDrive/{DiskDrivePartitionIdentifier}")]
    public class AnalyzeDiskDriveRequest : IReturn<AnalyzeDiskDriveResponse> {
        public AnalyzeDiskDriveRequest() : this(new AnalyzeDiskDriveRequestPayload()) { }
        public AnalyzeDiskDriveRequest(AnalyzeDiskDriveRequestPayload analyzeDiskDriveRequestPayload) {
            AnalyzeDiskDriveRequestPayload=analyzeDiskDriveRequestPayload;
        }

        public AnalyzeDiskDriveRequestPayload AnalyzeDiskDriveRequestPayload { get; set; }
    }
    public class AnalyzeDiskDriveRequestPayload {
        public AnalyzeDiskDriveRequestPayload() : this (new DiskDriveSpecifier()) {  }

        public AnalyzeDiskDriveRequestPayload(DiskDriveSpecifier diskDriveSpecifier) {
            DiskDriveSpecifier=diskDriveSpecifier??throw new ArgumentNullException(nameof(diskDriveSpecifier));
        }

        public DiskDriveSpecifier DiskDriveSpecifier { get; set; }
    }
    public class AnalyzeDiskDriveResponse {
        public AnalyzeDiskDriveResponse() : this(new AnalyzeDiskDriveResponsePayload()) {}
        public AnalyzeDiskDriveResponse(AnalyzeDiskDriveResponsePayload analyzeDiskDriveResponsePayload){
            AnalyzeDiskDriveResponsePayload = analyzeDiskDriveResponsePayload; }

        public AnalyzeDiskDriveResponsePayload AnalyzeDiskDriveResponsePayload { get; set; }
    }

    public class AnalyzeDiskDriveResponsePayload : IEquatable<AnalyzeDiskDriveResponsePayload> {
        public AnalyzeDiskDriveResponsePayload() : this (new List<Id<LongRunningTaskInfo>>()) {}

        public AnalyzeDiskDriveResponsePayload(List<Id<LongRunningTaskInfo>> longRunningTaskIds) {
            LongRunningTaskIds=longRunningTaskIds??throw new ArgumentNullException(nameof(longRunningTaskIds));
        }

        public List<Id<LongRunningTaskInfo>> LongRunningTaskIds { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as AnalyzeDiskDriveResponsePayload);
        }

        public bool Equals(AnalyzeDiskDriveResponsePayload other) {
            return other!=null&&
                   EqualityComparer<List<Id<LongRunningTaskInfo>>>.Default.Equals(LongRunningTaskIds, other.LongRunningTaskIds);
        }

        public override int GetHashCode() {
            return 1230356731+EqualityComparer<List<Id<LongRunningTaskInfo>>>.Default.GetHashCode(LongRunningTaskIds);
        }

        public static bool operator ==(AnalyzeDiskDriveResponsePayload left, AnalyzeDiskDriveResponsePayload right) {
            return EqualityComparer<AnalyzeDiskDriveResponsePayload>.Default.Equals(left, right);
        }

        public static bool operator !=(AnalyzeDiskDriveResponsePayload left, AnalyzeDiskDriveResponsePayload right) {
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



