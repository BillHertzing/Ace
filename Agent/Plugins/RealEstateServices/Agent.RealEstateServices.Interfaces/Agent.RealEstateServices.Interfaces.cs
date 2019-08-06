using System;
using ServiceStack;
using ServiceStack.Logging;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using ATAP.Utilities.RealEstate.Enumerations;
using System.Collections.Generic;
using System.Threading;

namespace Ace.Plugin.RealEstateServices {
    public class RealEstateServices : Service {
        public static ILog Log = LogManager.GetLogger(typeof(RealEstateServices));

        public object Post(InitializationRequest request) {
            InitializationRequestPayload initializationRequestPayload = request.InitializationRequestPayload;
            Log.Debug($"You sent me InitializationRequestPayload = {initializationRequestPayload}");

            // Initialize a machine datastructure for this service/user/session/connection
            // Initialize a user datastructure for this service/user/session/connection

            //ToDo: the ConfigurationData structure need to be initialized in the plug startup code, and stored in teh Data instance, and this routine just copies those values

            // populate the ConfigurationData response structures
            string google_API_URI = (RealEstateServicesData.Google_API_URI!=null) ? RealEstateServicesData.Google_API_URI : "HTTP://GoogleAPIURINotDefined.com/";
            string homeAway_API_URI_API_URI = (RealEstateServicesData.HomeAway_API_URI!=null) ? RealEstateServicesData.HomeAway_API_URI : "HTTP://HomeAwayAPIURINotDefined.com/";
            ConfigurationData configurationData = new ConfigurationData(google_API_URI, homeAway_API_URI_API_URI);
            // populate the UserData response structures
            string googleAPIKeyEncrypted = (RealEstateServicesData.GoogleAPIKeyEncrypted!=null) ? RealEstateServicesData.GoogleAPIKeyEncrypted : "GoogleAPIKeyEncrypted needed";
            string googleAPIKeyPassPhrase = (RealEstateServicesData.GoogleAPIKeyPassPhrase!=null) ? RealEstateServicesData.GoogleAPIKeyPassPhrase : "GoogleAPIKeyPassPhrase needed";
            string homeAwayAPIKeyEncrypted = (RealEstateServicesData.HomeAwayAPIKeyEncrypted!=null) ? RealEstateServicesData.HomeAwayAPIKeyEncrypted : "HomeAwayAPIKeyEncrypted needed";
            string homeAwayAPIKeyPassPhrase = (RealEstateServicesData.HomeAwayAPIKeyPassPhrase!=null) ? RealEstateServicesData.HomeAwayAPIKeyPassPhrase : "HomeAwayAPIKeyPassPhrase needed";
            UserData userData = new UserData(googleAPIKeyEncrypted, homeAwayAPIKeyEncrypted, googleAPIKeyPassPhrase, homeAwayAPIKeyPassPhrase);

            // Create and populate the InitializationResponse data structure
            InitializationResponse initializationResponse = new InitializationResponse(new InitializationResponsePayload(configurationData, userData));
            // return information about this service/user/session
            return initializationResponse;
        }

        public object Post(SetConfigurationDataRequest request) {
            Log.Debug($"You sent me ConfigurationData = {request.SetConfigurationDataRequestPayload.ConfigurationData}");
            // ToDo: switch on the boolean
            // ToDo: These should be part of a ConfigurationData instance in plugin's Data instance
            RealEstateServicesData.Google_API_URI=request.SetConfigurationDataRequestPayload.ConfigurationData.Google_API_URI;
            RealEstateServicesData.HomeAway_API_URI=request.SetConfigurationDataRequestPayload.ConfigurationData.HomeAway_API_URI;
            SetConfigurationDataResponsePayload setConfigurationDataResponsePayload = new SetConfigurationDataResponsePayload("Ok");
            return new SetConfigurationDataResponse(setConfigurationDataResponsePayload);
        }
        public object Post(SetUserDataRequest request) {
            Log.Debug($"You sent me UserData = {request.SetUserDataRequestPayload.UserData}");
            // ToDo: switch on the boolean
            // ToDo: These should be part of a UserData instance in plugin's Data instance
            RealEstateServicesData.GoogleAPIKeyEncrypted=request.SetUserDataRequestPayload.UserData.GoogleAPIKeyEncrypted;
            RealEstateServicesData.GoogleAPIKeyPassPhrase=request.SetUserDataRequestPayload.UserData.GoogleAPIKeyPassPhrase;
            RealEstateServicesData.HomeAwayAPIKeyEncrypted=request.SetUserDataRequestPayload.UserData.HomeAwayAPIKeyEncrypted;
            RealEstateServicesData.HomeAwayAPIKeyPassPhrase=request.SetUserDataRequestPayload.UserData.HomeAwayAPIKeyPassPhrase;
            SetUserDataResponsePayload setUserDataResponsePayload = new SetUserDataResponsePayload("Ok");
            return new SetUserDataResponse(setUserDataResponsePayload);
        }
        public object Post(PropertySearchRequest request) {
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
            try {
                Log.Debug("Calling through gateway {gateway}, using Registry Key {RegistryKey}");

            }
            catch {
                Log.Debug("Caught an exception");
            }
            finally {
            }


            // update the Plugin Data Structure with the data from the response
            //RealEstateServicesData.PluginRootCOD.Add("test1", 100);
            List<ListingSearchHit> listingSearchHits = new List<ListingSearchHit>();
            PropertySearchResponseData propertySearchResponseData = new PropertySearchResponseData(listingSearchHits);
            PropertySearchResponsePayload propertySearchResponsePayload = new PropertySearchResponsePayload(propertySearchResponseData);
            PropertySearchResponse propertySearchResponse = new PropertySearchResponse();
            propertySearchResponse.PropertySearchResponsePayload=propertySearchResponsePayload;
            return propertySearchResponse;
        }
        public RealEstateServicesData RealEstateServicesData { get; set; }
    }


}
