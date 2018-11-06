using System;
using ServiceStack;
using ServiceStack.Logging;
using ATAP.Utilities.RealEstate.Enumerations;

namespace Ace.Agent.RealEstateServices
{
  #region SetRealEstateSearchServicesConfigurationData
  [Route("/SetRealEstateSearchServicesConfigurationData")]
  public class SetRealEstateSearchServicesConfigurationDataRequest : IReturn<SetRealEstateSearchServicesConfigurationDataResponse>
  {
    public SetRealEstateSearchServicesConfigurationDataRequestData SetRealEstateSearchServicesConfigurationDataRequestData { get; set; }
  }
  #endregion SetRealEstateSearchServicesConfigurationData
  #region GetRealEstateSearchServicesConfigurationData
  [Route("/GetRealEstateSearchServicesConfigurationData")]
  [Route("/GetRealEstateSearchServicesConfigurationData/{GetRealEstateSearchServicesConfigurationDataRequestData}")]
  public class GetRealEstateSearchServicesConfigurationDataRequest : IReturn<GetRealEstateSearchServicesConfigurationDataResponse>
  {
    public GetRealEstateSearchServicesConfigurationDataRequestData GetRealEstateSearchServicesConfigurationDataRequestData { get; set; }
  }
  #endregion GetRealEstateSearchServicesConfigurationData

  #region SetRealEstateSearchServicesUserData
  [Route("/SetRealEstateSearchServicesUserData")]
  [Route("/SetRealEstateSearchServicesUserData/{SetRealEstateSearchServicesUserDataRequestData}")]
  public class SetRealEstateSearchServicesUserDataRequest : IReturn<SetRealEstateSearchServicesUserDataResponse>
  {
    public SetRealEstateSearchServicesUserDataRequestData SetRealEstateSearchServicesUserDataRequestData { get; set; }
  }
  #endregion SetRealEstateSearchServicesUserData
  #region GetRealEstateSearchServicesUserData
  [Route("/GetRealEstateSearchServicesUserData")]
  [Route("/GetRealEstateSearchServicesUserData/{GetRealEstateSearchServicesUserDataRequestData}")]
  public class GetRealEstateSearchServicesUserDataRequest : IReturn<GetRealEstateSearchServicesUserDataResponse>
  {
    public GetRealEstateSearchServicesUserDataRequestData GetRealEstateSearchServicesUserDataRequestData { get; set; }
  }
  #endregion SetRealEstateSearchServicesUserData

  #region RealEstateSearchServicesInitialization
  [Route("/RealEstateSearchServicesInitialization")]
  public class RealEstateSearchServicesInitializationRequest : IReturn<RealEstateSearchServicesInitializationResponse>
  {
    public RealEstateSearchServicesInitializationDataRequestData RealEstateSearchServicesInitializationDataRequestData { get; set; }
  }
  public class RealEstateSearchServicesInitializationResponse
  {
    public RealEstateSearchServicesInitializationResponse() : this( new RealEstateSearchServicesInitializationResponseData()) { }
    public RealEstateSearchServicesInitializationResponse(RealEstateSearchServicesInitializationResponseData realEstateSearchServicesInitializationResponseData) {
      RealEstateSearchServicesInitializationResponseData = realEstateSearchServicesInitializationResponseData;
    }
    public RealEstateSearchServicesInitializationResponseData RealEstateSearchServicesInitializationResponseData { get; set; }
  }
  #endregion RealEstateSearchServicesInitialization

  #region PropertySearch
  [Route("/PropertySearch")]
  [Route("/PropertySearch/{Filters}")]
  public class PropertySearchRequest : IReturn<PropertySearchResponse>
  {
    public PropertySearchRequestPayload PropertySearchRequestPayload { get; set; }
  }
  public class PropertySearchResponse
  {
    public PropertySearchResponsePayload PropertySearchResponsePayload { get; set; }
  }
  #endregion PropertySearch

  [Route("/MonitorRealEstateSearchServicesDataStructures")]
    public class MonitorRealEstateSearchServicesDataStructuresRequest : IReturn<MonitorRealEstateSearchServicesDataStructuresResponse>
  {
    public string Filters { get; set; }
  }
  public class MonitorRealEstateSearchServicesDataStructuresResponse
  {
    public string[] Result { get; set; }
    public Operation Kind { get; set; }
  }


}



