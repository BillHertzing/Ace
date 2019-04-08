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
    public DiskAnalysisServicesInitializationResponse() : this(new DiskAnalysisServicesInitializationResponseData()) { }
    public DiskAnalysisServicesInitializationResponse(DiskAnalysisServicesInitializationResponseData diskAnalysisServicesInitializationResponseData)
    {
      DiskAnalysisServicesInitializationResponseData = diskAnalysisServicesInitializationResponseData;
    }
    public DiskAnalysisServicesInitializationResponseData DiskAnalysisServicesInitializationResponseData { get; set; }
  }
  #endregion DiskAnalysisServicesInitialization

  #region SetDiskAnalysisServicesConfigurationData
  [Route("/SetDiskAnalysisServicesConfigurationData")]
  public class SetDiskAnalysisServicesConfigurationDataRequest : IReturn<SetDiskAnalysisServicesConfigurationDataResponse>
  {
    public SetDiskAnalysisServicesConfigurationDataRequestData SetDiskAnalysisServicesConfigurationDataRequestData { get; set; }
  }
  #endregion SetDiskAnalysisServicesConfigurationData
  #region GetDiskAnalysisServicesConfigurationData
  [Route("/GetDiskAnalysisServicesConfigurationData")]
  [Route("/GetDiskAnalysisServicesConfigurationData/{GetDiskAnalysisServicesConfigurationDataRequestData}")]
  public class GetDiskAnalysisServicesConfigurationDataRequest : IReturn<GetDiskAnalysisServicesConfigurationDataResponse>
  {
    public GetDiskAnalysisServicesConfigurationDataRequestData GetDiskAnalysisServicesConfigurationDataRequestData { get; set; }
  }
  #endregion GetDiskAnalysisServicesConfigurationData

  #region SetDiskAnalysisServicesUserData
  [Route("/SetDiskAnalysisServicesUserData")]
  [Route("/SetDiskAnalysisServicesUserData/{SetDiskAnalysisServicesUserDataRequestData}")]
  public class SetDiskAnalysisServicesUserDataRequest : IReturn<SetDiskAnalysisServicesUserDataResponse>
  {
    public SetDiskAnalysisServicesUserDataRequestData SetDiskAnalysisServicesUserDataRequestData { get; set; }
  }
  #endregion SetDiskAnalysisServicesUserData
  #region GetDiskAnalysisServicesUserData
  [Route("/GetDiskAnalysisServicesUserData")]
  [Route("/GetDiskAnalysisServicesUserData/{GetDiskAnalysisServicesUserDataRequestData}")]
  public class GetDiskAnalysisServicesUserDataRequest : IReturn<GetDiskAnalysisServicesUserDataResponse>
  {
    public GetDiskAnalysisServicesUserDataRequestData GetDiskAnalysisServicesUserDataRequestData { get; set; }
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



