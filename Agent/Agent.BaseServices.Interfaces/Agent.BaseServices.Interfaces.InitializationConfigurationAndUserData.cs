using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Logging;
using ATAP.Utilities.ETW;

namespace Ace.Agent.BaseServices {
    public partial class BaseServices : Service {
        public static ILog Log = LogManager.GetLogger(typeof(BaseServices));

        #region  Interfaces:GetConfigurationDataRequest
        public object Post(GetConfigurationDataRequest request) {
            GetConfigurationDataResponse getConfigurationDataResponse = new GetConfigurationDataResponse(BaseServicesData.ConfigurationData);
            return getConfigurationDataResponse;
        }
        #endregion

        #region Interfaces:GetUserDataRequest
        public object Post(GetUserDataRequest request) {
            GetUserDataResponse getUserDataResponse = new GetUserDataResponse(BaseServicesData.UserData);
            return getUserDataResponse;
        }
        #endregion

        public object Post(InitializationRequest request) {
            InitializationRequestPayload initializationRequestPayload = request.InitializationRequestPayload;
            Log.Debug($"You sent me InitializationRequestPayload = {initializationRequestPayload}");
            Log.Debug($"You sent me InitializationData = {initializationRequestPayload.InitializationData}");
            // Initialize the plugin's data structures for this service/user/session/connection
            // ToDo: Figure out if the Initialization request from the GUI has any impact on the configuration or user data structures
            InitializationData initializationData = initializationRequestPayload.InitializationData;
            // Get the BaseServicesData and diskAnalysisServicesData instances that were injected into the DI container
            var baseServicesData = HostContext.TryResolve<BaseServicesData>();
            // Create the task's action
            // Copy the BaseServices' current ConfigurationData structure to the response
            //ToDo: this is merely a placeholder until ConfigurationData is figured out
            ConfigurationData configurationData = baseServicesData.ConfigurationData;
            // Copy the Plugin's current UserData structure to the response
            // ToDo: this is merely a placeholder until UserData  is figured out
            UserData userData = baseServicesData.UserData;
            // Create and populate the Response data structure
            InitializationResponsePayload initializationResponsePayload = new InitializationResponsePayload(configurationData, userData);
            InitializationResponse initializationResponse = new InitializationResponse(initializationResponsePayload);
            // return information about this service/user/session
            return initializationResponse;
        }

        // ServiceStack will autowire this property with the corresponding instance from the DI Container
        public BaseServicesData BaseServicesData { get; set; }
        /*
        public object Any(BaseServicePutConfiguration request) {
        if(!IsAuthenticated && AppSettings.Get("LimitRemoteControlToAuthenticatedUsers", false))
        {
        throw new HttpError(HttpStatusCode.Forbidden, "You must be authenticated and authorized to use remote control.");
        }

        return new PutConfigurationResponse { Result = "SUCCESS OR FAILURE" };
        }

        public IAppSettings AppSettings {
        get; set;
        }
        */
    }
}
