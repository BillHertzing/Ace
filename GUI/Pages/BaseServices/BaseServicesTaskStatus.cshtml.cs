// Required for the HttpClient
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ace.Agent.BaseServices;
// Required for the Log/logging
//using Blazor.Extensions.Logging;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
// Required for the Log/logging
using Microsoft.Extensions.Logging;
// Required for ComputerInventory used in BaseServices
using ATAP.Utilities.DiskDrive;
using Swordfish.NET.Collections;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.LongRunningTasks;
using System.Collections;
using System.Collections.Generic;

namespace Ace.AceGUI.Pages {
    public partial class BaseServicesCodeBehind : ComponentBase {

        #region StringConstants
        // Eventually replace with localization
        #region StringConstants:TaskStatus
        public const string labelForGetLongRunningTasksStatusButton = "Press to Get Tasks Status";
        #endregion
        #endregion


        #region GetLongRunningTasksStatus
        public async Task GetLongRunningTasksStatus(IEnumerable taskList) {
            await new Task(() => { Thread.Sleep(5); });
        }
        public async Task GetLongRunningTasksStatus() {
            // Log.LogDebug($"Starting GetLongRunningTasksStatus (all)");
            List<Id<LongRunningTaskInfo>> longRunningTaskInfoIds = new List<Id<LongRunningTaskInfo>>();
            longRunningTaskInfoIds.AddRange(LongRunningTasksCOD.Keys);
            GetLongRunningTasksStatusRequest getLongRunningTasksStatusRequest = new GetLongRunningTasksStatusRequest(longRunningTaskInfoIds);
            //Log.LogDebug($"Calling PostJsonAsync<GetLongRunningTasksStatusResponse> with getLongRunningTasksStatusRequest = {getLongRunningTasksStatusRequest}");
            GetLongRunningTasksStatusResponse getLongRunningTasksStatusResponse =
      await HttpClient.PostJsonAsync<GetLongRunningTasksStatusResponse>("/GetLongRunningTasksStatus", getLongRunningTasksStatusRequest);
            // Log.LogDebug($"Returned from PostJsonAsync<GetLongRunningTasksStatusResponse> with getLongRunningTasksStatusResponse = {getLongRunningTasksStatusResponse}");
            // Log.LogDebug($"Leaving GetLongRunningTasksStatus (all)");
        }

        #endregion


        public async Task GetLongRunningTasksState(List<Id<LongRunningTaskInfo>> longRunningTaskIds) {
            //Logger.LogDebug($"Starting GetLongRunningTasksState");
            //ToDo: add a cancellation token
            //Logger.LogDebug($"Calling PostJsonAsync<GetLongRunningTaskStateResponse>");
            // change the ReadDisk button's color

            GetLongRunningTasksStatusResponse=
          await HttpClient.PostJsonAsync<GetLongRunningTasksStatusResponse>("/GetLongRunningTasksStatus?format=json",
                                                                 new GetLongRunningTasksStatusRequest(longRunningTaskIds));
            //Logger.LogDebug($"Returned from PostJsonAsync<GetLongRunningTasksStatusResponse>");
            foreach (var s in GetLongRunningTasksStatusResponse.LongRunningTaskStatuses.LongRunningTaskStatusList) {
                LongRunningTasksCOD[s.Id]=s;
            }
            //Logger.LogDebug($"Leaving ReadDisk");
        }


        #region Properties
        #region Properties:LongRunningTasks
        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskStatus> LongRunningTasksCOD { get; set; }

        public GetLongRunningTasksStatusResponse GetLongRunningTasksStatusResponse { get; set; }
        #endregion
        #endregion

    }
}
