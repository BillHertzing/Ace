using System;
using ServiceStack;
using ServiceStack.Logging;
using ATAP.Utilities.RealEstate.Enumerations;

namespace Ace.AceService.RealEstateSearchServices.Models
{
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



