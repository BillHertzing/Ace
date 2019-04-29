using System;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Text;
using Swordfish.NET.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Collections.Generic;
using ATAP.Utilities.ComputerInventory;
using ATAP.Utilities.ComputerHardware.Enumerations;
using ATAP.Utilities.DiskDrive;

using ATAP.Utilities.Database.Enumerations;
using System.Threading.Tasks;
using Funq;
using System.Threading;
using Ace.Agent.BaseServices;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.DiskDriveAnalysis;

namespace Ace.Agent.DiskAnalysisServices {


    public partial class DiskAnalysisServices : Service {

        public async Task<object> Post(AnalyzeDiskDriveRequest request) {
            Log.Debug("starting Post(AnalyzeDiskDriveRequest)");
            // Housekeeping setup for the task to be created
            // Create new Id for this LongRunningTask
            Id<LongRunningTaskInfo> longRunningTaskID = new Id<LongRunningTaskInfo>(Guid.NewGuid());
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationTokenSourceId = new Id<CancellationTokenSource>(Guid.NewGuid());
            var cancellationToken = cancellationTokenSource.Token;

            // Setup the instance 
            var diskAnalysis = new DiskDriveAnalysis(Log);
            var diskDriveSpecifier = request.AnalyzeDiskDriveRequestPayload.DiskDriveSpecifier;

            //ToDo: Create an association between the remote cancellation token and a new cancellation token, and record this in the RemoteToLocalCancellationToken dictionary in the Container

            // Setup the long-running task that will read one or more disk drives and CRUD a collection of DiskDriveInfoEx records, and optionally CRUD a DB Representation of that collection

            // Make a DB Representation of one or more disk drives and the partitions on those disk drives
            // Create multiple lambdas that will update SQLServerDB
            // The CRUD value comes in the request
            // There is a collection of DiskDriveInfoEx records in the ComputerInventory.ComputerHardware property of the BaseServicesData instance, which can be accessed through the DiskAnalysisServicesData.BaseServicesData property of the DiskAnalysisServicesData instance.     

            // Reading the actual Disk Drives connected to a named computer (localhost or a remote computer) can create a collection of DiskDriveInfoEx records

            // Reading a file formated to follow the conventions of the Configuration data for ComputerHardware can create a collection of DiskDriveInfoEx records
            // ToDo: figure out library Logging so that we don't need to pass a Log instance to the library

            //ToDo: Better switch on exactly what to Walk, - local machine, remote machine, configuration data, hypothetical data.
            // if the request's data is null, analyze all physical disks, else analyze the list of physical disks sent with the request

            // Get the BaseServicesData and diskAnalysisServicesData instances that were injected into the DI container
            var baseServicesData = HostContext.TryResolve<BaseServicesData>();
            var diskAnalysisServicesData =  HostContext.TryResolve<DiskAnalysisServicesData>();
 
            // Create storage for the results and progress
            var analyzeDiskDriveResult = new AnalyzeDiskDriveResult();
            diskAnalysisServicesData.AnalyzeDiskDriveResultsCOD.Add(longRunningTaskID, analyzeDiskDriveResult);
            var analyzeDiskDriveProgress = new AnalyzeDiskDriveProgress();
            diskAnalysisServicesData.AnalyzeDiskDriveProgressCOD.Add(longRunningTaskID, analyzeDiskDriveProgress);

            // Define the lambda that describes the task
            var task =new Task(() => {
                diskAnalysis.AnalyzeDiskDrive(
                    request.AnalyzeDiskDriveRequestPayload.DiskDriveSpecifier,
                    analyzeDiskDriveResult, analyzeDiskDriveProgress,
                    cancellationToken,
                    (crud, r) => {
                        Log.Debug($"starting recordRoot Lambda, r = {r}");
                    }
                ).ConfigureAwait(false);
            });

            LongRunningTaskInfo longRunningTaskInfo = new LongRunningTaskInfo(longRunningTaskID, task, cancellationTokenSource) ;

            // baseServicesData.CancellationTokenSources.Add(cancellationTokenSourceId, cancellationTokenSource);
            // Record this task (plus additional information about it) in the longRunningTasks dictionary in the BaseServicesData found in the Container
            baseServicesData.LongRunningTasks.Add(longRunningTaskID, longRunningTaskInfo);
            // record the TaskID and task info into the LookupDiskDriveAnalysisResultsCOD
            diskAnalysisServicesData.LookupDiskDriveAnalysisResultsCOD.Add(longRunningTaskID, longRunningTaskInfo);

            // ToDo: Setup the SSE receiver that will monitor the long-running task
            // ToDo: return to the caller the callback URL and the longRunningTaskID to allow the caller to connect to the SSE that monitors the task and the data structures it updates
            // Start the task running
            try {
                baseServicesData.LongRunningTasks[longRunningTaskID].LRTask.Start();

            }
            catch (Exception e) when (e is InvalidOperationException||e is ObjectDisposedException) {
                Log.Debug($"Exception when trying to start the AnalyzeDiskDrive task, message is {e.Message}");
                // ToDo: need to be sure that the when.any loop and GetLongRunningTaskStatus can handle a LongRunningTaskTaskInfo in these states;

            }
            var longRunningTaskIds = new List<Id<LongRunningTaskInfo>>();
            longRunningTaskIds.Add(longRunningTaskID);
            var analyzeDiskDriveResponsePayload = new AnalyzeDiskDriveResponsePayload(longRunningTaskIds);
            var analyzeDiskDriveResponse = new AnalyzeDiskDriveResponse(analyzeDiskDriveResponsePayload);
            Log.Debug($"Leaving Post(AnalyzeDiskDriveRequest), analyzeDiskDriveResponse = {analyzeDiskDriveResponse}");
            return analyzeDiskDriveResponse;
        }

    }


}
