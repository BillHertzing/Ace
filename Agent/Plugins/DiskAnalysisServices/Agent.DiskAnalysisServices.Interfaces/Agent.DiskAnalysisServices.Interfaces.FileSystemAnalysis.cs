using System;
using ServiceStack;
using ServiceStack.Text;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Collections.Generic;
using ATAP.Utilities.ComputerInventory;
using ATAP.Utilities.ComputerHardware.Enumerations;
using ATAP.Utilities.FileSystem;
using ATAP.Utilities.FileSystem.Enumerations;
using ATAP.Utilities.Database.Enumerations;
using System.Threading.Tasks;
using Funq;
using System.Threading;
using Ace.Agent.BaseServices;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.DiskDriveAnalysis;
using ATAP.Utilities.DiskDrive;
using System.Text;
using System.Net.Http;
using Serilog;

namespace Ace.Plugin.DiskAnalysisServices {


    public partial class DiskAnalysisServices : Service {

        public async Task<object> Post(AnalyzeFileSystemRequest request) {
            // Housekeeping setup for the task to be created
            // Create new Id for this LongRunningTask
            Id<LongRunningTaskInfo> longRunningTaskID = new Id<LongRunningTaskInfo>(Guid.NewGuid());
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationTokenSourceId = new Id<CancellationTokenSource>(Guid.NewGuid());
            var cancellationToken = cancellationTokenSource.Token;
            Log.Debug("in Post(AnalyzeFileSystemRequest) 1");
            // Get the BaseServicesData and diskAnalysisServicesData instances that were injected into the DI container
            var baseServicesData = HostContext.TryResolve<BaseServicesData>();
            var diskAnalysisServicesData = HostContext.TryResolve<DiskAnalysisServicesData>();

            // Setup the instance. Use Configuration Data if the request payload is null
            var blockSize = request.AnalyzeFileSystemRequestPayload.AsyncFileReadBlockSize>=0? request.AnalyzeFileSystemRequestPayload.AsyncFileReadBlockSize : diskAnalysisServicesData.ConfigurationData.BlockSize;
            var fileSystemAnalysis = new FileSystemAnalysis(Log.Logger, diskAnalysisServicesData.ConfigurationData.BlockSize);
            Log.Debug("in Post(AnalyzeFileSystemRequest) 2");


            // Create storage for the results and progress
            var analyzeFileSystemResult =new AnalyzeFileSystemResult();
            diskAnalysisServicesData.AnalyzeFileSystemResultsCOD.Add(longRunningTaskID, analyzeFileSystemResult);
            var analyzeFileSystemProgress = new AnalyzeFileSystemProgress();
            diskAnalysisServicesData.AnalyzeFileSystemProgressCOD.Add(longRunningTaskID, analyzeFileSystemProgress);

            // Define the lambda that describes the task
            var task = new Task(() => {
                fileSystemAnalysis.AnalyzeFileSystem(
                    request.AnalyzeFileSystemRequestPayload.Root,
                    analyzeFileSystemResult, analyzeFileSystemProgress,
                    cancellationToken,
                    (crud, r) => {
                        Log.Debug($"starting recordRoot Lambda, r = {r}");
                    }
                ).ConfigureAwait(false);
            });

            LongRunningTaskInfo longRunningTaskInfo = new LongRunningTaskInfo(longRunningTaskID, task, cancellationTokenSource) ;
            // Record this task (plus additional information about it) in the longRunningTasks dictionary in the BaseServicesData found in the Container
            baseServicesData.LongRunningTasks.Add(longRunningTaskID, longRunningTaskInfo);
            // record the TaskID and task info into the LookupDiskDriveAnalysisResultsCOD
            diskAnalysisServicesData.LookupFileSystemAnalysisResultsCOD.Add(longRunningTaskID, longRunningTaskInfo);

            // ToDo: Setup the SSE receiver that will monitor the long-running task
            // ToDo: return to the caller the callback URL and the longRunningTaskID to allow the caller to connect to the SSE that monitors the task and the data structures it updates
            // Start the task running
            try {
                baseServicesData.LongRunningTasks[longRunningTaskID].LRTask.Start();
            }
            catch (Exception e) when (e is InvalidOperationException||e is ObjectDisposedException) {
                Log.Debug($"Exception when trying to start the AnalyzeDiskDrive task, message is {e.Message}");
                // ToDo: need to be sure that the when.any loop and GetLongRunningTasksStatus can handle a taskinfo in these states;

            }
            var analyzeFileSystemResponsePayload = new AnalyzeFileSystemResponsePayload(new List<Id<LongRunningTaskInfo>>() { longRunningTaskID });
            var analyzeFileSystemResponse = new AnalyzeFileSystemResponse(analyzeFileSystemResponsePayload);
            Log.Debug($"in AnalyzeFileSystemRequest analyzeFileSystemResponse = {analyzeFileSystemResponse.Dump()}");
            // testing
            //var analyzeFileSystemResponseSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(analyzeFileSystemResponse);
            //Log.Debug($"in AnalyzeFileSystemRequest analyzeFileSystemResponseSerialized = {analyzeFileSystemResponseSerialized.Dump()}");
            //var stringContent = new StringContent(analyzeFileSystemResponseSerialized, Encoding.UTF8, "application/json");
            //Log.Debug($"in AnalyzeFileSystemRequest stringContent = {stringContent.Dump()}");
            // end testing 
            await Task.Yield(); // ToDo: figure out if this is the right way to make the method async.
            Log.Debug($"Leaving Post(AnalyzeFileSystemRequest), analyzeFileSystemResponse = {analyzeFileSystemResponse}");
            return analyzeFileSystemResponse;
        }

    }


}
