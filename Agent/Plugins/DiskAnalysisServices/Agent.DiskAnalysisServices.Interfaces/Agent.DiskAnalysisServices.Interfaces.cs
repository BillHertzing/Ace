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

            // Create and populate the Response data structure
            InitializationResponsePayload initializationResponsePayload = new InitializationResponsePayload(configurationData, userData);
            InitializationResponse initializationResponse = new InitializationResponse(DiskAnalysisServicesData.ConfigurationData, DiskAnalysisServicesData.UserData);
            // return information about this service/user/session
            Log.Debug($"leaving Post(DiskAnalysisServicesInitializationRequest request), returning {initializationResponse}");
            return initializationResponse;
        }

        public object Post(SetConfigurationDataRequest request) {
            Log.Debug("starting Post(SetConfigurationDataRequest request)");
            Log.Debug($"You sent me SetConfigurationDataRequest = {request.Dump<SetConfigurationDataRequest>()}");
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
        public async Task<object> Post(DiskDrivesToDBGraphRequest request) {
            Log.Debug("starting Post(DiskDrivesToDBGraphRequest)");
            var diskDrivePartitionDriveLetterIdentifiers = request.DiskDrivePartitionDriveLetterIdentifiers;
            //ToDo: Create an association between the remote cancellation token and a new cancellation token, and record this in the RemoteToLocalCancellationToken dictionary in the Container

            // Setup the long-running task that will read the disk and update the plugin's data structures
            // Create a lambda that will update SQLServerDB

            // Make a DB Representation of all the file system entities on one or more of the diskdrives in the current ComputerInventory
            // The CRUD value comes in the request
            // The DiskDriveNumbers come in the request
            // The Partition Identifiers for each DiskDriveNumber come in the request
            // The current ComputerInventory is in the DiskAnalysisServicesData.BaseServciesData.ComputerInventory
            // 
            // ToDo: figure out library Logging so that we don't need to pass a Log instance to the library
            var analyzeDisk = new AnalyzeDisk(4095,Log);
            //ToDo: if the request's data is null, analyze all physical disks, else analyze the list of physical disks sent with the request
            var task = await analyzeDisk.DiskDriveManyToDBGraphAsync(diskDrivePartitionDriveLetterIdentifiers, DiskAnalysisServicesData.BaseServicesData.ComputerInventory, CrudType.Create, (r) => {
                Log.Debug($"starting recordRoot Lambda, r = {r}");
            });
            // Record this task (plus additional information about it) in the longRunningTasks dictionary in the Container
            var longRunningTaskID = Guid.NewGuid();
            var lRTaskInfo = new LRTaskInfo(longRunningTaskID, task);
            DiskAnalysisServicesData.BaseServicesData.LongRunningTasks.Add(lRTaskInfo.ID, lRTaskInfo);

            // Setup the SSE receiver that will monitor the long-running task
            // return to the caller the callback URL and the longRunningTaskID to allow the caller to connect to the SSE that monitors the task and the data structures it updates
            // ToDo: figure out how to integrate a CancellationToken
            // Long running task: update the Plugin Data Structure with the data from the response
            // Long running task: setup the DiskAnalysisServicesData.PluginRootCOD.Add("test1", 100);

            var diskDriveToDBGraphTasks = new List<Guid>();
            diskDriveToDBGraphTasks.Add(longRunningTaskID);
            var DiskDriveToDBGraphResponse = new DiskDriveToDBGraphResponseData(diskDriveToDBGraphTasks);
            Log.Debug($"Leaving Post(DiskDriveManyToDBGraphRequest), DiskDriveToDBGraphResponse = {DiskDriveToDBGraphResponse.Dump()}");
            //return DiskDriveToDBGraphResponse;
            return DiskDriveToDBGraphResponse;
        }
        public DiskAnalysisServicesData DiskAnalysisServicesData { get; set; }

        public Funq.Container Container { get; set; }
    }


}
