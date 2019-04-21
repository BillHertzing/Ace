using System;
using ServiceStack;
using ServiceStack.Text;
using ATAP.Utilities.ComputerInventory;
using System.Collections.Generic;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.DiskDrive;
using Ace.Agent.BaseServices;

namespace Ace.Agent.DiskAnalysisServices {

    //ToDo: Clean up region names
    #region InitializationRequest and Payload, InitializationResponse and Payload, and Route for DiskAnalysisServicesInitialization
    [Route("/DiskAnalysisServicesInitialization")]
    public class InitializationRequest : IReturn<InitializationResponse> {
        public InitializationRequest() : this(new InitializationRequestPayload()) { }

        public InitializationRequest(InitializationRequestPayload initializationRequestPayload) {
            InitializationRequestPayload=initializationRequestPayload??throw new ArgumentNullException(nameof(initializationRequestPayload));
        }

        public InitializationRequestPayload InitializationRequestPayload { get; set; }
    }

    public class InitializationRequestPayload {
        public InitializationRequestPayload() : this(new InitializationData()) { }
        public InitializationRequestPayload(InitializationData initializationData) { InitializationData=initializationData; }

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
        public InitializationResponsePayload() : this(new ConfigurationData(),
                                                                           new UserData()) { }

        public InitializationResponsePayload(ConfigurationData configurationData, UserData userData) {
            ConfigurationData=configurationData??throw new ArgumentNullException(nameof(configurationData));
            UserData=userData??throw new ArgumentNullException(nameof(userData));
        }

        public ConfigurationData ConfigurationData { get; set; }

        public UserData UserData { get; set; }
    }
    #endregion

    #region SetConfigurationDataRequest, SetConfigurationDataResponse, and Route for SetDiskAnalysisServicesConfigurationData
    [Route("/SetDiskAnalysisServicesConfigurationData")]
    public class SetConfigurationDataRequest : IReturn<SetConfigurationDataResponse> {
        public ConfigurationData ConfigurationData { get; set; }
        public bool ConfigurationDataSave { get; set; }
    }
    public class SetConfigurationDataRequestPayload {
        public SetConfigurationDataRequestPayload() : this(new ConfigurationData(),
                                                                                false) { }
        public SetConfigurationDataRequestPayload(ConfigurationData configurationData, bool saveConfigurationData) {
            ConfigurationData=configurationData;
            SaveConfigurationData=saveConfigurationData;
        }

        public ConfigurationData ConfigurationData { get; set; }

        public bool SaveConfigurationData { get; set; }
    }
    public class SetConfigurationDataResponse {
        public SetConfigurationDataResponse() : this(new SetConfigurationDataResponsePayload()) { }
        public SetConfigurationDataResponse(SetConfigurationDataResponsePayload setConfigurationDataResponsePayload) {
            SetConfigurationDataResponsePayload=setConfigurationDataResponsePayload;
        }
        public SetConfigurationDataResponsePayload SetConfigurationDataResponsePayload { get; set; }
    }
    public class SetConfigurationDataResponsePayload {
        public SetConfigurationDataResponsePayload() : this(string.Empty) { }
        public SetConfigurationDataResponsePayload(string result) { Result=result; }

        public string Result { get; set; }
    }
    #endregion

    #region GetConfigurationDataRequest, GetConfigurationDataResponse, and Route for GetDiskAnalysisServicesConfigurationDataRequest
    [Route("/GetDiskAnalysisServicesConfigurationData")]
    public class GetConfigurationDataRequest : IReturn<GetConfigurationDataResponse> {
        public GetConfigurationDataRequestPayload GetConfigurationDataRequestPayload { get; set; }
    }
    public class GetConfigurationDataRequestPayload {
        public GetConfigurationDataRequestPayload() : this(string.Empty) { }
        public GetConfigurationDataRequestPayload(string placeholder) { Placeholder=placeholder; }

        public string Placeholder { get; set; }
    }
    public class GetConfigurationDataResponse {
        public GetConfigurationDataResponse() : this(new ConfigurationData()) { }
        public GetConfigurationDataResponse(ConfigurationData configurationData) {
            ConfigurationData=configurationData;
        }
        public ConfigurationData ConfigurationData { get; set; }
    }
    public class GetConfigurationDataResponsePayload {
        public GetConfigurationDataResponsePayload() : this(new ConfigurationData()) { }
        public GetConfigurationDataResponsePayload(ConfigurationData configurationData) { ConfigurationData=configurationData; }

        public ConfigurationData ConfigurationData { get; set; }
    }
    #endregion

    #region SetUserDataRequest, SetUserDataResponse and Route for SetDiskAnalysisServicesUserData
    [Route("/SetDiskAnalysisServicesUserData")]
    [Route("/SetDiskAnalysisServicesUserData/{SetUserRequestPayload}")]
    public class SetUserDataRequest : IReturn<SetUserDataResponse> {
        public SetUserDataRequest() : this(new SetUserDataRequestPayload()) { }
        public SetUserDataRequest(SetUserDataRequestPayload setUserDataRequestPayload) {
            SetUserDataRequestPayload=setUserDataRequestPayload;
        }
        public SetUserDataRequestPayload SetUserDataRequestPayload { get; set; }
    }
    public class SetUserDataRequestPayload {
        public SetUserDataRequestPayload() : this(new UserData(), false) { }

