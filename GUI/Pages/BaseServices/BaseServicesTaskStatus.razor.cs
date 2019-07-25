// Required for the HttpClient
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ace.Agent.BaseServices;
// Required for Blazor
using Microsoft.AspNetCore.Components;
// Required for Browser Console Logging
using Microsoft.Extensions.Logging;
using Blazor.Extensions.Logging;
// Required for Blazor LocalStorage
// Required for ComputerInventory used in BaseServices
using Swordfish.NET.Collections;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.LongRunningTasks;
using System.Collections.Generic;
using ServiceStack.Text;
using Ace.AceGUI.HttpClientExtenssions;

namespace Ace.AceGUI.Pages {
    public partial class BaseServicesCodeBehind : ComponentBase {

        #region StringConstants
        // Eventually replace with localization
        #region StringConstants:TaskStatus
        public const string labelForGetLongRunningTasksStatusButton = "Press to Get all Long Running Tasks Status";
        public const string labelForGetLongRunningTasksStatusByListButton = "Press to Get Long Running Tasks Status ByList";
        #endregion
        #endregion

        #region LongRunningTasksCOD Initialization Handler
        protected async Task InitLongRunningTasksStatusAsync() {
            Log.LogDebug($"Starting BaseServices.InitLongRunningTasksStatusAsync");
            // ToDo: analyze code paths to be sure there is no way this can be called before local storage is initialized
            // IsInitialized=await LStorage.GetItemAsync<bool>("BaseServices.IsInitialized");
            // ToDo: test and throw an error if local storage is not yet initialized
            // if (!IsInitialized) {}

            // initialize BaseServices.LongRunningTasksCOD property with data from local Storage

            LongRunningTasksCOD=await LStorage.GetItemAsync<ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskStatus>>("BaseServices.LongRunningTasksCOD");
            Log.LogDebug($"Leaving BaseServices.InitLongRunningTasksStatusAsync");
        }
        #endregion

        #region GetLongRunningTasksStatus
        public async Task GetLongRunningTasksStatusByList() {
            Log.LogDebug($"Starting BaseServices.GetLongRunningTasksStatusByList");
            var longRunningTaskInfoIdList = new List<Id<LongRunningTaskInfo>>();
            Log.LogDebug($"in BaseServices.GetLongRunningTasksStatusByList longRunningTaskInfoIdList = {longRunningTaskInfoIdList.Dump()}");
            longRunningTaskInfoIdList.Add(new Id<LongRunningTaskInfo>());
            Log.LogDebug($"in BaseServices.GetLongRunningTasksStatusByList longRunningTaskInfoIdList = {longRunningTaskInfoIdList.Dump()}");
            var getLongRunningTasksStatusRequest = new GetLongRunningTasksStatusRequest(longRunningTaskInfoIdList);
            /*
            var getLongRunningTasksStatusRequestSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(getLongRunningTasksStatusRequest);
            Log.LogDebug($"in BaseServices.GetLongRunningTasksStatusByList getLongRunningTasksStatusRequestSerialized = {getLongRunningTasksStatusRequestSerialized.Dump()}");
            var stringContent = new StringContent(getLongRunningTasksStatusRequestSerialized, Encoding.UTF8, "application/json");
            Log.LogDebug($"in BaseServices.GetLongRunningTasksStatusByList stringContent = {stringContent.Dump()}");
            Log.LogDebug($"Calling  HttpClient.SendAsync with content = stringContent");
            UriBuilder.Path="GetLongRunningTasksStatus";
            var getLongRunningTasksStatusResponseMessage = await HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, UriBuilder.Uri.ToString()) {
                Content=stringContent
            });
            Log.LogDebug($"in BaseServices.GetLongRunningTasksStatusByList getLongRunningTasksStatusResponseMessage = {getLongRunningTasksStatusResponseMessage.Dump()}");
            string getLongRunningTasksStatusResponseStr = await getLongRunningTasksStatusResponseMessage.Content.ReadAsStringAsync();
            Log.LogDebug($"in BaseServices.GetLongRunningTasksStatusByList getLongRunningTasksStatusResponseStr = {getLongRunningTasksStatusResponseStr.Dump()}");
            var getLongRunningTasksStatusResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<GetLongRunningTasksStatusResponse>(getLongRunningTasksStatusResponseStr);
            */
            Log.LogDebug($"Calling PostJsonAsyncSS<GetLongRunningTasksStatusResponse> with getLongRunningTasksStatusRequest = {getLongRunningTasksStatusRequest.Dump()}");
            var getLongRunningTasksStatusResponse =
            await HttpClient.PostJsonAsyncSS<GetLongRunningTasksStatusResponse>(UriBuilder.Uri.ToString(), getLongRunningTasksStatusRequest);
            Log.LogDebug($"Returned from PostJsonAsyncSS<GetLongRunningTasksStatusResponse> with getLongRunningTasksStatusResponse = {getLongRunningTasksStatusResponse.Dump()}");

            Log.LogDebug($"Leaving BaseServices.GetLongRunningTasksStatusByList");
        }
        public async Task GetLongRunningTasksStatus() {
            Log.LogDebug($"Starting BaseServices.GetLongRunningTasksStatus (all)");
            List<Id<LongRunningTaskInfo>> longRunningTaskInfoIdList = new List<Id<LongRunningTaskInfo>>();
            Log.LogDebug($"in BaseServices.GetLongRunningTasksStatus (all): longRunningTaskInfoIdList = {longRunningTaskInfoIdList.Dump()}");
            Log.LogDebug($"in BaseServices.GetLongRunningTasksStatus (all): LongRunningTasksCOD = {LongRunningTasksCOD.Dump()}");
            longRunningTaskInfoIdList.AddRange(LongRunningTasksCOD.Keys);
            GetLongRunningTasksStatusRequest getLongRunningTasksStatusRequest = new GetLongRunningTasksStatusRequest(longRunningTaskInfoIdList);
            UriBuilder.Path="GetLongRunningTasksStatus";
            Log.LogDebug($"Calling PostJsonAsyncSS<GetLongRunningTasksStatusResponse> with getLongRunningTasksStatusRequest = {getLongRunningTasksStatusRequest.Dump()}");
            GetLongRunningTasksStatusResponse getLongRunningTasksStatusResponse =
      await HttpClient.PostJsonAsyncSS<GetLongRunningTasksStatusResponse>(UriBuilder.Uri.ToString(), getLongRunningTasksStatusRequest);
            Log.LogDebug($"Returned from PostJsonAsyncSS<GetLongRunningTasksStatusResponse> with getLongRunningTasksStatusResponse = {getLongRunningTasksStatusResponse.Dump()}");
            Log.LogDebug($"Leaving BaseServices.GetLongRunningTasksStatus (all)");
        }

        #endregion

        #region Properties
        #region Properties:LongRunningTasks
        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskStatus> LongRunningTasksCOD { get; set; }

        public GetLongRunningTasksStatusResponse GetLongRunningTasksStatusResponse { get; set; }
        #endregion
        #endregion

    }
}
