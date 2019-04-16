using System;
using ServiceStack;
using ServiceStack.Text;
using ATAP.Utilities.ComputerInventory;
using System.Collections.Generic;

namespace Ace.Agent.DiskAnalysisServices
{
  #region InitializationRequest, InitializationResponse, and Route for DiskAnalysisServicesInitialization
  [Route("/DiskAnalysisServicesInitialization")]
  public class InitializationRequest : IReturn<InitializationResponse>
  {
    public InitializationRequestPayload InitializationRequestPayload { get; set; }
  }
  public class InitializationResponse
  {
    public InitializationResponse() : this(new InitializationResponsePayload()) { }
    public InitializationResponse(InitializationResponsePayload initializationResponsePayload)
    {
      InitializationResponsePayload = initializationResponsePayload;
    }
    public InitializationResponsePayload InitializationResponsePayload { get; set; }
  }
  #endregion InitializationRequest, InitializationResponse, and Route for DiskAnalysisServicesInitialization

  #region SetConfigurationDataRequest, SetConfigurationDataResponse, and Route for SetDiskAnalysisServicesConfigurationData
  [Route("/SetDiskAnalysisServicesConfigurationData")]
  public class SetConfigurationDataRequest : IReturn<SetConfigurationDataResponse>
  {
    public SetConfigurationDataRequestPayload SetConfigurationDataRequestPayload { get; set; }
  }
  public class SetConfigurationDataResponse
  {
    public SetConfigurationDataResponse() : this(new SetConfigurationDataResponsePayload()) { }
    public SetConfigurationDataResponse(SetConfigurationDataResponsePayload setConfigurationDataResponsePayload)
    {
      SetConfigurationDataResponsePayload = setConfigurationDataResponsePayload;
    }
    public SetConfigurationDataResponsePayload SetConfigurationDataResponsePayload { get; set; }
  }
  #endregion SetConfigurationDataRequest, SetConfigurationDataResponse, and Route for SetDiskAnalysisServicesConfigurationData

  #region GetConfigurationDataRequest, GetConfigurationDataResponse, and Route for GetDiskAnalysisServicesConfigurationDataRequest
  [Route("/GetDiskAnalysisServicesConfigurationData")]
  [Route("/GetDiskAnalysisServicesConfigurationData/{GetDiskAnalysisServicesConfigurationDataRequestPayload}")]
  public class GetConfigurationDataRequest : IReturn<GetConfigurationDataResponse>
  {
    public GetConfigurationDataRequestPayload GetConfigurationDataRequestPayload { get; set; }
  }
  public class GetConfigurationDataResponse
  {
    public GetConfigurationDataResponse() : this(new GetConfigurationDataResponsePayload()) { }
    public GetConfigurationDataResponse(GetConfigurationDataResponsePayload getConfigurationDataResponsePayload)
    {
      GetConfigurationDataResponsePayload = getConfigurationDataResponsePayload;
    }
    public GetConfigurationDataResponsePayload GetConfigurationDataResponsePayload { get; set; }
  }
  #endregion GetDiskAnalysisServicesConfigurationDataRequest, GetDiskAnalysisServicesConfigurationDataResponse, and Route for GetDiskAnalysisServicesConfigurationDataRequest

  #region SetUserDataRequest, SetUserDataResponse and Route for SetDiskAnalysisServicesUserData
  [Route("/SetDiskAnalysisServicesUserData")]
  [Route("/SetDiskAnalysisServicesUserData/{SetUserRequestPayload}")]
  public class SetUserDataRequest : IReturn<SetUserDataResponse>
  {
    public SetUserDataRequest() : this(new SetUserDataRequestPayload()) { }
    public SetUserDataRequest(SetUserDataRequestPayload setUserDataRequestPayload)
    {
      SetUserDataRequestPayload = setUserDataRequestPayload;
    }
    public SetUserDataRequestPayload SetUserDataRequestPayload { get; set; }
  }

