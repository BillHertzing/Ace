using ServiceStack;
using System;
using ATAP.Utilities.LongRunningTasks;
using System.Collections.Generic;
using ATAP.Utilities.TypedGuids;

namespace Ace.Agent.BaseServices {


    #region GetLongRunningTasksStatusRequest, GetLongRunningTasksStatusResponse, and route GetLongRunningTasksStatusRequest
    [Route("/GetLongRunningTasksStatus")]
    public class GetLongRunningTasksStatusRequest : IReturn<GetLongRunningTasksStatusResponse> {
        public GetLongRunningTasksStatusRequest() : this(new List<Id<LongRunningTaskInfo>>()) { }
        public GetLongRunningTasksStatusRequest(List<Id<LongRunningTaskInfo>> longRunningTaskInfoIDs) {
            LongRunningTaskInfoIDs=longRunningTaskInfoIDs;
        }

        public List<Id<LongRunningTaskInfo>> LongRunningTaskInfoIDs { get; set; }
    }

    public class GetLongRunningTasksStatusResponse {
        public GetLongRunningTasksStatusResponse() : this(new LongRunningTaskStatuses()) { }
        public GetLongRunningTasksStatusResponse(LongRunningTaskStatuses longRunningTaskStatuses) {
            LongRunningTaskStatuses=longRunningTaskStatuses;
        }
        public LongRunningTaskStatuses LongRunningTaskStatuses { get; set; }
    }
    /*
    public class GetLongRunningTasksStatusResponsePayload {
        public GetLongRunningTasksStatusResponsePayload() : this(new LongRunningTaskStatuses()) { }

        public GetLongRunningTasksStatusResponsePayload(LongRunningTaskStatuses longRunningTaskStatuses) {
            LongRunningTaskStatuses=longRunningTaskStatuses??throw new ArgumentNullException(nameof(longRunningTaskStatuses));
        }

        public LongRunningTaskStatuses LongRunningTaskStatuses { get; set; }
    }
    */
    #endregion


    #region GetLongRunningTasksListRequest, GetLongRunningTasksListResponse and Route for GetLongRunningTasksList
    [Route("/GetLongRunningTasksList")]
    //[Route("/GetLongRunningTasksList/{LongRunningTaskID}")]
    public class GetLongRunningTasksListRequest : IReturn<GetLongRunningTasksListResponse> {
        //public GetLongRunningTasksListRequest() : this(Guid.Empty) { }
        public GetLongRunningTasksListRequest() {}
    }

    public class GetLongRunningTasksListResponse {
        public GetLongRunningTasksListResponse() : this(new List<Id<LongRunningTaskInfo>>()) { }
        public GetLongRunningTasksListResponse(List<Id<LongRunningTaskInfo>> longRunningTasksList) {
            LongRunningTasksList=longRunningTasksList;
        }
        public List<Id<LongRunningTaskInfo>> LongRunningTasksList { get; set; }
    }
    #endregion

}
