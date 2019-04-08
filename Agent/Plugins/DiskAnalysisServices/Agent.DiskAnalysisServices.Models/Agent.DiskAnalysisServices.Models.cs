using System;
using ServiceStack;

namespace Ace.Agent.DiskAnalysisServices
{
  #region DiskAnalysisServicesInitialization
  [Route("/DiskAnalysisServicesInitialization")]
  public class DiskAnalysisServicesInitializationRequest : IReturn<DiskAnalysisServicesInitializationResponse>
  {
    public DiskAnalysisServicesInitializationDataRequestData DiskAnalysisServicesInitializationDataRequestData { get; set; }
  }
  public class DiskAnalysisServicesInitializationResponse
  {
    public DiskAnalysisServicesInitializationResponse() : this(new DiskAnalysisServicesInitializationResponsePayload()) { }
    public DiskAnalysisServicesInitializationResponse(DiskAnalysisServicesInitializationResponsePayload diskAnalysisServicesInitializationResponseData)
    {
      DiskAnalysisServicesInitializationResponseData = diskAnalysisServicesInitializationResponseData;
    }
    public DiskAnalysisServicesInitializationResponsePayload DiskAnalysisServicesInitializationResponseData { get; set; }
  }
  #endregion DiskAnalysisServicesInitialization

  #region SetDiskAnalysisServicesConfigurationData
  [Route("/SetDiskAnalysisServicesConfigurationData")]
  public class SetDiskAnalysisServicesConfigurationDataRequest : IReturn<SetDiskAnalysisServicesConfigurationDataResponsePayload>
  {
    public SetDiskAnalysisServicesConfigurationDataRequestPayload SetDiskAnalysisServicesConfigurationDataRequestData { get; set; }
  }
  #endregion SetDiskAnalysisServicesConfigurationData
  #region GetDiskAnalysisServicesConfigurationData
  [Route("/GetDiskAnalysisServicesConfigurationData")]
  [Route("/GetDiskAnalysisServicesConfigurationData/{GetDiskAnalysisServicesConfigurationDataRequestPayload}")]
  public class GetDiskAnalysisServicesConfigurationDataRequest : IReturn<GetDiskAnalysisServicesConfigurationDataResponsePayload>
  {
    public GetDiskAnalysisServicesConfigurationDataRequestPayload GetDiskAnalysisServicesConfigurationDataRequestData { get; set; }
  }
  #endregion GetDiskAnalysisServicesConfigurationData

  #region SetDiskAnalysisServicesUserData
  [Route("/SetDiskAnalysisServicesUserData")]
  [Route("/SetDiskAnalysisServicesUserData/{SetDiskAnalysisServicesUserRequestPayload}")]
  public class SetDiskAnalysisServicesUserDataRequest : IReturn<SetDiskAnalysisServicesUserDataResponsePayload>
  {
    public SetDiskAnalysisServicesUserDataRequestPayload SetDiskAnalysisServicesUserDataRequestData { get; set; }
  }
  #endregion SetDiskAnalysisServicesUserData
  #region GetDiskAnalysisServicesUserData
  [Route("/GetDiskAnalysisServicesUserData")]
  [Route("/GetDiskAnalysisServicesUserData/{GetDiskAnalysisServicesUserDataRequestPayload}")]
  public class GetDiskAnalysisServicesUserDataRequest : IReturn<GetDiskAnalysisServicesUserDataResponsePayload>
  {
    public GetDiskAnalysisServicesUserDataRequestPayload GetDiskAnalysisServicesUserDataRequestData { get; set; }
  }
  #endregion SetDiskAnalysisServicesUserData

  #region ReadDisk
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
  #endregion ReadDisk

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



