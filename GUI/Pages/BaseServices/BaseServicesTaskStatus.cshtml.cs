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

namespace Ace.AceGUI.Pages {
    public partial class BaseServicesCodeBehind : ComponentBase {

        #region StringConstants
        // Eventually replace with localization
        #region StringConstants:TaskStatus
        public const string labelForUpdateLongRunningTasksStatusButton = "Press to Update Task Status";
        #endregion
        #endregion


        #region UpdateLongRunningTasksStatus
        public async Task UpdateLongRunningTasksStatus(int placeholder) {
            await new Task(() => { Thread.Sleep(5); });
        }
        public async Task UpdateLongRunningTasksStatus() {
            // Log.LogDebug($"Starting UpdateLongRunningTasksStatus, placeholder = {placeholder}");
            UpdateLongRunningTasksStatusRequest updateLongRunningTasksStatusRequest = new UpdateLongRunningTasksStatusRequest();
            //Log.LogDebug($"Calling PostJsonAsync<UpdateLongRunningTasksStatusResponse> with updateLongRunningTasksStatusRequest = {updateLongRunningTasksStatusRequest}");
            UpdateLongRunningTasksStatusResponse updateLongRunningTasksStatusResponse =
      await HttpClient.PostJsonAsync<UpdateLongRunningTasksStatusResponse>("/UpdateLongRunningTasksStatus", updateLongRunningTasksStatusRequest);
            //Log.LogDebug($"Returned from PostJsonAsync<UpdateLongRunningTasksStatusResponse> with updateLongRunningTasksStatusResponse = {updateLongRunningTasksStatusResponse}");

            // Log.LogDebug($"Leaving UpdateLongRunningTasksStatus");
        }

        #endregion

 

        #region Properties
        #region Properties:LongRunningTasks
        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo> LongRunningTasksCOD { get; set; }
        #endregion
        #endregion

    }
}
