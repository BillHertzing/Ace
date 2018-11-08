using System;
using ServiceStack;
using ServiceStack.Logging;
using Swordfish.NET.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using ATAP.Utilities.RealEstate.Enumerations;
using System.Collections.Generic;

namespace Ace.Agent.RealEstateServices

{
  public class RealEstateServices : Service
  {
    public static ILog Log = LogManager.GetLogger(typeof(RealEstateServices));
    
     public object Post(RealEstateServicesInitializationRequest request)
    {
      Log.Debug("starting Post(RealEstateServicesInitializationRequest request)");
      RealEstateServicesInitializationDataRequestData RealEstateServicesInitializationDataRequestData = request.RealEstateServicesInitializationDataRequestData;
      //Log.Debug($"You sent me RealEstateServicesInitializationDataRequestData = {RealEstateServicesInitializationDataRequestData}");
      //Log.Debug($"You sent me RealEstateServicesInitializationData = {RealEstateServicesInitializationDataRequestData.RealEstateServicesInitializationData}");
      // Initialize a machine datastructure for this service/user/session/connection
      // Initialize a user datastructure for this service/user/session/connection

      // populate the ConfigurationData response structures
      string google_API_URI = (RealEstateServicesData.Google_API_URI != null) ? RealEstateServicesData.Google_API_URI : "HTTP://GoogleAPIURINotDefined.com/";
      string homeAway_API_URI_API_URI = (RealEstateServicesData.HomeAway_API_URI != null) ? RealEstateServicesData.HomeAway_API_URI : "HTTP://HomeAwayAPIURINotDefined.com/";
      RealEstateServicesConfigurationData RealEstateServicesConfigurationData = new RealEstateServicesConfigurationData(google_API_URI, homeAway_API_URI_API_URI);
      // populate the UserData response structures
      string googleAPIKeyEncrypted = (RealEstateServicesData.GoogleAPIKeyEncrypted != null) ? RealEstateServicesData.GoogleAPIKeyEncrypted : "GoogleAPIKeyEncrypted needed";
      string googleAPIKeyPassPhrase = (RealEstateServicesData.GoogleAPIKeyPassPhrase != null) ? RealEstateServicesData.GoogleAPIKeyPassPhrase : "GoogleAPIKeyPassPhrase needed";
      string homeAwayAPIKeyEncrypted = (RealEstateServicesData.HomeAwayAPIKeyEncrypted != null) ? RealEstateServicesData.HomeAwayAPIKeyEncrypted : "HomeAwayAPIKeyEncrypted needed";
      string homeAwayAPIKeyPassPhrase = (RealEstateServicesData.HomeAwayAPIKeyPassPhrase != null) ? RealEstateServicesData.HomeAwayAPIKeyPassPhrase : "HomeAwayAPIKeyPassPhrase needed";
      RealEstateServicesUserData RealEstateServicesUserData = new RealEstateServicesUserData(googleAPIKeyEncrypted, homeAwayAPIKeyEncrypted, googleAPIKeyPassPhrase,  homeAwayAPIKeyPassPhrase);

      // Create and populate the Response data structure
      RealEstateServicesInitializationResponseData RealEstateServicesInitializationResponseData = new RealEstateServicesInitializationResponseData(RealEstateServicesConfigurationData, RealEstateServicesUserData);
      RealEstateServicesInitializationResponse RealEstateServicesInitializationResponse = new RealEstateServicesInitializationResponse(RealEstateServicesInitializationResponseData);
      // return information about this service/user/session
      Log.Debug($"leaving Post(RealEstateServicesInitializationRequest request), returning {RealEstateServicesInitializationResponse}");
      return RealEstateServicesInitializationResponse;
    }