  public class SetUserDataResponse
  {
    public SetUserDataResponse() : this(new SetUserDataResponsePayload()) { }
    public SetUserDataResponse(SetUserDataResponsePayload setUserDataResponsePayload)
    {
      SetUserDataResponsePayload = setUserDataResponsePayload;
    }
    public SetUserDataResponsePayload SetUserDataResponsePayload { get; set; }
  }
  #endregion SetUserDataRequest, SetUserDataResponse and Route for SetDiskAnalysisServicesUserData

  #region GetUserDataRequest, GetUserDataResponse and Route for GetDiskAnalysisServicesUserData
  [Route("/GetDiskAnalysisServicesUserData")]
  [Route("/GetDiskAnalysisServicesUserData/{GetDiskAnalysisServicesUserDataRequestPayload}")]
  public class GetUserDataRequest : IReturn<GetUserDataResponse>
  {
    public GetUserDataRequestPayload GetUserDataRequestPayload { get; set; }
  }

  public class GetUserDataResponse
  {
    public GetUserDataResponse() : this(new GetUserDataResponsePayload()) { }
    public GetUserDataResponse(GetUserDataResponsePayload getUserDataResponsePayload)
    {
      GetUserDataResponsePayload = getUserDataResponsePayload;
    }
    public GetUserDataResponsePayload GetUserDataResponsePayload { get; set; }
  }
    #endregion GetUserDataRequest, GetUserDataResponsePayload and Route for GetDiskAnalysisServicesUserData

    #region DiskDriveManyToDBGraphRequest, DiskDriveManyToDBGraphResponse, and Route for DiskDriveManyToDBGraph
    [Route("/DiskDriveManyToDBGraph")]
  [Route("/DiskDriveManyToDBGraph/{DiskDriveAndPartitionSpecification}")]
  public class DiskDriveManyToDBGraphRequest : IReturn<DiskDriveManyToDBGraphResponse>
  {
    public DiskDriveManyToDBGraphRequest(IEnumerable<DiskDrivePartitionDriveLetterIdentifier> diskDrivePartitionDriveLetterIdentifiers) { DiskDrivePartitionDriveLetterIdentifiers=diskDrivePartitionDriveLetterIdentifiers; }
    public IEnumerable<DiskDrivePartitionDriveLetterIdentifier> DiskDrivePartitionDriveLetterIdentifiers { get; set; }
  }
  public class DiskDriveManyToDBGraphResponse {
    public DiskDriveManyToDBGraphResponse() : this(new DiskDriveToDBGraphResponseData()) { }
    public DiskDriveManyToDBGraphResponse(DiskDriveToDBGraphResponseData diskDriveManyToDBGraphResponseData) { DiskDriveManyToDBGraphResponseData=diskDriveManyToDBGraphResponseData; }
    public DiskDriveToDBGraphResponseData DiskDriveManyToDBGraphResponseData { get; set; }
  }
  #endregion 

  #region GetLongRunningTaskStateRequest, GetLongRunningTaskStateResponse and Route for GetLongRunningTaskState
  [Route("/GetLongRunningTaskState")]
  [Route("/GetLongRunningTaskState/{LongRunningTaskID}")]
  public class GetLongRunningTaskStateRequest : IReturn<SetUserDataResponse>
  {
    public GetLongRunningTaskStateRequest() : this(Guid.Empty) { }
    public GetLongRunningTaskStateRequest(Guid longRunningTaskID)
    {
      LongRunningTaskID = longRunningTaskID;
    }
    public Guid LongRunningTaskID { get; set; }
  }

  public class GetLongRunningTaskStateResponse
  {
    public GetLongRunningTaskStateResponse() : this("DefaultTaskState") { }
    public GetLongRunningTaskStateResponse(string longRunningTaskState)
    {
      LongRunningTaskState = longRunningTaskState;
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



