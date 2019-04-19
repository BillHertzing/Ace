// Required for the HttpClient
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ace.Agent.RealEstateServices;
// Required for the logger/logging
//using Blazor.Extensions.Logging;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Ace.AceGUI.Pages {
    public class RealEstateServicesCodeBehind : ComponentBase {
        public const string labelForConfigurationDataSave = "save the Configuration data?";
        public const string labelForGoogle_API_URI = "Enter Google API URI string";
        // Eventually replace with localization
        public const string labelForGoogleAPIKey = "Enter Google APIKey";
        public const string labelForGoogleAPIKeyPassPhrase = "Enter Google passphrase to decrypt Google APIKey stored in iCacheProvider";
        public const string labelForHomeAway_API_URI = "Enter HomeAway API Uri string";
        public const string labelForHomeAwayAPIKey = "Enter HomeAway APIKey";
        public const string labelForHomeAwayAPIKeyPassPhrase = "Enter HomeAway passphrase to decrypt HomeAway APIKey stored in iCacheProvider";
        public const string labelForMaxBathrooms = "Maximum number of Bathrooms";
        public const string labelForMaxBedrooms = "Maximum number of Bedrooms";

        public const string labelForMinBathrooms = "Minumum number of Bathrooms";
        public const string labelForMinBedrooms = "Minumum number of Bedrooms";
        public const string labelForAvailability = "Availability";
        public const string labelForSavingGoogleAPIKey = "save the Google APIKey for this application?";
        public const string labelForUserDataSave = "save the User data?";
        public const string MaxBathroomsPlaceHolder = "4";
        public const string MaxBedroomsPlaceHolder = "4";
        public const string MinBathroomsPlaceHolder = "2";
        public const string MinBedroomsPlaceHolder = "3";
        public const string placeHolderForGoogle_API_URI = "GoogleAPI Uri string";
        public const string placeHolderForGoogleAPIKey = "GoogleAPI Key";
        public const string placeHolderForGoogleAPIKeyPassPhrase = "GoogleAPI Key Passphrase";
        public const string placeHolderForHomeAway_API_URI = "HomeAwayAPI Uri string";
        public const string placeHolderForHomeAwayAPIKey = "HomeAwayAPI Key";
        public const string placeHolderForHomeAwayAPIKeyPassPhrase = "HomeAwayAPI Key Passphrase";


        // This syntax adds to the class a Method that accesses the DI container, and retrieves the instance having the specified type from the DI container. In this case, we are accessing a builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type. This method call returns that object from the DI container  
        [Inject]
        HttpClient HttpClient {
            get;
            set;
        }

        protected override async Task OnInitAsync() {
            //Logger.LogDebug($"Starting OnInitAsync");

            var initializationRequestPayload = new InitializationRequestPayload();
            var initializationRequest = new InitializationRequest(initializationRequestPayload);
            //Logger.LogDebug($"Calling PostJsonAsync<InitializationResponse> with InitializationRequest ={initializationRequest}");
            InitializationResponse=await HttpClient.PostJsonAsync<InitializationResponse>("RealEstateServicesInitialization", initializationRequest);
            //Logger.LogDebug($"Returned from GetJsonAsync<InitializationResponse>, initializationResponse = {initializationResponse}");
            ConfigurationData=InitializationResponse.InitializationResponsePayload.ConfigurationData;
            UserData=InitializationResponse.InitializationResponsePayload.UserData;
            // ToDo: Design better security for the API keys on the client (e.g, keep only tokens on the client, or, make the user enter the API keys on the client,a nd only keep them there (per user?)
            GoogleAPIKey=UserData.GoogleAPIKeyEncrypted;
            //GoogleAPIKeyPassPhrase=UserData.GoogleAPIKeyPassPhrase;
            HomeAwayAPIKey=UserData.HomeAwayAPIKeyEncrypted;
            //HomeAwayAPIKeyPassPhrase=UserData.HomeAwayAPIKeyPassPhrase;

            //Logger.LogDebug($"Leaving OnInitAsync");
        }


        public async Task SetRealEstateServicesConfigurationData() {
            //Logger.LogDebug($"Starting SetRealEstateServicesConfigurationData");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            var setConfigurationDataRequestPayload = new SetConfigurationDataRequestPayload(ConfigurationData, ConfigurationDataSave);
            var setConfigurationDataRequest = new SetConfigurationDataRequest(setConfigurationDataRequestPayload);
            //Logger.LogDebug($"Calling GetJsonAsync<SetRealEstateServicesConfigurationDataResponse> with 
            SetConfigurationDataResponse =
await HttpClient.PostJsonAsync<SetConfigurationDataResponse>("/SetRealEstateServicesConfigurationData",
                                                                                     setConfigurationDataRequest);
            //Logger.LogDebug($"Returned from GetJsonAsync<SetConfigurationDataResponse> with setConfigurationDataResponse.Result = {setConfigurationDataResponse.Result}");
            //Logger.LogDebug($"Leaving SetRealEstateServicesConfigurationData");
        }

        public async Task SetRealEstateServicesUserData() {
            //Logger.LogDebug($"Starting SetRealEstateServicesUserData");
            // Create the payload for  the Post
            var setUserDataRequestPayload = new SetUserDataRequestPayload(UserData, UserDataSave);
            var setUserDataRequest = new SetUserDataRequest(setUserDataRequestPayload);
            //Logger.LogDebug($"Calling PostJsonAsync<SetRealEstateServicesUserDataResponse>");
            var setUserDataResponse =
await HttpClient.PostJsonAsync<SetUserDataResponse>("/SetRealEstateServicesUserData?format=json",
                                                                            setUserDataRequest);
            //Logger.LogDebug($"Returned from PostJsonAsync<SetUserDataResponse>");
            ////Logger.LogDebug($"Returned from PostJsonAsync<SetUserDataResponse> with SetUserDataResponse.Result = {SetUserDataResponse.Result}");
            //Logger.LogDebug($"Leaving SetRealEstateServicesUserData");
        }



        public async Task PropertySearch() {
            //Logger.LogDebug($"Starting PropertySearch");
            //Logger.LogDebug($"MinBedrooms = {MinBedrooms}, MaxBedrooms = {MaxBedrooms}, MinBathrooms = {MinBathrooms}, MaxBathrooms = {MaxBathrooms}, Lat = {Lat}, Lng = {Lng}, DistanceInKm = {DistanceInKm}");
            //Logger.LogDebug($"Availability = {Availability}, Details = {Details}, Location = {Location}, Rates = {Rates}, Sites = {Sites}");
            // Create the payload for  the Post
            PropertySearchRequestData propertySearchRequestData = new PropertySearchRequestData(new SearchParameters(MinBedrooms,
                                                                                                                           MaxBedrooms,
                                                                                                                           MinBathrooms,
                                                                                                                           MaxBathrooms,
                                                                                                                           Lat,
                                                                                                                           Lng,
                                                                                                                           DistanceInKm),
                                                                                                      new ListingParameters(Availability, Details, Location, Rates, Sites));
            PropertySearchRequestPayload propertySearchRequestPayload = new PropertySearchRequestPayload(propertySearchRequestData,
                                                                                                         SavePropertySearch);

            PropertySearchRequest propertySearchRequest = new PropertySearchRequest();
            propertySearchRequest.PropertySearchRequestPayload=propertySearchRequestPayload;
            //Logger.LogDebug($"Calling PostJsonAsync<PropertySearchResponse>");
            PropertySearchResponse=
    await HttpClient.PostJsonAsync<PropertySearchResponse>("/PropertySearch?format=json",
                                                           propertySearchRequest);
            //Logger.LogDebug($"Returned from PostJsonAsync<PropertySearchResponse>");
            // Do something with the results

            //Logger.LogDebug($"Leaving PropertySearch");
        }
        // Access the Logging extensions registered in the DI container
        /*
        [Inject]
        public ILogger<RealEstateServicesCodeBehind> Logger {
            get;
            set;
        }
        */

        #region Properties:Initialization
        public InitializationResponse InitializationResponse { get; set; } = null;
        #endregion
        #region Properties:UserData
        public UserData UserData { get; set; } = new UserData() { GoogleAPIKeyPassPhrase=placeHolderForGoogleAPIKeyPassPhrase, HomeAwayAPIKeyPassPhrase=placeHolderForHomeAwayAPIKeyPassPhrase };
        public bool GoogleAPIKeyPassPhraseSave { get; set; }

        public string HomeAwayAPIKey { get; set; }

        public string HomeAwayAPIKeyPlaceHolder { get; set; } = placeHolderForHomeAwayAPIKey;

        public string GoogleAPIKey { get; set; }

        public string GoogleAPIKeyPlaceHolder { get; set; } = placeHolderForGoogleAPIKey;

        public bool UserDataSave { get; set; }

        #endregion PropertiesUserData

        #region Properties:ConfigurationData
        public ConfigurationData ConfigurationData { get; set; } = new ConfigurationData() {
        Google_API_URI = placeHolderForGoogle_API_URI,
        HomeAway_API_URI = placeHolderForHomeAway_API_URI};
        public SetConfigurationDataResponse SetConfigurationDataResponse { get; set; }
        public bool ConfigurationDataSave { get; set; }
        #endregion

     
        #region PropertySearch
        public int MinBedrooms { get; set; }

        public int MaxBedrooms { get; set; }

        public decimal MinBathrooms { get; set; }

        public decimal MaxBathrooms { get; set; }

        public decimal Lat { get; set; }

        public decimal DistanceInKm { get; set; }

        public decimal Lng { get; set; }

        public bool Availability { get; set; }
        public bool Details { get; set; }
        public bool Location { get; set; }
        public bool Rates { get; set; }
        public bool Sites { get; set; }

        public bool SavePropertySearch { get; set; }

        public PropertySearchResponse PropertySearchResponse {
            get;
            set;
        }
        #endregion PropertySearch
    }
}
