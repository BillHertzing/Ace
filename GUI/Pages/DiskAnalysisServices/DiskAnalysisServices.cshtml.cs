// Required for the HttpClient
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ace.Agent.BaseServices;
using Ace.Agent.DiskAnalysisServices;
using ATAP.Utilities.DiskDrive;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
// Required for the logger/logging
//using Blazor.Extensions.Logging;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Ace.AceGUI.Pages
{
    public partial class DiskAnalysisServicesCodeBehind : ComponentBase
    {

        // This syntax adds to the class a Method that accesses the DI container, and retrieves the instance having the specified type from the DI container. In this case, we are accessing a builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type. This method call returns that object from the DI container  
        [Inject]
        HttpClient HttpClient { get; set; }

        // Access the Logging extensions registered in the DI container
        // [Inject]
        // public ILogger<DiskAnalysisServicesCodeBehind> Logger { get; set; }

        public async Task AnalyzeDiskDrive(string computerName, int? diskDriveNumber)
        {
            //Logger.LogDebug($"Starting AnalyzeDiskDrive");
            var analyzeDiskDriveRequestPayload = new AnalyzeDiskDriveRequestPayload(new DiskDriveSpecifier() { ComputerName = computerName, DiskDriveNumber = diskDriveNumber, DiskDrivePartitionIdentifier = new DiskDrivePartitionIdentifier() });
            var analyzeDiskDriveRequest = new AnalyzeDiskDriveRequest(analyzeDiskDriveRequestPayload);
            //Logger.LogDebug($"Calling PostJsonAsync<ReadDiskResponse>");
            // ToDo: deactivate the AnalyzeDiskDrive button
            var analyzeDiskDriveResponse =
          await HttpClient.PostJsonAsync<AnalyzeDiskDriveResponse>("/AnalyzeDiskDrive?format=json",
                                                                 analyzeDiskDriveRequest);
            //Logger.LogDebug($"Returned from PostJsonAsync<AnalyzeDiskDriveResponse>");
            // record the TaskID, creating the List if it does not yet exist
            if (AnalyzeDiskDriveLongRunningTaskIds == null) { AnalyzeDiskDriveLongRunningTaskIds=new List<Id<LongRunningTaskInfo>>(); }
            AnalyzeDiskDriveLongRunningTaskIds.AddRange(analyzeDiskDriveResponse.AnalyzeDiskDriveResponsePayload.LongRunningTaskIds);
            // This should be a URL and and ID for connecting to a SSE, and the next step
            // is to draw a base result, then hookup a local task that monitors the SSE and updates the local copy of the COD
            // ToDo: Activate the AnalyzeDiskDrive button
            //Logger.LogDebug($"Leaving AnalyzeDiskDrive");
        }

        public async Task AnalyzeFileSystem(string drive, int asyncFileReadBlockSize) {
            //Logger.LogDebug($"Starting AnalyzeFileSystem");
            var analyzeFileSystemRequestPayload = new AnalyzeFileSystemRequestPayload(drive, asyncFileReadBlockSize);
            var analyzeFileSystemRequest = new AnalyzeFileSystemRequest(analyzeFileSystemRequestPayload);
            //Logger.LogDebug($"Calling PostJsonAsync<ReadDiskResponse>");
            // ToDo: deactivate the AnalyzeFileSystem button
            var analyzeFileSystemResponse =
          await HttpClient.PostJsonAsync<AnalyzeFileSystemResponse>("/AnalyzeFileSystem?format=json",
                                                                 analyzeFileSystemRequest);
            //Logger.LogDebug($"Returned from PostJsonAsync<AnalyzeFileSystemResponse>");
            // record the TaskID
            if (AnalyzeFileSystemLongRunningTaskIds==null) { AnalyzeFileSystemLongRunningTaskIds=new List<Id<LongRunningTaskInfo>>(); }

            AnalyzeFileSystemLongRunningTaskIds.AddRange(analyzeFileSystemResponse.AnalyzeFileSystemResponsePayload.LongRunningTaskIds);
            // This should be a URL and and ID for connecting to a SSE, and the next step
            // is to draw a base result, then hookup a local task that monitors the SSE and updates the local copy of the COD
            // ToDo: Activate the AnalyzeFileSystem button
            //Logger.LogDebug($"Leaving AnalyzeFileSystem");
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
