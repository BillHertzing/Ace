// Required for the HttpClient
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ace.AceCommon.Plugin.RealEstateSearchServices;
// Required for the logger/logging
using Blazor.Extensions.Logging;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.Extensions.Logging;

namespace Ace.AceGUI.Pages {
    public class RealEstateServicesCodeBehind : BlazorComponent {
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
    HttpClient HttpClient
    {
        get;
        set;
    }

        protected override async Task OnInitAsync() {
            Logger.LogDebug($"Starting OnInitAsync");

            RealEstateSearchServicesInitializationData realEstateSearchServicesInitializationData = new RealEstateSearchServicesInitializationData();
            RealEstateSearchServicesInitializationDataRequestData realEstateSearchServicesInitializationDataRequestData = new RealEstateSearchServicesInitializationDataRequestData(realEstateSearchServicesInitializationData);
            Logger.LogDebug($"Calling PostJsonAsync<RealEstateSearchServicesInitializationResponse> with realEstateSearchServicesInitializationDataRequestData ={realEstateSearchServicesInitializationDataRequestData}");
            RealEstateSearchServicesInitializationResponse = await HttpClient.PostJsonAsync<RealEstateSearchServicesInitializationResponse>("RealEstateSearchServicesInitialization",
                                                                                                                                            realEstateSearchServicesInitializationDataRequestData);
            Logger.LogDebug($"Returned from GetJsonAsync<RealEstateSearchServicesInitializationResponse>, RealEstateSearchServicesInitializationResponse = {RealEstateSearchServicesInitializationResponse}, RealEstateSearchServicesInitializationResponseData = {RealEstateSearchServicesInitializationResponse.RealEstateSearchServicesInitializationResponseData}");
            Google_API_URI = RealEstateSearchServicesInitializationResponse.RealEstateSearchServicesInitializationResponseData
                .RealEstateSearchServicesConfigurationData
                .Google_API_URI;
            HomeAway_API_URI = RealEstateSearchServicesInitializationResponse.RealEstateSearchServicesInitializationResponseData
                .RealEstateSearchServicesConfigurationData
                .HomeAway_API_URI;
            //decrypt
            GoogleAPIKey = RealEstateSearchServicesInitializationResponse.RealEstateSearchServicesInitializationResponseData
                .RealEstateSearchServicesUserData
                .GoogleAPIKeyEncrypted;
            //decrypt
            GoogleAPIKeyPassPhrase = RealEstateSearchServicesInitializationResponse.RealEstateSearchServicesInitializationResponseData
                .RealEstateSearchServicesUserData
                .GoogleAPIKeyPassPhrase;
            HomeAwayAPIKey = RealEstateSearchServicesInitializationResponse.RealEstateSearchServicesInitializationResponseData
                .RealEstateSearchServicesUserData
                .HomeAwayAPIKeyEncrypted;
            HomeAwayAPIKeyPassPhrase = RealEstateSearchServicesInitializationResponse.RealEstateSearchServicesInitializationResponseData
                .RealEstateSearchServicesUserData
                .HomeAwayAPIKeyPassPhrase;

            Logger.LogDebug($"Leaving OnInitAsync");
        }

        public async Task PropertySearch() {
            Logger.LogDebug($"Starting PropertySearch");
      Logger.LogDebug($"MinBedrooms = {MinBedrooms}, MaxBedrooms = {MaxBedrooms}, MinBathrooms = {MinBathrooms}, MaxBathrooms = {MaxBathrooms}, Lat = {Lat}, Lng = {Lng}, DistanceInKm = {DistanceInKm}");
      Logger.LogDebug($"Availability = {Availability}, Details = {Details}, Location = {Location}, Rates = {Rates}, Sites = {Sites}");
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
            propertySearchRequest.PropertySearchRequestPayload = propertySearchRequestPayload;
            Logger.LogDebug($"Calling PostJsonAsync<PropertySearchResponse>");
            PropertySearchResponse =
    await HttpClient.PostJsonAsync<PropertySearchResponse>("/PropertySearch?format=json",
                                                           propertySearchRequest);
            Logger.LogDebug($"Returned from PostJsonAsync<PropertySearchResponse>");
            // Do something with the results

            Logger.LogDebug($"Leaving PropertySearch");
        }

