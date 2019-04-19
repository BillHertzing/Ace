using System;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Text;
using Swordfish.NET.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Collections.Generic;
using ATAP.Utilities.ComputerInventory;
using ATAP.Utilities.ComputerInventory.Enumerations;
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


    public class DiskAnalysisServices : Service {
        public static ServiceStack.Logging.ILog Log = LogManager.GetLogger(typeof(DiskAnalysisServices));

        public object Post(InitializationRequest request) {
            Log.Debug("starting Post(InitializationRequest request)");
            /* Currently Initialization Request sends no data
            InitializationRequestPayload initializationRequestPayload = request.InitializationRequestPayload;
            Log.Debug($"You sent me InitializationRequestPayload = {initializationRequestPayload}");
            Log.Debug($"You sent me InitializationData = {initializationRequestPayload.InitializationData}");
            // Initialize the plugin's data structures for this service/user/session/connection
            // ToDo: Figure out if the Initialization request from the GUI has any impact on the configuration or user data structures
            InitializationData initializationData = initializationRequestPayload.InitializationData;
            */
            // Copy the Plugin's current ConfigurationData structure to the response
            //ToDo: this is merly a placeholder until ConfigurationData is figured out
            ConfigurationData configurationData = DiskAnalysisServicesData.ConfigurationData;
            // Copy the Plugin's current UserData structure to the response
            //ToDo: this is merly a placeholder until UserData  is figured out
            UserData userData = new UserData(DiskAnalysisServicesData.PluginRootCOD.Values.ToString());
            //ToDO: remove this temporary delay
            Thread.Sleep(5000);
            // Create and populate the Response data structure
            InitializationResponsePayload initializationResponsePayload = new InitializationResponsePayload(configurationData, userData);
            InitializationResponse initializationResponse = new InitializationResponse(initializationResponsePayload);
            // return information about this service/user/session
            Log.Debug($"leaving Post(DiskAnalysisServicesInitializationRequest request), returning {initializationResponse}");
            return initializationResponse;
        }

        public object Post(SetConfigurationDataRequest request) {
            Log.Debug("starting Post(SetConfigurationDataRequest request)");
            Log.Debug($"You sent me SetConfigurationDataRequest = {request}");
            if (request.ConfigurationDataSave) {
                // Action to take if "save" is true
                DiskAnalysisServicesData.ConfigurationData=request.ConfigurationData;
            }
            //} else { // Action to take if "save" is false }
            // return information about this service/user/session
            string result = "OK";
            Log.Debug($"leaving Any(SetConfigurationDataRequest request), returning {result}");
            return new SetConfigurationDataResponse(new SetConfigurationDataResponsePayload(result));
        }

        public object Post(SetUserDataRequest request) {
            Log.Debug("starting Post(SetUserDataRequest request)");
            Log.Debug($"You sent me SetUserDataRequest = {request}");
            SetUserDataRequestPayload setUserDataRequestPayload = request.SetUserDataRequestPayload;
            Log.Debug($"You sent me SetUserDataRequestPayload = {setUserDataRequestPayload}");
            UserData userData = setUserDataRequestPayload.UserData;
            Log.Debug($"You sent me UserData = {userData}");
            // ToDo: action to take if "save" is false
            // ToDo: Action to ttake if "save" is true
            // ToDo: Update the DiskAnalysisServicesConfigurationData (COD and its subscribers) in the Data assembly
            // return information about this service/user/session
            string result = "OK";
            Log.Debug($"leaving Post(SetUserDataRequest request), returning {result}");
            return new SetUserDataResponse(new SetUserDataResponsePayload(result));
        }
        public async Task<object> Post(WalkDiskDriveRequest request) {
            Log.Debug("starting Post(WalkDiskDriveRequest)");
            var diskAnalysis = new DiskAnalysis(Log);

            //ToDo: Create an association between the remote cancellation token and a new cancellation token, and record this in the RemoteToLocalCancellationToken dictionary in the Container

            // Setup the long-running task that will read one or more disk drives and CRUD a collection of DiskDriveInfoEx records, and optionally CRUD a DB Representation of that collection

            // Make a DB Representation of one or more disk drives and the partitions on those disk drives
            // Create multiple lambdas that will update SQLServerDB
            // The CRUD value comes in the request
            // If a DiskDriveNumber comes in the request, then Walk one disk drive
            // If a DiskDrivePartitionIdentifier comes in the request, then Walk the drives and prtitions specified

            // There is a collection of DiskDriveInfoEx records in the ComputerInventory.ComputerHardware property of the BaseServicesData instance, which can be accessed through the DiskAnalysisServicesData.BaseServciesData property of teh DiskAnalysisServicesData instance.     

            // Reading the actual Disk Drives connected to a named computer (localhost or a remote computer) can create a collection of DiskDriveInfoEx records

            // Reading a file formated to follow the conventions of the Configuration data for ComputerHardware can create a collection of DiskDriveInfoEx records
            // ToDo: figure out library Logging so that we don't need to pass a Log instance to the library

            //ToDo: Better switch on exactly what to Walk, - local machine, remote machine, configuraiotn data, hypothetical data.
            // if the request's data is null, analyze all physical disks, else analyze the list of physical disks sent with the request

            // Create new Id for this LongRunningTask
            Id<LongRunningTaskInfo> longRunningTaskID = new Id<LongRunningTaskInfo>(Guid.NewGuid());
            // Create a new result container instance holding a new result instance for this task and add it to the Plugin's data structure 
            DiskAnalysisServicesData.WalkDiskDriveResultContainers.Add(longRunningTaskID, new WalkDiskDriveResultContainer(new WalkDiskDriveResult()));
            LongRunningTaskInfo longRunningTaskInfo = new LongRunningTaskInfo() { Id=longRunningTaskID };
            if (request.DiskDriveNumber.HasValue) {
                longRunningTaskInfo.LRTask=new Task(() => {
                    diskAnalysis.WalkDiskDrive(
request.DiskDriveNumber.GetValueOrDefault(), 
DiskAnalysisServicesData.BaseServicesData.ComputerInventory.ComputerHardware, 
DiskAnalysisServicesData.WalkDiskDriveResultContainers[longRunningTaskID], 
(crud, r) => {
    Log.Debug($"starting recordRoot Lambda, r = {r}");
});
                });

            } else if (request.DiskDrivePartitionIdentifier!=null) {
                longRunningTaskInfo.LRTask=new Task(() => {
                    diskAnalysis.WalkDiskDrive(request.DiskDrivePartitionIdentifier, DiskAnalysisServicesData.BaseServicesData.ComputerInventory.ComputerHardware, DiskAnalysisServicesData.WalkDiskDriveResultContainers[longRunningTaskID],
                (crud, r) => {
                    Log.Debug($"starting recordRoot Lambda, r = {r}");
                });
                });
                } else { throw new InvalidOperationException("WalkDiskDriveParmatersNotUnderstood"); }


                // Record this task (plus additional information about it) in the longRunningTasks dictionary in the BaseServicesData found in the Container

                // ToDo: Setup the SSE receiver that will monitor the long-running task
                // ToDo: return to the caller the callback URL and the longRunningTaskID to allow the caller to connect to the SSE that monitors the task and the data structures it updates
                // ToDo: figure out how to integrate a CancellationToken
                DiskAnalysisServicesData.BaseServicesData.LongRunningTasks.Add(longRunningTaskID, longRunningTaskInfo);

                var walkDiskDriveResponse = new WalkDiskDriveResponse(new List<Id<LongRunningTaskInfo>>() { longRunningTaskID });
                Log.Debug($"Leaving Post(WalkDiskDriveRequest), walkDiskDriveResponse = {walkDiskDriveResponse}");
                //return DiskDriveToDBGraphResponse;
                return walkDiskDriveResponse;
            }
        public DiskAnalysisServicesData DiskAnalysisServicesData { get; set; }

        public Funq.Container Container { get; set; }
    }


}