    public object Post(SetRealEstateServicesConfigurationDataRequest request)
    {
      Log.Debug("starting Post(SetRealEstateServicesConfigurationDataRequest request)");
      SetRealEstateServicesConfigurationDataRequestData setRealEstateServicesConfigurationDataRequestData = request.SetRealEstateServicesConfigurationDataRequestData;
      RealEstateServicesConfigurationData RealEstateServicesConfigurationData = setRealEstateServicesConfigurationDataRequestData.RealEstateServicesConfigurationData;
      Log.Debug($"You sent me RealEstateServicesConfigurationData = {RealEstateServicesConfigurationData}");
      RealEstateServicesData.Google_API_URI = RealEstateServicesConfigurationData.Google_API_URI;
      RealEstateServicesData.HomeAway_API_URI = RealEstateServicesConfigurationData.HomeAway_API_URI;
      // return information about this service/user/session
      string result = "OK";
      Log.Debug($"leaving Any(SetRealEstateServicesConfigurationDataRequest request), returning {result}");
      return new SetRealEstateServicesConfigurationDataResponse { Result = result };
    }
    public object Post(SetRealEstateServicesUserDataRequest request)
    {
      Log.Debug("starting Post(SetRealEstateServicesUserDataRequest request)");
      SetRealEstateServicesUserDataRequestData setRealEstateServicesUserDataRequestData = request.SetRealEstateServicesUserDataRequestData;
      RealEstateServicesUserData RealEstateServicesUserData = setRealEstateServicesUserDataRequestData.RealEstateServicesUserData;
      Log.Debug($"You sent me RealEstateServicesUserData = {RealEstateServicesUserData}");
      RealEstateServicesData.GoogleAPIKeyEncrypted = RealEstateServicesUserData.GoogleAPIKeyEncrypted;
      RealEstateServicesData.GoogleAPIKeyPassPhrase = RealEstateServicesUserData.GoogleAPIKeyPassPhrase;
      RealEstateServicesData.HomeAwayAPIKeyEncrypted = RealEstateServicesUserData.HomeAwayAPIKeyEncrypted;
      RealEstateServicesData.HomeAwayAPIKeyPassPhrase = RealEstateServicesUserData.HomeAwayAPIKeyPassPhrase;
      // return information about this service/user/session
      string result = "OK";
      Log.Debug($"leaving Post(SetRealEstateServicesUserDataRequest request), returning {result}");
      return new SetRealEstateServicesConfigurationDataResponse { Result = result };
    }
     public object Post(PropertySearchRequest request)
    {
      Log.Debug("starting Post(PropertySearchRequest)");
      PropertySearchRequestPayload propertySearchRequestPayload = request.PropertySearchRequestPayload;
      PropertySearchRequestData propertySearchRequestData = propertySearchRequestPayload.PropertySearchRequestData;
      bool savePropertySearchData = propertySearchRequestPayload.SavePropertySearchData;
      SearchParameters searchParameters = propertySearchRequestData.SearchParameters;
      ListingParameters listingParameters = propertySearchRequestData.ListingParameters;
      // Get the name of the gateway to use
      var gatewayName = "HomeAway";
      // Get the HTTP request for a property search from the gateway
      // update it with the searchParameters
      // update it with the listingParameters
      // Get the type of Response to expect back
      // async call the gateway URI for property search, using the predefined policy and retry scheme
      // ToDo: figure out how to integrate a CancellationToken
      try
      {
        Log.Debug("Calling through gateway {gateway}, using Registry Key {RegistryKey}");

      }
      catch {
        Log.Debug("leaving Post(PropertySearchRequest)");
      }
      finally
      {
      }


      // update the Plugin Data Structure with the data from the response
      //RealEstateServicesData.PluginRootCOD.Add("test1", 100);
      Log.Debug("leaving Post(PropertySearchRequest)");
      List<ListingSearchHit> listingSearchHits = new List<ListingSearchHit>();
      PropertySearchResponseData propertySearchResponseData = new PropertySearchResponseData(listingSearchHits);
      PropertySearchResponsePayload propertySearchResponsePayload = new PropertySearchResponsePayload(propertySearchResponseData);
      PropertySearchResponse propertySearchResponse = new PropertySearchResponse();
      propertySearchResponse.PropertySearchResponsePayload = propertySearchResponsePayload;
      Log.Debug("Leaving Post(PropertySearchRequest)");
      return propertySearchResponse;
    }
    public RealEstateServicesData RealEstateServicesData { get; set; }
  }


}
