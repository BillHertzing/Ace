using System;
using ServiceStack;
using ServiceStack.Logging;
namespace Ace.AceService.RealEstateSearchServices.Models
{
  [Route("/PropertySearch")]
  public class PropertySearchRequest : IReturn<PropertySearchResponse>
  {
    public string Filters { get; set; }
  }
  public class PropertySearchResponse
  {
    public string[] Result { get; set; }
  }
}
