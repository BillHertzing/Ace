using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Logging;
using ATAP.Utilities.LongRunningTasks;

namespace Ace.Agent.BaseServices {
    public partial class BaseServices : Service {

        #region Interfaces:UpdateLongRunningTasksStatus
        public object Post(UpdateLongRunningTasksStatusRequest request) {
            Log.Debug("starting Post(UpdateLongRunningTasksStatusRequest request)");
            var baseServicesData = HostContext.TryResolve<BaseServicesData>();

            var updateLongRunningTasksStatusResponsePayload = new UpdateLongRunningTasksStatusResponsePayload();
            foreach (var LRTid in BaseServicesData.LongRunningTasks.Keys) {
                var taskstatus = BaseServicesData.LongRunningTasks[LRTid].LRTask.Status;
                // ToDo: Add start and duration
                updateLongRunningTasksStatusResponsePayload.LongRunningTaskStatuses.LongRunningTaskStatusList.Add(new LongRunningTaskStatus(LRTid, taskstatus));
        }
            var updateLongRunningTasksStatusResponse = new UpdateLongRunningTasksStatusResponse(updateLongRunningTasksStatusResponsePayload);
            Log.Debug("leaving Post(UpdateLongRunningTasksStatusRequest request)");
            return updateLongRunningTasksStatusResponse;
        }
        #endregion

    }
}
