using System;
using ServiceStack;
using ATAP.Utilities.RealEstate.Enumerations;

namespace Ace.Agent.RealEstateServices
{
  #region SetRealEstateServicesConfigurationData
  [Route("/SetRealEstateServicesConfigurationData")]
  public class SetRealEstateServicesConfigurationDataRequest : IReturn<SetRealEstateServicesConfigurationDataResponse>
  {
    public SetRealEstateServicesConfigurationDataRequestData SetRealEstateServicesConfigurationDataRequestData { get; set; }
  }
  #endregion SetRealEstateServicesConfigurationData
  #region GetRealEstateServicesConfigurationData
  [Route("/GetRealEstateServicesConfigurationData")]
  [Route("/GetRealEstateServicesConfigurationData/{GetRealEstateServicesConfigurationDataRequestData}")]
  public class GetRealEstateServicesConfigurationDataRequest : IReturn<GetRealEstateServicesConfigurationDataResponse>
  {
    public GetRealEstateServicesConfigurationDataRequestData GetRealEstateServicesConfigurationDataRequestData { get; set; }
  }
  #endregion GetRealEstateServicesConfigurationData

  #region SetRealEstateServicesUserData
  [Route("/SetRealEstateServicesUserData")]
  [Route("/SetRealEstateServicesUserData/{SetRealEstateServicesUserDataRequestData}")]
  public class SetRealEstateServicesUserDataRequest : IReturn<SetRealEstateServicesUserDataResponse>
  {
    public SetRealEstateServicesUserDataRequestData SetRealEstateServicesUserDataRequestData { get; set; }
  }
  #endregion SetRealEstateServicesUserData
  #region GetRealEstateServicesUserData
  [Route("/GetRealEstateServicesUserData")]
  [Route("/GetRealEstateServicesUserData/{GetRealEstateServicesUserDataRequestData}")]
  public class GetRealEstateServicesUserDataRequest : IReturn<GetRealEstateServicesUserDataResponse>
  {
    public GetRealEstateServicesUserDataRequestData GetRealEstateServicesUserDataRequestData { get; set; }
  }
  #endregion SetRealEstateServicesUserData

  #region RealEstateServicesInitialization
  [Route("/RealEstateServicesInitialization")]
  public class RealEstateServicesInitializationRequest : IReturn<RealEstateServicesInitializationResponse>
  {
    public RealEstateServicesInitializationDataRequestData RealEstateServicesInitializationDataRequestData { get; set; }
  }
  public class RealEstateServicesInitializationResponse
  {
    public RealEstateServicesInitializationResponse() : this( new RealEstateServicesInitializationResponseData()) { }
    public RealEstateServicesInitializationResponse(RealEstateServicesInitializationResponseData realEstateServicesInitializationResponseData) {
      RealEstateServicesInitializationResponseData = realEstateServicesInitializationResponseData;
    }
    public RealEstateServicesInitializationResponseData RealEstateServicesInitializationResponseData { get; set; }
  }
  #endregion RealEstateServicesInitialization

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

  [Route("/MonitorRealEstateServicesDataStructures")]
    public class MonitorRealEstateServicesDataStructuresRequest : IReturn<MonitorRealEstateServicesDataStructuresResponse>
  {
    public string Filters { get; set; }
  }
  public class MonitorRealEstateServicesDataStructuresResponse
  {
    public string[] Result { get; set; }
    public Operation Kind { get; set; }
  }


}



