using System;
using Ace.AceService.RealEstateSearchServices.Models;
using ServiceStack;
using ServiceStack.Logging;
namespace Ace.AceService.RealEstateSearchServices.Interfaces
{
  public class RealEstateSearchServices : Service
  {
    public static ILog Log = LogManager.GetLogger(typeof(RealEstateSearchServices));

    public object Any(PropertySearchRequest request)
    {
      Log.Debug("starting Any Property request");
      var filters = request.Filters;
      // ToDo: add the code that returns True/False for the route that includes the kind/version
      // Create the HTTP request for a property search, and update it with the filters
      {

      }
      string[] results = new string[1] { "FoundAHome" };
      return new PropertySearchResponse { Result = results };
    }
  }
}
