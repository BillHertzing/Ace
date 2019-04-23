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

    #region DiskDrivesToDBGraphRequest, DiskDrivesToDBGraphResponse, and Route for DiskDrivesToDBGraph
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
        public AnalyzeDiskDriveRequestPayload() {
            DiskDriveSpecifier=new DiskDriveSpecifier();
            CancellationToken=new CancellationToken(true);
        }

        public AnalyzeDiskDriveRequestPayload(DiskDriveSpecifier diskDriveSpecifier, CancellationToken cancellationToken) {
            DiskDriveSpecifier=diskDriveSpecifier??throw new ArgumentNullException(nameof(diskDriveSpecifier));
            CancellationToken=cancellationToken;
        }

        public DiskDriveSpecifier DiskDriveSpecifier { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
    public class AnalyzeDiskDriveResponse {
        public AnalyzeDiskDriveResponse() : this(new List<Id<LongRunningTaskInfo>>()) { }
        public AnalyzeDiskDriveResponse(List<Id<LongRunningTaskInfo>> longRunningTaskIDs) { LongRunningTaskIDs=longRunningTaskIDs; }
        public List<Id<LongRunningTaskInfo>> LongRunningTaskIDs { get; set; }
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



