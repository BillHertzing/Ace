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
        public static ServiceStack.Logging.ILog Log = LogManager.GetLogger(typeof(DiskAnalysisServices));

        public object Post(InitializationRequest request) {
            Log.Debug("starting Post(InitializationRequest request)");
            InitializationRequestPayload initializationRequestPayload = request.InitializationRequestPayload;
            Log.Debug($"You sent me InitializationRequestPayload = {initializationRequestPayload}");
            Log.Debug($"You sent me InitializationData = {initializationRequestPayload.InitializationData}");
            // Initialize the plugin's data structures for this service/user/session/connection
            // ToDo: Figure out if the Initialization request from the GUI has any impact on the configuration or user data structures
            InitializationData initializationData = initializationRequestPayload.InitializationData;
            // Copy the Plugin's current ConfigurationData structure to the response
            //ToDo: this is merly a placeholder until ConfigurationData is figured out
            ConfigurationData configurationData = DiskAnalysisServicesData.ConfigurationData;
            // Copy the Plugin's current UserData structure to the response
            // ToDo: this is merly a placeholder until UserData  is figured out
            UserData userData = new UserData(DiskAnalysisServicesData.PluginRootCOD.Values.ToString());
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
			// ToDo: turn this into a Task, and put it on the LongRunningTasks in the BaseServiceData
			// Get the remote cancellation token from the request
			// Create a new cancellation token
			// Create the association between the remote cancellation token and the local cancellation Token, store in the BaseServicesData
			// Update the plugin's Data object's ConfigurationData
            if (request.ConfigurationDataSave) {
                // Action to take if "save" is true
                DiskAnalysisServicesData.ConfigurationData=request.ConfigurationData;
            }
            //} else { // Action to take if "save" is false }
			// ToDo: Start the LongRunningTask
            // ToDo: return the LongRunningTaskId
            string result = "OK";
            Log.Debug($"leaving Any(SetConfigurationDataRequest request), returning {result}");
            return new SetConfigurationDataResponse(new SetConfigurationDataResponsePayload(result));
        }

        public object Post(SetUserDataRequest request) {
            Log.Debug("starting Post(SetUserDataRequest request)");
		Log.Debug($"You sent me SetUserDataRequest = {request}");
            SetUserDataRequestPayload setUserDataRequestPayload = request.SetUserDataRequestPayload;
            Log.Debug($"You sent me SetUserDataRequestPayload = {setUserDataRequestPayload}, UserData = {DiskAnalysisServicesData.UserData}");
					// ToDo: turn this into a Task, and return the LongRunningtaskId
			// Get the remote cancellation token from the request
			// Create a new cancellation token
			// Create the association between the remote cancellation token and the local cancellation Token, store in the BaseServicesData
			// Update the plugin's Data object's UserData
			if (request.SetUserDataRequestPayload.UserDataSave) {
                // Action to take if "save" is true
                DiskAnalysisServicesData.UserData=request.SetUserDataRequestPayload.UserData;
            }
            //} else { // Action to take if "save" is false }

			// ToDo: Start the LongRunningTask
            // ToDo: return the LongRunningTaskId
            string result = "OK";
            Log.Debug($"leaving Post(SetUserDataRequest request), returning {result}");
            return new SetUserDataResponse(new SetUserDataResponsePayload(result));
        }
		
		// This is the code block where the DiskAnalysisServices assembly declares it's plugin's data 
        public DiskAnalysisServicesData DiskAnalysisServicesData { get; set; }
    }


}
