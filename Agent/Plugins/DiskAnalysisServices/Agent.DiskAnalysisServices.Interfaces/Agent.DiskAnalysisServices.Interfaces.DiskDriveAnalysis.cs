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
using ATAP.Utilities.FileSystem;
using ATAP.Utilities.FileSystem.Enumerations;
using ATAP.Utilities.Database.Enumerations;
using System.Threading.Tasks;
using Funq;
using System.Threading;
using Ace.Agent.BaseServices;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.DiskAnalysis;
using ATAP.Utilities.DiskDrive;

namespace Ace.Agent.DiskAnalysisServices {


    public partial class DiskAnalysisServices : Service {

	public async Task<object> Post(WalkDiskDriveRequest request) {
            Log.Debug("starting Post(WalkDiskDriveRequest)");
            var diskAnalysis = new DiskAnalysis(Log);

            //ToDo: Create an association between the remote cancellation token and a new cancellation token, and record this in the RemoteToLocalCancellationToken dictionary in the Container

            // Setup the long-running task that will read one or more disk drives and CRUD a collection of DiskDriveInfoEx records, and optionally CRUD a DB Representation of that collection

            // Make a DB Representation of one or more disk drives and the partitions on those disk drives
            // Create multiple lambdas that will update SQLServerDB
            // The CRUD value comes in the request
            // If a DiskDriveNumber comes in the request, then Walk one disk drive
            // If a DiskDrivePartitionIdentifier comes in the request, then Walk the drives and partitions specified

            // There is a collection of DiskDriveInfoEx records in the ComputerInventory.ComputerHardware property of the BaseServicesData instance, which can be accessed through the DiskAnalysisServicesData.BaseServicesData property of the DiskAnalysisServicesData instance.     

            // Reading the actual Disk Drives connected to a named computer (localhost or a remote computer) can create a collection of DiskDriveInfoEx records

            // Reading a file formated to follow the conventions of the Configuration data for ComputerHardware can create a collection of DiskDriveInfoEx records
            // ToDo: figure out library Logging so that we don't need to pass a Log instance to the library

            //ToDo: Better switch on exactly what to Walk, - local machine, remote machine, configuration data, hypothetical data.
            // if the request's data is null, analyze all physical disks, else analyze the list of physical disks sent with the request

            // Create new Id for this LongRunningTask
            Id<LongRunningTaskInfo> longRunningTaskID = new Id<LongRunningTaskInfo>(Guid.NewGuid());
            // Create a new result container instance holding a new result instance for this task and add it to the Plugin's data structure 
            // DiskAnalysisServicesData.WalkDiskDriveResultContainers.Add(longRunningTaskID, new WalkDiskDriveResultContainer(new WalkDiskDriveResult()));
            LongRunningTaskInfo longRunningTaskInfo = new LongRunningTaskInfo() { Id=longRunningTaskID };
            if (request.DiskDriveNumber.HasValue) {
                longRunningTaskInfo.LRTask=new Task(() => {
                    diskAnalysis.WalkDiskDrive(
request.DiskDriveNumber.GetValueOrDefault(), 
DiskAnalysisServicesData.BaseServicesData.ComputerInventory.ComputerHardware, 
DiskAnalysisServicesData.WalkDiskDriveResultContainers[longRunningTaskID], 
(crud, r) => {
    Log.Debug($"starting recordRoot Lambda, r = {r}");
}).ConfigureAwait(false);
                });

            } else if (request.DiskDrivePartitionIdentifier!=null) {
                longRunningTaskInfo.LRTask=new Task(() => {
                    diskAnalysis.WalkDiskDrive(request.DiskDrivePartitionIdentifier, DiskAnalysisServicesData.BaseServicesData.ComputerInventory.ComputerHardware, DiskAnalysisServicesData.WalkDiskDriveResultContainers[longRunningTaskID],
                (crud, r) => {
                    Log.Debug($"starting recordRoot Lambda, r = {r}");
                }).ConfigureAwait(false);
                });
                } else { throw new InvalidOperationException("WalkDiskDriveParmatersNotUnderstood"); }


                // Record this task (plus additional information about it) in the longRunningTasks dictionary in the BaseServicesData found in the Container

                // ToDo: Setup the SSE receiver that will monitor the long-running task
                // ToDo: return to the caller the callback URL and the longRunningTaskID to allow the caller to connect to the SSE that monitors the task and the data structures it updates
                // ToDo: figure out how to integrate a CancellationToken
                DiskAnalysisServicesData.BaseServicesData.LongRunningTasks.Add(longRunningTaskID, longRunningTaskInfo);
            // record the TaskID and task info into the LookupDiskDriveAnalysisResultsCOD
            DiskAnalysisServicesData.LookupDiskDriveAnalysisResultsCOD.Add(longRunningTaskID, longRunningTaskInfo);
            // Start the task running
            try {
                DiskAnalysisServicesData.BaseServicesData.LongRunningTasks[longRunningTaskID].LRTask.Start();

            }
            catch (Exception e) when (e is InvalidOperationException||e is ObjectDisposedException) {
                Log.Debug($"Exception when trying to start the WalkDiskDrive task, message is {e.Message}");
                // ToDo: need to be sure that the when.any loop and GetLongRunningtasksStatus can handle a taskinfo in these states;
                
            }            var walkDiskDriveResponse = new WalkDiskDriveResponse(new List<Id<LongRunningTaskInfo>>() { longRunningTaskID });
                Log.Debug($"Leaving Post(WalkDiskDriveRequest), walkDiskDriveResponse = {walkDiskDriveResponse}");
                //return DiskDriveToDBGraphResponse;
                return walkDiskDriveResponse;
            }
      
    }


}