        public SetUserDataRequestPayload(UserData userData, bool userDataSave) {
            UserData=userData;
            UserDataSave=userDataSave;
        }

        public UserData UserData { get; set; }

        public bool UserDataSave { get; set; }
    }
    public class SetUserDataResponse {
        public SetUserDataResponse() : this(new SetUserDataResponsePayload()) { }
        public SetUserDataResponse(SetUserDataResponsePayload setUserDataResponsePayload) {
            SetUserDataResponsePayload=setUserDataResponsePayload;
        }
        public SetUserDataResponsePayload SetUserDataResponsePayload { get; set; }
    }
    public class SetUserDataResponsePayload {
        public SetUserDataResponsePayload() : this(string.Empty) { }
        public SetUserDataResponsePayload(string result) { Result=result; }

        public string Result { get; set; }
    }
    #endregion

    #region GetUserDataRequest, GetUserDataResponse and Route for GetDiskAnalysisServicesUserData
    [Route("/GetDiskAnalysisServicesUserData")]
    [Route("/GetDiskAnalysisServicesUserData/{GetDiskAnalysisServicesUserDataRequestPayload}")]
    public class GetUserDataRequest : IReturn<GetUserDataResponse> {
        public GetUserDataRequestPayload GetUserDataRequestPayload { get; set; }
    }
    public class GetUserDataRequestPayload {
        public GetUserDataRequestPayload() : this(string.Empty) { }
        public GetUserDataRequestPayload(string placeholder) { Placeholder=placeholder; }

        public string Placeholder { get; set; }
    }
    public class GetUserDataResponse {
        public GetUserDataResponse() : this(new GetUserDataResponsePayload()) { }
        public GetUserDataResponse(GetUserDataResponsePayload getUserDataResponsePayload) {
            GetUserDataResponsePayload=getUserDataResponsePayload;
        }
        public GetUserDataResponsePayload GetUserDataResponsePayload { get; set; }
    }
    public class GetUserDataResponsePayload {
        public GetUserDataResponsePayload() : this(new UserData()) { }
        public GetUserDataResponsePayload(UserData userData) { this.UserData=userData; }

        public UserData UserData { get; set; }
    }
    #endregion


    #region DiskDrivesToDBGraphRequest, DiskDrivesToDBGraphResponse, and Route for DiskDrivesToDBGraph
    [Route("/WalkDiskDrive")]
    //[Route("/WalkDiskDrive/{DiskDriveNumber}")]
    //[Route("/WalkDiskDrive/{DiskDrivePartitionIdentifier}")]
    public class WalkDiskDriveRequest : IReturn<WalkDiskDriveResponse> {
        public WalkDiskDriveRequest(int? diskNumber, DiskDrivePartitionIdentifier diskDrivePartitionIdentifier) {
            DiskDriveNumber=diskNumber;
            DiskDrivePartitionIdentifier=diskDrivePartitionIdentifier;
        }
        public WalkDiskDriveRequest(DiskDrivePartitionIdentifier diskDrivePartitionIdentifier) { }
        public int? DiskDriveNumber { get; set; }
        public DiskDrivePartitionIdentifier DiskDrivePartitionIdentifier { get; set; }
    }
    public class WalkDiskDriveResponse {
        public WalkDiskDriveResponse() : this(new List<Id<LongRunningTaskInfo>>()) { }
        public WalkDiskDriveResponse(List<Id<LongRunningTaskInfo>> longRunningTaskIDs) { LongRunningTaskIDs=longRunningTaskIDs; }
        public List<Id<LongRunningTaskInfo>> LongRunningTaskIDs { get; set; }
    }
    #endregion

    #region GetLongRunningTaskStateRequest, GetLongRunningTaskStateResponse and Route for GetLongRunningTaskState
    [Route("/GetLongRunningTaskState")]
    [Route("/GetLongRunningTaskState/{LongRunningTaskID}")]
    public class GetLongRunningTaskStateRequest : IReturn<SetUserDataResponse> {
        //public GetLongRunningTaskStateRequest() : this(Guid.Empty) { }
        public GetLongRunningTaskStateRequest(List<Id<LongRunningTaskInfo>> longRunningTaskIDs) {
            LongRunningTaskIDs=longRunningTaskIDs;
        }
        public List<Id<LongRunningTaskInfo>> LongRunningTaskIDs { get; set; }
    }

    public class GetLongRunningTaskStateResponse {
        public GetLongRunningTaskStateResponse() : this("DefaultTaskState") { }
        public GetLongRunningTaskStateResponse(string longRunningTaskState) {
            LongRunningTaskState=longRunningTaskState;
        }
        public string LongRunningTaskState { get; set; }
    }
    #endregion GetLongRunningTaskStateRequest, GetLongRunningTaskStateResponse and Route for GetLongRunningTaskState

    /*
    #region Monitor Data Structures
    [Route("/MonitorDiskAnalysisServicesDataStructures")]
      public class MonitorDiskAnalysisServicesDataStructuresRequest : IReturn<MonitorDiskAnalysisServicesDataStructuresResponse>
    {
      public string Filters { get; set; }
    }
    public class MonitorDiskAnalysisServicesDataStructuresResponse
    {
      public string[] Result { get; set; }
      public Operation Kind { get; set; }
    }
    #endregion
    */

}



