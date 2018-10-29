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
  
    public class RealEstateSearchServicesCodeBehind : BlazorComponent {
    // Eventually replace with localization
    public const string labelForGoogleAPIKey = "Enter Google APIKey";
    public const string placeHolderForGoogleAPIKey = "GoogleAPI Key";
    public const string labelForGoogleAPIKeyPassPhrase = "Enter Google passphrase to decrypt Google APIKey stored in iCacheProvider";
    public const string placeHolderForGoogleAPIKeyPassPhrase = "GoogleAPI Key Passphrase";
    public const string labelForSavingGoogleAPIKey = "save the Google APIKey for this application?";
    public const string labelForHomeAwayAPIKeyPassPhrase = "Enter HomeAway passphrase to decrypt HomeAway APIKey stored in iCacheProvider";
    public const string placeHolderForHomeAwayAPIKeyPassPhrase = "HomeAwayAPI Key Passphrase";
    public const string labelForHomeAwayAPIKey = "Enter HomeAway APIKey";
    public const string labelForConfigurationDataSave = "save the Configuration data?";
    public const string placeHolderForHomeAwayAPIKey = "HomeAwayAPI Key";
    public const string labelForUserDataSave = "save the User data?";
    public const string labelForGoogle_API_URI = "Enter Google API URI string";
    public const string placeHolderForGoogle_API_URI = "GoogleAPI Uri string";
    public const string labelForHomeAway_API_URI = "Enter HomeAway API Uri string";
    public const string placeHolderForHomeAway_API_URI = "HomeAwayAPI Uri string";

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
            RealEstateSearchServicesInitializationResponse = await HttpClient.PostJsonAsync<RealEstateSearchServicesInitializationResponse>("RealEstateSearchServicesInitialization", realEstateSearchServicesInitializationDataRequestData);
      Logger.LogDebug($"Returned from GetJsonAsync<RealEstateSearchServicesInitializationResponse>, RealEstateSearchServicesInitializationResponse = {RealEstateSearchServicesInitializationResponse}, RealEstateSearchServicesInitializationResponseData = {RealEstateSearchServicesInitializationResponse.RealEstateSearchServicesInitializationResponseData}");
      Google_API_URI = RealEstateSearchServicesInitializationResponse.RealEstateSearchServicesInitializationResponseData.RealEstateSearchServicesConfigurationData.Google_API_URI;
      HomeAway_API_URI = RealEstateSearchServicesInitializationResponse.RealEstateSearchServicesInitializationResponseData.RealEstateSearchServicesConfigurationData.HomeAway_API_URI;
      //decrypt
      GoogleAPIKey = RealEstateSearchServicesInitializationResponse.RealEstateSearchServicesInitializationResponseData.RealEstateSearchServicesUserData.GoogleAPIKeyEncrypted;
      //decrypt
      GoogleAPIKeyPassPhrase = RealEstateSearchServicesInitializationResponse.RealEstateSearchServicesInitializationResponseData.RealEstateSearchServicesUserData.GoogleAPIKeyPassPhrase;
      HomeAwayAPIKey = RealEstateSearchServicesInitializationResponse.RealEstateSearchServicesInitializationResponseData.RealEstateSearchServicesUserData.HomeAwayAPIKeyEncrypted;
      HomeAwayAPIKeyPassPhrase = RealEstateSearchServicesInitializationResponse.RealEstateSearchServicesInitializationResponseData.RealEstateSearchServicesUserData.HomeAwayAPIKeyPassPhrase;

      Logger.LogDebug($"Leaving OnInitAsync");
        }

    public async Task SetRealEstateSearchServicesConfigurationData()
    {
      Logger.LogDebug($"Starting SetRealEstateSearchServicesConfigurationData");
      // Create the payload for the Post
      // ToDo: Validators on the input field will make this better
      // ToDo: wrap in a try catch block and handle errors with a model dialog
      RealEstateSearchServicesConfigurationData realEstateSearchServicesConfigurationData = new RealEstateSearchServicesConfigurationData(Google_API_URI, HomeAway_API_URI);
      SetRealEstateSearchServicesConfigurationDataRequestData setRealEstateSearchServicesConfigurationDataRequestData = new SetRealEstateSearchServicesConfigurationDataRequestData (realEstateSearchServicesConfigurationData, ConfigurationDataSave);
      SetRealEstateSearchServicesConfigurationDataRequest setRealEstateSearchServicesConfigurationDataRequest = new SetRealEstateSearchServicesConfigurationDataRequest();
      setRealEstateSearchServicesConfigurationDataRequest.SetRealEstateSearchServicesConfigurationDataRequestData = setRealEstateSearchServicesConfigurationDataRequestData;
      //var realEstateSearchServicesConfigurationDataEncoded =  new FormUrlEncodedContent(realEstateSearchServicesConfigurationData);
      Logger.LogDebug($"Calling GetJsonAsync<SetRealEstateSearchServicesConfigurationDataResponse> with SetRealEstateSearchServicesConfigurationDataRequestData = {setRealEstateSearchServicesConfigurationDataRequestData}");
      SetRealEstateSearchServicesConfigurationDataResponse =
          await HttpClient.PostJsonAsync<SetRealEstateSearchServicesConfigurationDataResponse>("/SetRealEstateSearchServicesConfigurationData?format=json", setRealEstateSearchServicesConfigurationDataRequest);
      Logger.LogDebug($"Returned from GetJsonAsync<SetRealEstateSearchServicesConfigurationDataResponse> with SetRealEstateSearchServicesConfigurationDataResponse = {SetRealEstateSearchServicesConfigurationDataResponse}");
      Logger.LogDebug($"Leaving SetRealEstateSearchServicesConfigurationData");
    }

    public async Task SetRealEstateSearchServicesUserData()
    {
      Logger.LogDebug($"Starting SetRealEstateSearchServicesUserData");
      // Create the payload for  the Post
      RealEstateSearchServicesUserData realEstateSearchServicesUserData = new RealEstateSearchServicesUserData(GoogleAPIKey,HomeAwayAPIKey, GoogleAPIKeyPassPhrase, HomeAwayAPIKeyPassPhrase);
      SetRealEstateSearchServicesUserDataRequestData setRealEstateSearchServicesUserDataRequestData = new SetRealEstateSearchServicesUserDataRequestData(realEstateSearchServicesUserData,UserDataSave);
      SetRealEstateSearchServicesUserDataRequest setRealEstateSearchServicesUserDataRequest = new SetRealEstateSearchServicesUserDataRequest();
      setRealEstateSearchServicesUserDataRequest.SetRealEstateSearchServicesUserDataRequestData = setRealEstateSearchServicesUserDataRequestData;
        Logger.LogDebug($"Calling GetJsonAsync<SetRealEstateSearchServicesUserDataResponse> with SetRealEstateSearchServicesUserDataRequestData = {setRealEstateSearchServicesUserDataRequestData.ToString()}");
      SetRealEstateSearchServicesUserDataResponse =
          await HttpClient.PostJsonAsync<SetRealEstateSearchServicesUserDataResponse>("/SetRealEstateSearchServicesUserData?format=json", setRealEstateSearchServicesUserDataRequest);
      Logger.LogDebug($"Returned from GetJsonAsync<SetRealEstateSearchServicesUserDataResponse> with SetRealEstateSearchServicesConfigurationDataResponse = {SetRealEstateSearchServicesConfigurationDataResponse}");
      var Result = SetRealEstateSearchServicesConfigurationDataResponse.Result;
      Logger.LogDebug($"Leaving SetRealEstateSearchServicesUserData");
    }

    // Access the Logging extensions registered in the DI container
    [Inject]
    public ILogger<RealEstateSearchServicesCodeBehind> Logger
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
    public SetRealEstateSearchServicesUserDataResponse SetRealEstateSearchServicesUserDataResponse { get; set; }
    #endregion PropertiesUserData

    #region PropertiesConfigurationData
    public string HomeAway_API_URI { get; set; }
    public string Google_API_URI { get; set; }
    public bool ConfigurationDataSave { get; set; }
    public SetRealEstateSearchServicesConfigurationDataResponse SetRealEstateSearchServicesConfigurationDataResponse { get; set; }
    #endregion PropertiesConfigurationData

    #region PropertiesInitialization
    public RealEstateSearchServicesInitializationResponse RealEstateSearchServicesInitializationResponse { get; set; }
    public bool RealEstateSearchServicesInitializationResponseOK = false;
    public string RealEstateSearchServicesInitializationRequestParameters = "None";
    #endregion PropertiesInitialization
  }
  }
