using System;
using ServiceStack;
using ServiceStack.Text;
using ATAP.Utilities.ComputerInventory;
using System.Collections.Generic;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.DiskDrive;
using Ace.Agent.BaseServices;

namespace Ace.Agent.DiskAnalysisServices {

    #region DiskDrivesToDBGraphRequest, DiskDrivesToDBGraphResponse, and Route for DiskDrivesToDBGraph
    [Route("/WalkDiskDrive")]
    //[Route("/WalkDiskDrive/{DiskDriveNumber}")]
    //[Route("/WalkDiskDrive/{DiskDrivePartitionIdentifier}")]
    public class WalkDiskDriveRequest : IReturn<WalkDiskDriveResponse> {
        public WalkDiskDriveRequest(int? diskNumber, DiskDrivePartitionIdentifier diskDrivePartitionIdentifier) {
            DiskDriveNumber=diskNumber;
            DiskDrivePartitionIdentifier=diskDrivePartitionIdentifier;
        }
        public WalkDiskDriveRequest(DiskDrivePartitionIdentifier diskDrivePartitionIdentifier) { }
        public int? DiskDriveNumber { get; set; }
        public DiskDrivePartitionIdentifier DiskDrivePartitionIdentifier { get; set; }
    }
    public class WalkDiskDriveResponse {
        public WalkDiskDriveResponse() : this(new List<Id<LongRunningTaskInfo>>()) { }
        public WalkDiskDriveResponse(List<Id<LongRunningTaskInfo>> longRunningTaskIDs) { LongRunningTaskIDs=longRunningTaskIDs; }
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



