using System;
using ServiceStack;
using ServiceStack.Logging;
using Swordfish.NET.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using ATAP.Utilities.RealEstate.Enumerations;

namespace Ace.AceCommon.Plugin.RealEstateSearchServices
  
{
  public class RealEstateSearchServices : Service
  {
    public static ILog Log = LogManager.GetLogger(typeof(RealEstateSearchServices));
    
     public object Post(RealEstateSearchServicesInitializationRequest request)
    {
      Log.Debug("starting Post(RealEstateSearchServicesInitializationRequest request)");
      RealEstateSearchServicesInitializationDataRequestData realEstateSearchServicesInitializationDataRequestData = request.RealEstateSearchServicesInitializationDataRequestData;
      //Log.Debug($"You sent me RealEstateSearchServicesInitializationDataRequestData = {realEstateSearchServicesInitializationDataRequestData}");
      //Log.Debug($"You sent me RealEstateSearchServicesInitializationData = {realEstateSearchServicesInitializationDataRequestData.RealEstateSearchServicesInitializationData}");
      // Initialize a machine datastructure for this service/user/session/connection
      // Initialize a user datastructure for this service/user/session/connection

      // populate the ConfigurationData response structures
      string google_API_URI = (RealEstateSearchServicesPluginData.Google_API_URI != null) ? RealEstateSearchServicesPluginData.Google_API_URI : "HTTP://GoogleAPIURINotDefined.com/";
      string homeAway_API_URI_API_URI = (RealEstateSearchServicesPluginData.HomeAway_API_URI != null) ? RealEstateSearchServicesPluginData.HomeAway_API_URI : "HTTP://HomeAwayAPIURINotDefined.com/";
      RealEstateSearchServicesConfigurationData realEstateSearchServicesConfigurationData = new RealEstateSearchServicesConfigurationData(google_API_URI, homeAway_API_URI_API_URI);
      // populate the UserData response structures
      string googleAPIKeyEncrypted = (RealEstateSearchServicesPluginData.GoogleAPIKeyEncrypted != null) ? RealEstateSearchServicesPluginData.GoogleAPIKeyEncrypted : "GoogleAPIKeyEncrypted needed";
      string googleAPIKeyPassPhrase = (RealEstateSearchServicesPluginData.GoogleAPIKeyPassPhrase != null) ? RealEstateSearchServicesPluginData.GoogleAPIKeyPassPhrase : "GoogleAPIKeyPassPhrase needed";
      string homeAwayAPIKeyEncrypted = (RealEstateSearchServicesPluginData.HomeAwayAPIKeyEncrypted != null) ? RealEstateSearchServicesPluginData.HomeAwayAPIKeyEncrypted : "HomeAwayAPIKeyEncrypted needed";
      string homeAwayAPIKeyPassPhrase = (RealEstateSearchServicesPluginData.HomeAwayAPIKeyPassPhrase != null) ? RealEstateSearchServicesPluginData.HomeAwayAPIKeyPassPhrase : "HomeAwayAPIKeyPassPhrase needed";
      RealEstateSearchServicesUserData realEstateSearchServicesUserData = new RealEstateSearchServicesUserData(googleAPIKeyEncrypted, homeAwayAPIKeyEncrypted, googleAPIKeyPassPhrase,  homeAwayAPIKeyPassPhrase);

      // Create and populate the Response data structure
      RealEstateSearchServicesInitializationResponseData realEstateSearchServicesInitializationResponseData = new RealEstateSearchServicesInitializationResponseData(realEstateSearchServicesConfigurationData, realEstateSearchServicesUserData);
      RealEstateSearchServicesInitializationResponse realEstateSearchServicesInitializationResponse = new RealEstateSearchServicesInitializationResponse(realEstateSearchServicesInitializationResponseData);
      // return information about this service/user/session
      Log.Debug($"leaving Post(RealEstateSearchServicesInitializationRequest request), returning {realEstateSearchServicesInitializationResponse}");
      return realEstateSearchServicesInitializationResponse;
    }

    public object Post(SetRealEstateSearchServicesConfigurationDataRequest request)
    {
      Log.Debug("starting Post(SetRealEstateSearchServicesConfigurationDataRequest request)");
      SetRealEstateSearchServicesConfigurationDataRequestData setRealEstateSearchServicesConfigurationDataRequestData = request.SetRealEstateSearchServicesConfigurationDataRequestData;
      RealEstateSearchServicesConfigurationData realEstateSearchServicesConfigurationData = setRealEstateSearchServicesConfigurationDataRequestData.RealEstateSearchServicesConfigurationData;
      Log.Debug($"You sent me RealEstateSearchServicesConfigurationData = {realEstateSearchServicesConfigurationData}");
      RealEstateSearchServicesPluginData.Google_API_URI = realEstateSearchServicesConfigurationData.Google_API_URI;
      RealEstateSearchServicesPluginData.HomeAway_API_URI = realEstateSearchServicesConfigurationData.HomeAway_API_URI;
      // return information about this service/user/session
      string result = "OK";
      Log.Debug($"leaving Any(SetRealEstateSearchServicesConfigurationDataRequest request), returning {result}");
      return new SetRealEstateSearchServicesConfigurationDataResponse { Result = result };
    }
    public object Post(SetRealEstateSearchServicesUserDataRequest request)
    {
      Log.Debug("starting Post(SetRealEstateSearchServicesUserDataRequest request)");
      SetRealEstateSearchServicesUserDataRequestData setRealEstateSearchServicesUserDataRequestData = request.SetRealEstateSearchServicesUserDataRequestData;
      RealEstateSearchServicesUserData realEstateSearchServicesUserData = setRealEstateSearchServicesUserDataRequestData.RealEstateSearchServicesUserData;
      Log.Debug($"You sent me RealEstateSearchServicesUserData = {realEstateSearchServicesUserData}");
      RealEstateSearchServicesPluginData.GoogleAPIKeyEncrypted = realEstateSearchServicesUserData.GoogleAPIKeyEncrypted;
      RealEstateSearchServicesPluginData.GoogleAPIKeyPassPhrase = realEstateSearchServicesUserData.GoogleAPIKeyPassPhrase;
      RealEstateSearchServicesPluginData.HomeAwayAPIKeyEncrypted = realEstateSearchServicesUserData.HomeAwayAPIKeyEncrypted;
      RealEstateSearchServicesPluginData.HomeAwayAPIKeyPassPhrase = realEstateSearchServicesUserData.HomeAwayAPIKeyPassPhrase;
      // return information about this service/user/session
      string result = "OK";
      Log.Debug($"leaving Post(SetRealEstateSearchServicesUserDataRequest request), returning {result}");
      return new SetRealEstateSearchServicesConfigurationDataResponse { Result = result };
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
