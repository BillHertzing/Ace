using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Ace.Plugin.DiskAnalysisServices;
using ATAP.Utilities.DiskDrive;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ServiceStack.Text;
using Ace.AceGUI.HttpClientExtenssions;

namespace Ace.AceGUI.Pages {
    public partial class DiskAnalysisServicesCodeBehind : ComponentBase
    {
        #region Injected instances
        // Access the pre-configured and extended HTTPClient object in the DI container
        [Inject]
        HttpClient HttpClient { get; set; }

        // Access the pre-configured Logging extensions registered in the DI container
        [Inject]
        ILogger<DiskAnalysisServicesCodeBehind> Logger { get; set; }

        //// Access the IUriHelper registered in the DI container
        //[Inject]
        //IUriHelper UriHelper { get; set; }
        // Access the NavigationManager registered in the DI container
        [Inject]
        NavigationManager NavigationManager { get; set; }
        #endregion

        public static string HardcodedHost { get; set; } = "localhost";
        public async Task AnalyzeDiskDrive(string computerName, int? diskDriveNumber)
        {
            Logger.LogDebug($"Starting AnalyzeDiskDrive");
            var analyzeDiskDriveRequestPayload = new AnalyzeDiskDriveRequestPayload(new DiskDriveSpecifier() { ComputerName = computerName, DiskDriveNumber = diskDriveNumber});
            var analyzeDiskDriveRequest = new AnalyzeDiskDriveRequest(analyzeDiskDriveRequestPayload);
            // ToDo: deactivate the AnalyzeDiskDrive button
            var analyzeDiskDriveResponse = await HttpClient.PostJsonAsyncSS<AnalyzeDiskDriveResponse>(NavigationManager.ToAbsoluteUri("AnalyzeDiskDrive").ToString(), analyzeDiskDriveRequest);
            Logger.LogDebug($"Returned from PostJsonAsyncSS<AnalyzeDiskDriveResponse>, analyzeDiskDriveResponse = {analyzeDiskDriveResponse.Dump()}");
            // ToDo: move initialization of the AnalyzeDiskDriveLongRunningTaskIds object to a method that initializes a Lazy and see if it needs a Dispose??
            // record the TaskID, creating the List if it does not yet exist
            if (AnalyzeDiskDriveLongRunningTaskIds == null) { AnalyzeDiskDriveLongRunningTaskIds=new List<Id<LongRunningTaskInfo>>(); }
            AnalyzeDiskDriveLongRunningTaskIds.AddRange(analyzeDiskDriveResponse.AnalyzeDiskDriveResponsePayload.LongRunningTaskIds);
            // This should be a URL and and ID for connecting to a SSE, and the next step
            // is to draw a base result, then hookup a local task that monitors the SSE and updates the local copy of the COD
            // ToDo: Activate the AnalyzeDiskDrive button
            //Logger.LogDebug($"Leaving AnalyzeDiskDrive");
        }

        public static string HardcodedDrive { get; set; } = "E:\\";
        public async Task AnalyzeFileSystem(string drive, int asyncFileReadBlockSize) {
            Logger.LogDebug($"Starting AnalyzeFileSystem");
            
            var analyzeFileSystemRequestPayload = new AnalyzeFileSystemRequestPayload(drive, asyncFileReadBlockSize);
            var analyzeFileSystemRequest = new AnalyzeFileSystemRequest(analyzeFileSystemRequestPayload);
            // ToDo: deactivate the AnalyzeFileSystem button
            Logger.LogDebug($"in AnalyzeFileSystem: Calling PostJsonAsyncSS<AnalyzeFileSystemResponse> with analyzeFileSystemRequest = {analyzeFileSystemRequest.Dump()}");
            //ToDo: every call back to the host may potentially return status code 500 and an error message, must wrap the call in try/catch
            var analyzeFileSystemResponse =
            await HttpClient.PostJsonAsyncSS<AnalyzeFileSystemResponse>(NavigationManager.ToAbsoluteUri("AnalyzeFileSystem").ToString(),
                                                                   analyzeFileSystemRequest);
            Logger.LogDebug($"in AnalyzeFileSystem: PostJsonAsyncSS<AnalyzeFileSystemResponse> returned analyzeFileSystemResponse = {analyzeFileSystemResponse.Dump()}");

            // ToDo: move initialization of the AnalyzeFileSystemLongRunningTaskIds object to a method that initializes a Lazy and see if it needs a Dispose??
            if (AnalyzeFileSystemLongRunningTaskIds==null) { AnalyzeFileSystemLongRunningTaskIds=new List<Id<LongRunningTaskInfo>>(); }

            // record the TaskID
            AnalyzeFileSystemLongRunningTaskIds.AddRange(analyzeFileSystemResponse.AnalyzeFileSystemResponsePayload.LongRunningTaskIds);
            // This should be a URL and and ID for connecting to a SSE, and the next step
            // is to draw a base result, then hookup a local task that monitors the SSE and updates the local copy of the COD
            // ToDo: Activate the AnalyzeFileSystem button
            Logger.LogDebug($"Leaving AnalyzeFileSystem");
        }
        #region Properties:AnalyzeDiskDrive
        public AnalyzeDiskDriveResponse AnalyzeDiskDriveResponse { get; set; }
        public List<Id<LongRunningTaskInfo>> AnalyzeDiskDriveLongRunningTaskIds { get; set; }
        #endregion

        #region Properties:AnalyzeFileSystem
        public AnalyzeFileSystemResponse AnalyzeFileSystemResponse { get; set; }
        public List<Id<LongRunningTaskInfo>> AnalyzeFileSystemLongRunningTaskIds { get; set; }
        #endregion

        #region Properties:GetAnalyzeDiskDriveLongRunningTasksList
        //public GetLongRunningTasksListResponse GetAnalyzeDiskDriveLongRunningTasksListResponse { get; set; }

        #endregion
    }
}
