using ServiceStack;
using System;
using ATAP.Utilities.LongRunningTasks;

namespace Ace.Agent.BaseServices {
    #region BaseServices Initialization
    [Route("/BaseServicesInitialization")]
    public class InitializationRequest : IReturn<InitializationResponse> {
        public InitializationRequest() :this(new InitializationRequestPayload()) {
        }

        public InitializationRequest(InitializationRequestPayload initializationRequestPayload) {
            InitializationRequestPayload=initializationRequestPayload??throw new ArgumentNullException(nameof(initializationRequestPayload));
        }

        public InitializationRequestPayload InitializationRequestPayload { get; set; }
    }

    public class InitializationRequestPayload {
        public InitializationRequestPayload() : this (new InitializationData()) { }

        public InitializationRequestPayload(InitializationData initializationData) {
            InitializationData=initializationData??throw new ArgumentNullException(nameof(initializationData));
        }

        public InitializationData InitializationData { get; set; }

    }
	
    public class InitializationResponse {
        public InitializationResponse() : this(new InitializationResponsePayload()) { }

        public InitializationResponse(InitializationResponsePayload initializationResponsePayload) {
            InitializationResponsePayload=initializationResponsePayload??throw new ArgumentNullException(nameof(initializationResponsePayload));
        }

        public InitializationResponsePayload InitializationResponsePayload { get; set; }

    }
	
    public class InitializationResponsePayload {
        public InitializationResponsePayload() : this(new ConfigurationData(), new UserData()) { }

        public InitializationResponsePayload(ConfigurationData configurationData, UserData userData) {
            ConfigurationData=configurationData??throw new ArgumentNullException(nameof(configurationData));
            UserData=userData??throw new ArgumentNullException(nameof(userData));
        }

        public ConfigurationData ConfigurationData { get; set; }
        public UserData UserData { get; set; }
    }
    #endregion

    #region GetConfigurationDataRequest, GetConfigurationDataResponse, and route GetBaseServicesConfigurationData
    [Route("/GetBaseServicesConfigurationData")]
    public class GetConfigurationDataRequest : IReturn<GetConfigurationDataResponse> {
        public GetConfigurationDataRequest() : this(new ConfigurationData()) { }
        public GetConfigurationDataRequest(ConfigurationData configurationData) {
            ConfigurationData=configurationData;
        }
        public ConfigurationData ConfigurationData { get; set; }
    }
	
    public class GetConfigurationDataResponse {
        public GetConfigurationDataResponse() : this(new ConfigurationData()) { }
        public GetConfigurationDataResponse(ConfigurationData configurationData) {
            ConfigurationData=configurationData;
        }
        public ConfigurationData ConfigurationData { get; set; }
    }
    #endregion

    #region GetUserDataRequest
    [Route("/GetBaseServicesUserData")]
    public class GetUserDataRequest : IReturn<GetUserDataResponse> {
        public GetUserDataRequest() : this(new UserData()) { }
        public GetUserDataRequest(UserData userData) {
            UserData=userData;
        }
        public UserData UserData { get; set; }

    }
    public class GetUserDataResponse {
        public GetUserDataResponse() : this(new UserData()) { }
        public GetUserDataResponse(UserData userData) {
            UserData=userData;
        }
        public UserData UserData { get; set; }
    }

	//ToDo: add SetUserdata and SetConfigurationData
    #endregion

		
    

}
