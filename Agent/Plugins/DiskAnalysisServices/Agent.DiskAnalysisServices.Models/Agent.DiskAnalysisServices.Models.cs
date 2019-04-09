using System;
using ServiceStack;

namespace Ace.Agent.DiskAnalysisServices
{
  #region DiskAnalysisServicesInitializationRequest, DiskAnalysisServicesInitializationResponse, and Route for DiskAnalysisServicesInitialization
  [Route("/DiskAnalysisServicesInitialization")]
  public class DiskAnalysisServicesInitializationRequest : IReturn<DiskAnalysisServicesInitializationResponse>
  {
    public DiskAnalysisServicesInitializationRequestPayload DiskAnalysisServicesInitializationRequestPayload { get; set; }
  }
  public class DiskAnalysisServicesInitializationResponse
  {
    public DiskAnalysisServicesInitializationResponse() : this(new DiskAnalysisServicesInitializationResponsePayload()) { }
    public DiskAnalysisServicesInitializationResponse(DiskAnalysisServicesInitializationResponsePayload diskAnalysisServicesInitializationResponsePayload)
    {
      DiskAnalysisServicesInitializationResponsePayload = diskAnalysisServicesInitializationResponsePayload;
    }
    public DiskAnalysisServicesInitializationResponsePayload DiskAnalysisServicesInitializationResponsePayload { get; set; }
  }
  #endregion DiskAnalysisServicesInitializationRequest, DiskAnalysisServicesInitializationResponse, and Route for DiskAnalysisServicesInitialization

  #region SetDiskAnalysisServicesConfigurationDataRequest, SetDiskAnalysisServicesConfigurationDataResponse, and Route for SetDiskAnalysisServicesConfigurationData
  [Route("/SetDiskAnalysisServicesConfigurationData")]
  public class SetDiskAnalysisServicesConfigurationDataRequest : IReturn<SetDiskAnalysisServicesConfigurationDataResponsePayload>
  {
    public SetDiskAnalysisServicesConfigurationDataRequestPayload SetDiskAnalysisServicesConfigurationDataRequestPayload { get; set; }
  }
  public class SetDiskAnalysisServicesConfigurationDataResponse
  {
    public SetDiskAnalysisServicesConfigurationDataResponse() : this(new SetDiskAnalysisServicesConfigurationDataResponsePayload()) { }
    public SetDiskAnalysisServicesConfigurationDataResponse(SetDiskAnalysisServicesConfigurationDataResponsePayload setDiskAnalysisServicesConfigurationDataResponsePayload)
    {
      SetDiskAnalysisServicesConfigurationDataResponsePayload = setDiskAnalysisServicesConfigurationDataResponsePayload;
    }
    public SetDiskAnalysisServicesConfigurationDataResponsePayload SetDiskAnalysisServicesConfigurationDataResponsePayload { get; set; }
  }
  #endregion SetDiskAnalysisServicesConfigurationDataRequest, SetDiskAnalysisServicesConfigurationDataResponse, and Route for SetDiskAnalysisServicesConfigurationData

  #region GetDiskAnalysisServicesConfigurationDataRequest, GetDiskAnalysisServicesConfigurationDataResponse, and Route for GetDiskAnalysisServicesConfigurationDataRequest
  [Route("/GetDiskAnalysisServicesConfigurationData")]
  [Route("/GetDiskAnalysisServicesConfigurationData/{GetDiskAnalysisServicesConfigurationDataRequestPayload}")]
  public class GetDiskAnalysisServicesConfigurationDataRequest : IReturn<GetDiskAnalysisServicesConfigurationDataResponse>
  {
    public GetDiskAnalysisServicesConfigurationDataRequestPayload GetDiskAnalysisServicesConfigurationDataRequestPayload { get; set; }
  }
  public class GetDiskAnalysisServicesConfigurationDataResponse
  {
    public GetDiskAnalysisServicesConfigurationDataResponse() : this(new GetDiskAnalysisServicesConfigurationDataResponsePayload()) { }
    public GetDiskAnalysisServicesConfigurationDataResponse(GetDiskAnalysisServicesConfigurationDataResponsePayload getDiskAnalysisServicesConfigurationDataResponsePayload)
    {
      GetDiskAnalysisServicesConfigurationDataResponsePayload = getDiskAnalysisServicesConfigurationDataResponsePayload;
    }
    public GetDiskAnalysisServicesConfigurationDataResponsePayload GetDiskAnalysisServicesConfigurationDataResponsePayload { get; set; }
  }
  #endregion GetDiskAnalysisServicesConfigurationDataRequest, GetDiskAnalysisServicesConfigurationDataResponse, and Route for GetDiskAnalysisServicesConfigurationDataRequest

  #region SetDiskAnalysisServicesUserDataRequest and Route for SetDiskAnalysisServicesUserData
  [Route("/SetDiskAnalysisServicesUserData")]
  [Route("/SetDiskAnalysisServicesUserData/{SetDiskAnalysisServicesUserRequestPayload}")]
  public class SetDiskAnalysisServicesUserDataRequest : IReturn<SetDiskAnalysisServicesUserDataResponsePayload>
  {
    public SetDiskAnalysisServicesUserDataRequestPayload SetDiskAnalysisServicesUserDataRequestPayload { get; set; }
  }
  #endregion SetDiskAnalysisServicesUserDataRequest and Route for SetDiskAnalysisServicesUserData
  #region GetDiskAnalysisServicesUserDataRequest and Route for GetDiskAnalysisServicesUserData
  [Route("/GetDiskAnalysisServicesUserData")]
  [Route("/GetDiskAnalysisServicesUserData/{GetDiskAnalysisServicesUserDataRequestPayload}")]
  public class GetDiskAnalysisServicesUserDataRequest : IReturn<GetDiskAnalysisServicesUserDataResponsePayload>
  {
    public GetDiskAnalysisServicesUserDataRequestPayload GetDiskAnalysisServicesUserRequestPayload { get; set; }
  }
  #endregion GetDiskAnalysisServicesUserDataRequest and Route for GetDiskAnalysisServicesUserData

  #region ReadDiskRequest, ReadDiskResponse, and Route for ReadDisk
  [Route("/ReadDisk")]
  [Route("/ReadDisk/{Filters}")]
  public class ReadDiskRequest : IReturn<ReadDiskResponse>
  {
    public ReadDiskRequestPayload ReadDiskRequestPayload { get; set; }
  }
  public class ReadDiskResponse
  {
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



