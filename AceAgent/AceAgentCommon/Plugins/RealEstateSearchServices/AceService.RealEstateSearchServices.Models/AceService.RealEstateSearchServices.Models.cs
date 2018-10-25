using System;
using ServiceStack;
using ServiceStack.Logging;
using ATAP.Utilities.RealEstate.Enumerations;

namespace Ace.AceService.RealEstateSearchServices.Models
{
  [Route("/SubmitGoogleAPIKeyPassPhrase")]
  [Route("/SubmitGoogleAPIKeyPassPhrase/{GoogleAPIKeyPassPhrase}")]
  public class SubmitGoogleAPIKeyPassPhraseRequest : IReturn<SubmitGoogleAPIKeyPassPhraseResponse>
  {
    public string GoogleAPIKeyPassPhrase { get; set; }
  }
  public class SubmitGoogleAPIKeyPassPhraseResponse
  {
    public string Result { get; set; }

  }
  #region RealEstateSearchServicesInitialization
  [Route("/RealEstateSearchServicesInitialization")]
  [Route("/RealEstateSearchServicesInitialization/{RealEstateSearchServicesInitializationRequestParameters}")]
  public class RealEstateSearchServicesInitializationRequest : IReturn<RealEstateSearchServicesInitializationResponse>
  {
    public string RealEstateSearchServicesInitializationRequestParameters { get; set; }
  }
  public class RealEstateSearchServicesInitializationResponse
  {
    public string Result { get; set; }
  }
  #endregion RealEstateSearchServicesInitialization


  #region PropertySearchServices
  [Route("/PropertySearch")]
  [Route("/PropertySearch/{Filters}")]
  public class PropertySearchRequest : IReturn<PropertySearchResponse>
  {
    public string Filters { get; set; }
  }
  public class PropertySearchResponse
  {
    public string[] Result { get; set; }
    public Operation Kind { get; set; }
  }
  #endregion PropertySearchServices

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



