using ServiceStack;
using System;
using ATAP.Utilities.LongRunningTasks;
using System.Collections.Generic;
using ATAP.Utilities.TypedGuids;

namespace Ace.Agent.BaseServices {
    

    #region UpdateLongRunningTasksStatusRequest, UpdateLongRunningTasksStatusResponse, and route UpdateLongRunningTasksStatus
    [Route("/UpdateLongRunningTasksStatus")]
    public class UpdateLongRunningTasksStatusRequest : IReturn<UpdateLongRunningTasksStatusResponse> {
        public UpdateLongRunningTasksStatusRequest() { }
    }

    public class UpdateLongRunningTasksStatusResponse {
        public UpdateLongRunningTasksStatusResponse() : this(new UpdateLongRunningTasksStatusResponsePayload()) { }
        public UpdateLongRunningTasksStatusResponse(UpdateLongRunningTasksStatusResponsePayload updateLongRunningTasksStatusResponsePayload) {
            UpdateLongRunningTasksStatusResponsePayload=updateLongRunningTasksStatusResponsePayload;
        }
        public UpdateLongRunningTasksStatusResponsePayload UpdateLongRunningTasksStatusResponsePayload { get; set; }
    }

    public class UpdateLongRunningTasksStatusResponsePayload {
        public UpdateLongRunningTasksStatusResponsePayload() : this(new LongRunningTaskStatuses()) { }

        public UpdateLongRunningTasksStatusResponsePayload(LongRunningTaskStatuses longRunningTaskStatuses) {
            LongRunningTaskStatuses=longRunningTaskStatuses??throw new ArgumentNullException(nameof(longRunningTaskStatuses));
        }

        public LongRunningTaskStatuses LongRunningTaskStatuses { get; set; }
    }
    #endregion
	
	
	 #region GetLongRunningTaskStateRequest, GetLongRunningTaskStateResponse and Route for GetLongRunningTaskState
    [Route("/GetLongRunningTaskState")]
    [Route("/GetLongRunningTaskState/{LongRunningTaskID}")]
    public class GetLongRunningTaskStateRequest : IReturn<GetLongRunningTaskStateResponse> {
        //public GetLongRunningTaskStateRequest() : this(Guid.Empty) { }
        public GetLongRunningTaskStateRequest(List<Id<LongRunningTaskInfo>> longRunningTaskIDs) {
            LongRunningTaskIDs=longRunningTaskIDs;
        }
        public List<Id<LongRunningTaskInfo>> LongRunningTaskIDs { get; set; }
    }

    public class GetLongRunningTaskStateResponse {
        public GetLongRunningTaskStateResponse() : this("DefaultTaskState") { }
        public GetLongRunningTaskStateResponse(string longRunningTaskState) {
            LongRunningTaskState=longRunningTaskState;
        }
        public string LongRunningTaskState { get; set; }
    }
    #endregion

}