        public async Task SetRealEstateSearchServicesConfigurationData() {
            Logger.LogDebug($"Starting SetRealEstateSearchServicesConfigurationData");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            RealEstateSearchServicesConfigurationData realEstateSearchServicesConfigurationData = new RealEstateSearchServicesConfigurationData(Google_API_URI,
                                                                                                                                                HomeAway_API_URI);
            SetRealEstateSearchServicesConfigurationDataRequestData setRealEstateSearchServicesConfigurationDataRequestData = new SetRealEstateSearchServicesConfigurationDataRequestData(realEstateSearchServicesConfigurationData,
                                                                                                                                                                                          ConfigurationDataSave);
            SetRealEstateSearchServicesConfigurationDataRequest setRealEstateSearchServicesConfigurationDataRequest = new SetRealEstateSearchServicesConfigurationDataRequest();
            setRealEstateSearchServicesConfigurationDataRequest.SetRealEstateSearchServicesConfigurationDataRequestData = setRealEstateSearchServicesConfigurationDataRequestData;
            //var realEstateSearchServicesConfigurationDataEncoded =  new FormUrlEncodedContent(realEstateSearchServicesConfigurationData);
            Logger.LogDebug($"Calling GetJsonAsync<SetRealEstateSearchServicesConfigurationDataResponse> with SetRealEstateSearchServicesConfigurationDataRequestData = {setRealEstateSearchServicesConfigurationDataRequestData}");
            SetRealEstateSearchServicesConfigurationDataResponse =
await HttpClient.PostJsonAsync<SetRealEstateSearchServicesConfigurationDataResponse>("/SetRealEstateSearchServicesConfigurationData?format=json",
                                                                                     setRealEstateSearchServicesConfigurationDataRequest);
            Logger.LogDebug($"Returned from GetJsonAsync<SetRealEstateSearchServicesConfigurationDataResponse> with SetRealEstateSearchServicesConfigurationDataResponse = {SetRealEstateSearchServicesConfigurationDataResponse}");
            Logger.LogDebug($"Leaving SetRealEstateSearchServicesConfigurationData");
        }

        public async Task SetRealEstateSearchServicesUserData() {
            Logger.LogDebug($"Starting SetRealEstateSearchServicesUserData");
            // Create the payload for  the Post
            RealEstateSearchServicesUserData realEstateSearchServicesUserData = new RealEstateSearchServicesUserData(GoogleAPIKey,
                                                                                                                     HomeAwayAPIKey,
                                                                                                                     GoogleAPIKeyPassPhrase,
                                                                                                                     HomeAwayAPIKeyPassPhrase);
            SetRealEstateSearchServicesUserDataRequestData setRealEstateSearchServicesUserDataRequestData = new SetRealEstateSearchServicesUserDataRequestData(realEstateSearchServicesUserData,
                                                                                                                                                               UserDataSave);
            SetRealEstateSearchServicesUserDataRequest setRealEstateSearchServicesUserDataRequest = new SetRealEstateSearchServicesUserDataRequest();
            setRealEstateSearchServicesUserDataRequest.SetRealEstateSearchServicesUserDataRequestData = setRealEstateSearchServicesUserDataRequestData;
            Logger.LogDebug($"Calling PostJsonAsync<SetRealEstateSearchServicesUserDataResponse>");
            SetRealEstateSearchServicesUserDataResponse =
await HttpClient.PostJsonAsync<SetRealEstateSearchServicesUserDataResponse>("/SetRealEstateSearchServicesUserData?format=json",
                                                                            setRealEstateSearchServicesUserDataRequest);
            Logger.LogDebug($"Returned from PostJsonAsync<SetRealEstateSearchServicesUserDataResponse>");
            //Logger.LogDebug($"Returned from PostJsonAsync<SetRealEstateSearchServicesUserDataResponse> with SetRealEstateSearchServicesConfigurationDataResponse.Result = {SetRealEstateSearchServicesConfigurationDataResponse.Result}");
            Logger.LogDebug($"Leaving SetRealEstateSearchServicesUserData");
        }

    // Access the Logging extensions registered in the DI container
    [Inject]
    public ILogger<RealEstateServicesCodeBehind> Logger
    {
        get;
        set;
    }

    #region PropertiesUserData
        public string GoogleAPIKeyPassPhrase { get; set; }

        public string GoogleAPIKeyPassPhrasePlaceHolder { get; set; } = placeHolderForGoogleAPIKeyPassPhrase;

        public bool GoogleAPIKeyPassPhraseSave { get; set; }

        public string HomeAwayAPIKeyPassPhrase { get; set; }

        public string HomeAwayAPIKeyPassPhrasePlaceHolder { get; set; } = placeHolderForHomeAwayAPIKeyPassPhrase;

        public string HomeAwayAPIKey { get; set; }

        public string HomeAwayAPIKeyPlaceHolder { get; set; } = placeHolderForHomeAwayAPIKey;

        public string GoogleAPIKey { get; set; }

        public string GoogleAPIKeyPlaceHolder { get; set; } = placeHolderForGoogleAPIKey;

        public bool UserDataSave { get; set; }

        public SetRealEstateSearchServicesUserDataResponse SetRealEstateSearchServicesUserDataResponse {
            get;
            set;
        }
    #endregion PropertiesUserData

    #region PropertiesConfigurationData
    public string HomeAway_API_URI { get; set; }

        public string Google_API_URI { get; set; }

        public bool ConfigurationDataSave { get; set; }

        public SetRealEstateSearchServicesConfigurationDataResponse SetRealEstateSearchServicesConfigurationDataResponse {
            get;
            set;
        }
    #endregion PropertiesConfigurationData

    #region PropertiesInitialization
    public RealEstateSearchServicesInitializationResponse RealEstateSearchServicesInitializationResponse { get; set; }

        public bool RealEstateSearchServicesInitializationResponseOK = false;
        public string RealEstateSearchServicesInitializationRequestParameters = "None";
    #endregion PropertiesInitialization

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
