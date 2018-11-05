using System;
using ServiceStack;
using ServiceStack.Logging;
using Swordfish.NET.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using ATAP.Utilities.RealEstate.Enumerations;
using System.Collections.Generic;

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
      //RealEstateSearchServicesPluginData.PluginRootCOD.Add("test1", 100);
      Log.Debug("leaving Post(PropertySearchRequest)");
      List<ListingSearchHit> listingSearchHits = new List<ListingSearchHit>();
      PropertySearchResponseData propertySearchResponseData = new PropertySearchResponseData(listingSearchHits);
      PropertySearchResponsePayload propertySearchResponsePayload = new PropertySearchResponsePayload(propertySearchResponseData);
      PropertySearchResponse propertySearchResponse = new PropertySearchResponse();
      propertySearchResponse.PropertySearchResponsePayload = propertySearchResponsePayload;
      Log.Debug("Leaving Post(PropertySearchRequest)");
      return propertySearchResponse;
    }
    public RealEstateSearchServicesPluginData RealEstateSearchServicesPluginData { get; set; }
  }

  public class HomeAwayGateway
  {
    public const string HomeAwayGatewayApiBaseUrl = "https://api.github.com/";

    public T GetJson<T>(string route, params object[] routeArgs)
    {
      //ToDo try/catch the many possible error returns
      return HomeAwayGatewayApiBaseUrl.AppendPath(route.Fmt(routeArgs))
          .GetJsonFromUrl(req => req.UserAgent = "ATAP AceCommander")
          .FromJson<T>();
    }

    public ListingSearchPaginator PublicSearch(SearchParameters searchParameters)
    {
      return GetJson<ListingSearchPaginator>("public/search", searchParameters);
    }

    public ListingAd PublicListing(ListingParameters listingParameters)
    {
      return GetJson<ListingAd>("public/listing", listingParameters);
    }
    /*
    public List<ListingAd> PublicSearchAndListing(SearchParameters searchParameters, ListingParameters listingParameters)
    {
      var map = new Dictionary<int, int>();
      PublicSearch(searchParameters).ForEach(x => map[x.Id] = x);
      PublicListing(listingParameters).ForEach(p =>
          GetOrgRepos(org.Login)
              .ForEach(repo => map[repo.Id] = repo));

      return map.Values.ToList();
    }
    */
    public class ListingAd
    {
      public ListingAd() { }
      public ListingAd(string listingId, ListingLocation listingLocation, string listingUrl)
      {
        ListingId = listingId;
        ListingLocation = listingLocation;
        ListingUrl = listingUrl;
      }
      string ListingId { get; set; }
      ListingLocation ListingLocation { get; set; }
      string ListingUrl { get; set; }
    }
    
    public class ListingSearchPaginator
    {
      public ListingSearchPaginator() { }
      public ListingSearchPaginator(List<ListingSearchHit> entries)
      {
        Entries = entries;
      }
      List<ListingSearchHit> Entries { get; set; }
    }
  }    
}
