using System;
using ServiceStack;

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

  #region ReadDiskRequest, ReadDiskResponse, and Route for ReadDisk
  [Route("/ReadDisk")]
  [Route("/ReadDisk/{Filters}")]
  public class ReadDiskRequest : IReturn<ReadDiskResponse>
  {
    public ReadDiskRequest(ReadDiskRequestPayload readDiskRequestPayload) { ReadDiskRequestPayload = readDiskRequestPayload; }
    public ReadDiskRequestPayload ReadDiskRequestPayload { get; set; }
  }
  public class ReadDiskResponse
  {
    public ReadDiskResponse(ReadDiskResponsePayload readDiskResponsePayload) { ReadDiskResponsePayload = readDiskResponsePayload; }
    public ReadDiskResponsePayload ReadDiskResponsePayload { get; set; }
  }
  #endregion ReadDiskRequest, ReadDiskResponse, and Route for ReadDisk

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



