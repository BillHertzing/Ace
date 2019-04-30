using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Logging;
using ATAP.Utilities.LongRunningTasks;

namespace Ace.Agent.BaseServices {
    public partial class BaseServices : Service {

        #region Interfaces:GetLongRunningTasksStatus
        public object Post(GetLongRunningTasksStatusRequest request) {
            Log.Debug("starting Post(GetLongRunningTasksStatusRequest request)");
            var baseServicesData = HostContext.TryResolve<BaseServicesData>();
            var getLongRunningTasksStatusResponse = new GetLongRunningTasksStatusResponse();
            foreach (var LRTid in BaseServicesData.LongRunningTasks.Keys) {
                var taskstatus = BaseServicesData.LongRunningTasks[LRTid].LRTask.Status;
                var ag = BaseServicesData.LongRunningTasks[LRTid].LRTask.Exception;
                var numInnerExceptions = (ag== null) ? 0 : ag.InnerExceptions.Count;
                // ToDo: Add start time and current duration, Interim result, if any, must be queried through specialized interfaces referencing back to this master list
                getLongRunningTasksStatusResponse.LongRunningTaskStatuses.LongRunningTaskStatusList.Add(new LongRunningTaskStatus(LRTid, taskstatus, numInnerExceptions));
            }
            Log.Debug("leaving Post(GetLongRunningTasksStatusRequest request)");
            return getLongRunningTasksStatusResponse;
        }
        #endregion

    }
}
