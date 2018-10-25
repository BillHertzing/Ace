using System;
using Ace.AceService.RealEstateSearchServices.Models;
using Ace.AceService.RealEstateSearchServices.PluginData;
using ServiceStack;
using ServiceStack.Logging;
using Swordfish.NET.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using ATAP.Utilities.RealEstate.Enumerations;

namespace Ace.AceService.RealEstateSearchServices.Interfaces
  
{
  public class RealEstateSearchServices : Service
  {
    public static ILog Log = LogManager.GetLogger(typeof(RealEstateSearchServices));
    //RealEstateSearchServicesPluginData realEstateSearchServicesPluginData;
    /*
    // Override the constructor, and get this Plugin's data structure, from the DI container
    override   RealEstateSearchServices : base () {
      //var appHost =
      // var container = appHost.Container;
      // Add Try
      //realEstateSearchServicesPluginData = container.TryResolve(typeof(RealEstateSearchServicesPluginData)) as RealEstateSearchServicesPluginData;
      }
      */
    public object Any(RealEstateSearchServicesInitializationRequest request)
    {
      Log.Debug("starting Any (RealEstateSearchServicesInitializationRequest request)");
      string realEstateSearchServicesInitializationRequestParameters = request.RealEstateSearchServicesInitializationRequestParameters;
      // Initialize a machine datastructure for this service/user/session/connection
      // Initialize a user datastructure for this service/user/session/connection
      {

      }
      // return information about this service/user/session
      string result =  $"You sent me RealEstateSearchServicesInitializationRequestParameters = {realEstateSearchServicesInitializationRequestParameters}";
      Log.Debug("leaving Any(RealEstateSearchServicesInitializationRequest request), returning {result}");
      return new RealEstateSearchServicesInitializationResponse { Result = result };
    }

    public object Any(SubmitGoogleAPIKeyPassPhraseRequest request)
    {
      Log.Debug("starting Any (SubmitGoogleAPIKeyPassPhraseRequest request)");
      string googleAPIKeyPassPhrase = request.GoogleAPIKeyPassPhrase;
      // Initialize a machine datastructure for this service/user/session/connection
      // Initialize a user datastructure for this service/user/session/connection
      {

      }
      // return information about this service/user/session
      string result = $"You sent me GoogleAPIKeyPassPhrase = {googleAPIKeyPassPhrase}";
      Log.Debug($"leaving Any(SubmitGoogleAPIKeyPassPhraseRequest request), returning {result}");
      return new SubmitGoogleAPIKeyPassPhraseResponse { Result = result };
    }
    public object Any(PropertySearchRequest request)
    {
      Log.Debug("starting Any(PropertySearchRequest request)");
      string filters = request.Filters;
      // Create the HTTP request for a property search, and update it with the filters
      {

      }
      // update the Plugin Data Structure
      RealEstateSearchServicesPluginData.PluginRootCOD.Add("test1", 100);
      string[] results = new string[2] { $"You sent me filter = {filters}", "Line2Complete" };
      //string results = $"You sent me filter = {filters}";
      Operation kind = Operation.PropertySearch;
      Log.Debug("leaving Any(PropertySearchRequest request)");
      return new PropertySearchResponse { Result = results, Kind = kind };
      //return new PropertySearchResponse { Result = results};
    }
    public RealEstateSearchServicesPluginData RealEstateSearchServicesPluginData { get; set; }
  }
}
