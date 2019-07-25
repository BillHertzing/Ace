using System;
using ServiceStack;
using ServiceStack.Text;
using Serilog;
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
using ATAP.Utilities.DiskDriveAnalysis;
using ATAP.Utilities.DiskDrive;

namespace Ace.Plugin.DiskAnalysisServices {


    public partial class DiskAnalysisServices : Service {


        public object Post(InitializationRequest request) {
            InitializationRequestPayload initializationRequestPayload = request.InitializationRequestPayload;
            Log.Debug($"You sent me InitializationRequestPayload = {initializationRequestPayload}");
            Log.Debug($"You sent me InitializationData = {initializationRequestPayload.InitializationData}");
            // Initialize the plugin's data structures for this service/user/session/connection
            // ToDo: Figure out if the Initialization request from the GUI has any impact on the configuration or user data structures
            InitializationData initializationData = initializationRequestPayload.InitializationData;
            // Get the BaseServicesData and diskAnalysisServicesData instances that were injected into the DI container
            var baseServicesData = HostContext.TryResolve<BaseServicesData>();
            var diskAnalysisServicesData = HostContext.TryResolve<DiskAnalysisServicesData>();
            // Create the task's action
            // Copy the Plugin's current ConfigurationData structure to the response
            //ToDo: this is merely a placeholder until ConfigurationData is figured out
            ConfigurationData configurationData = diskAnalysisServicesData.ConfigurationData;
            // Copy the Plugin's current UserData structure to the response
            // ToDo: this is merely a placeholder until UserData  is figured out
            UserData userData = diskAnalysisServicesData.UserData;
             // Create and populate the Response data structure
            InitializationResponsePayload initializationResponsePayload = new InitializationResponsePayload(configurationData, userData);
            InitializationResponse initializationResponse = new InitializationResponse(initializationResponsePayload);
            // return information about this service/user/session
            return initializationResponse;
        }

        public object Post(SetConfigurationDataRequest request) {
            Log.Debug($"You sent me SetConfigurationDataRequest = {request}");
            // ToDo: turn this into a Task, and put it on the LongRunningTasks in the BaseServiceData

            // Get the remote cancellation token from the request
            // Create a new cancellation token
            // Create the association between the remote cancellation token and the local cancellation Token, store in the BaseServicesData
            // Get the BaseServicesData and diskAnalysisServicesData instances that were injected into the DI container
            var baseServicesData = HostContext.TryResolve<BaseServicesData>();
            var diskAnalysisServicesData = HostContext.TryResolve<DiskAnalysisServicesData>();
            
            // Define the lambda that describes the task
            // Update the plugin's Data object's ConfigurationData
            if (request.ConfigurationDataSave) {
                // Action to take if "save" is true
                diskAnalysisServicesData.ConfigurationData=request.ConfigurationData;
            }
            //} else { // Action to take if "save" is false }
			// ToDo: Start the LongRunningTask
            // ToDo: return the LongRunningTaskId
            string result = "OK";
            return new SetConfigurationDataResponse(new SetConfigurationDataResponsePayload(result));
        }

        public object Post(SetUserDataRequest request) {
		Log.Debug($"You sent me SetUserDataRequest = {request}");
            SetUserDataRequestPayload setUserDataRequestPayload = request.SetUserDataRequestPayload;
            Log.Debug($"You sent me SetUserDataRequestPayload = {setUserDataRequestPayload}, UserData = {request.SetUserDataRequestPayload.UserData}");
            // ToDo: turn this into a Task, and return the LongRunningtaskId
            // Get the BaseServicesData and diskAnalysisServicesData instances that were injected into the DI container
            var baseServicesData = HostContext.TryResolve<BaseServicesData>();
            var diskAnalysisServicesData = HostContext.TryResolve<DiskAnalysisServicesData>();

            // Define the lambda that describes the task
            // Update the plugin's Data object's UserData
            if (request.SetUserDataRequestPayload.UserDataSave) {
                // Action to take if "save" is true
                diskAnalysisServicesData.UserData=request.SetUserDataRequestPayload.UserData;
            }
            //} else { // Action to take if "save" is false }

			// ToDo: Start the LongRunningTask
            // ToDo: return the LongRunningTaskId
            string result = "OK";
            return new SetUserDataResponse(new SetUserDataResponsePayload(result));
        }


    }


}
