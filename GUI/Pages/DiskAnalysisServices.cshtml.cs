// Required for the HttpClient
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ace.Agent.DiskAnalysisServices;
// Required for the logger/logging
using Blazor.Extensions.Logging;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.Extensions.Logging;

namespace Ace.AceGUI.Pages {
    public class DiskAnalysisServicesCodeBehind : BlazorComponent {
        public const string labelForDiskAnalysisConfigurationDataSave = "save the DiskAnalysis Configuration data?";
        public const string labelForDiskAnalysisConfigurationDataPlaceholder = "Placeholder";
        // Eventually replace with localization
        public const string labelForSavingDiskAnalysisConfigurationDataPlaceholder = "save the DiskAnalysisConfigurationDataPlaceholder for this application?";
        public const string labelForSavingDiskAnalysisUserDataPlaceholder = "save the User data?";

    // This syntax adds to the class a Method that accesses the DI container, and retrieves the instance having the specified type from the DI container. In this case, we are accessing a builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type. This method call returns that object from the DI container  
    [Inject]
    HttpClient HttpClient
    {
        get;
        set;
    }

        protected override async Task OnInitAsync() {
            Logger.LogDebug($"Starting OnInitAsync");

            DiskAnalysisServicesInitializationData diskAnalysisServicesInitializationData = new DiskAnalysisServicesInitializationData();
            DiskAnalysisServicesInitializationDataRequestData diskAnalysisServicesInitializationDataRequestData = new DiskAnalysisServicesInitializationDataRequestData(DiskAnalysisServicesInitializationData);
            Logger.LogDebug($"Calling PostJsonAsync<DiskAnalysisServicesInitializationResponse> with DiskAnalysisServicesInitializationDataRequestData ={DiskAnalysisServicesInitializationDataRequestData}");
            DiskAnalysisServicesInitializationResponse = await HttpClient.PostJsonAsync<DiskAnalysisServicesInitializationResponse>("DiskAnalysisServicesInitialization",
                                                                                                                                            DiskAnalysisServicesInitializationDataRequestData);
            Logger.LogDebug($"Returned from GetJsonAsync<DiskAnalysisServicesInitializationResponse>, DiskAnalysisServicesInitializationResponse = {DiskAnalysisServicesInitializationResponse}, DiskAnalysisServicesInitializationResponseData = {DiskAnalysisServicesInitializationResponse.DiskAnalysisServicesInitializationResponseData}");
            Google_API_URI = DiskAnalysisServicesInitializationResponse.DiskAnalysisServicesInitializationResponseData
                .DiskAnalysisServicesConfigurationData
                .Google_API_URI;
            HomeAway_API_URI = DiskAnalysisServicesInitializationResponse.DiskAnalysisServicesInitializationResponseData
                .DiskAnalysisServicesConfigurationData
                .HomeAway_API_URI;
            //decrypt
            GoogleAPIKey = DiskAnalysisServicesInitializationResponse.DiskAnalysisServicesInitializationResponseData
                .DiskAnalysisServicesUserData
                .GoogleAPIKeyEncrypted;
            //decrypt
            GoogleAPIKeyPassPhrase = DiskAnalysisServicesInitializationResponse.DiskAnalysisServicesInitializationResponseData
                .DiskAnalysisServicesUserData
                .GoogleAPIKeyPassPhrase;
            HomeAwayAPIKey = DiskAnalysisServicesInitializationResponse.DiskAnalysisServicesInitializationResponseData
                .DiskAnalysisServicesUserData
                .HomeAwayAPIKeyEncrypted;
            HomeAwayAPIKeyPassPhrase = DiskAnalysisServicesInitializationResponse.DiskAnalysisServicesInitializationResponseData
                .DiskAnalysisServicesUserData
                .HomeAwayAPIKeyPassPhrase;

            Logger.LogDebug($"Leaving OnInitAsync");
        }

        public async Task ReadDisk() {
            Logger.LogDebug($"Starting ReadDisk");

      // Create the payload for  the Post
      ReadDiskRequestData readDiskRequestData = new ReadDiskRequestData(new ReadDiskParameters("placeholder"));

            ReadDiskRequest ReadDiskRequest = new ReadDiskRequest();
            ReadDiskRequest.ReadDiskRequestPayload = ReadDiskRequestPayload;
            Logger.LogDebug($"Calling PostJsonAsync<ReadDiskResponse>");
            ReadDiskResponse =
    await HttpClient.PostJsonAsync<ReadDiskResponse>("/ReadDisk?format=json",
                                                           ReadDiskRequest);
            Logger.LogDebug($"Returned from PostJsonAsync<ReadDiskResponse>");
            // Do something with the results

            Logger.LogDebug($"Leaving ReadDisk");
        }

        public async Task SetDiskAnalysisServicesConfigurationData() {
            Logger.LogDebug($"Starting SetDiskAnalysisServicesConfigurationData");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            DiskAnalysisServicesConfigurationData DiskAnalysisServicesConfigurationData = new DiskAnalysisServicesConfigurationData(Google_API_URI,
                                                                                                                                                HomeAway_API_URI);
            SetDiskAnalysisServicesConfigurationDataRequestPayload setDiskAnalysisServicesConfigurationDataRequestData = new SetDiskAnalysisServicesConfigurationDataRequestPayload(DiskAnalysisServicesConfigurationData,
                                                                                                                                                                                          ConfigurationDataSave);
            SetDiskAnalysisServicesConfigurationDataRequest setDiskAnalysisServicesConfigurationDataRequest = new SetDiskAnalysisServicesConfigurationDataRequest();
            setDiskAnalysisServicesConfigurationDataRequest.SetDiskAnalysisServicesConfigurationDataRequestData = setDiskAnalysisServicesConfigurationDataRequestData;
            //var DiskAnalysisServicesConfigurationDataEncoded =  new FormUrlEncodedContent(DiskAnalysisServicesConfigurationData);
            Logger.LogDebug($"Calling GetJsonAsync<SetDiskAnalysisServicesConfigurationDataResponsePayload> with SetDiskAnalysisServicesConfigurationDataRequestPayload = {setDiskAnalysisServicesConfigurationDataRequestData}");
            SetDiskAnalysisServicesConfigurationDataResponse =
await HttpClient.PostJsonAsync<SetDiskAnalysisServicesConfigurationDataResponsePayload>("/SetDiskAnalysisServicesConfigurationData?format=json",
                                                                                     setDiskAnalysisServicesConfigurationDataRequest);
            Logger.LogDebug($"Returned from GetJsonAsync<SetDiskAnalysisServicesConfigurationDataResponsePayload> with SetDiskAnalysisServicesConfigurationDataResponsePayload = {SetDiskAnalysisServicesConfigurationDataResponse}");
            Logger.LogDebug($"Leaving SetDiskAnalysisServicesConfigurationData");
        }

        public async Task SetDiskAnalysisServicesUserData() {
            Logger.LogDebug($"Starting SetDiskAnalysisServicesUserData");
            // Create the payload for the Post
            DiskAnalysisServicesUserData diskAnalysisServicesUserData = new DiskAnalysisServicesUserData("placeholder");
            SetDiskAnalysisServicesUserDataRequestPayload setDiskAnalysisServicesUserDataRequestData = new SetDiskAnalysisServicesUserDataRequestPayload(diskAnalysisServicesUserData,
diskAnalysisServicesUserDataSave);
            SetDiskAnalysisServicesUserDataRequest setDiskAnalysisServicesUserDataRequest = new SetDiskAnalysisServicesUserDataRequest();
            setDiskAnalysisServicesUserDataRequest.SetDiskAnalysisServicesUserDataRequestData = setDiskAnalysisServicesUserDataRequestData;
            Logger.LogDebug($"Calling PostJsonAsync<SetDiskAnalysisServicesUserDataResponsePayload>");
            SetDiskAnalysisServicesUserDataResponse =
await HttpClient.PostJsonAsync<SetDiskAnalysisServicesUserDataResponsePayload>("/SetDiskAnalysisServicesUserData?format=json",
                                                                            setDiskAnalysisServicesUserDataRequest);
            Logger.LogDebug($"Returned from PostJsonAsync<SetDiskAnalysisServicesUserDataResponsePayload>");
            //Logger.LogDebug($"Returned from PostJsonAsync<SetDiskAnalysisServicesUserDataResponsePayload> with SetDiskAnalysisServicesConfigurationDataResponsePayload.Result = {SetDiskAnalysisServicesConfigurationDataResponsePayload.Result}");
            Logger.LogDebug($"Leaving SetDiskAnalysisServicesUserData");
        }

    // Access the Logging extensions registered in the DI container
    [Inject]
    public ILogger<DiskAnalysisServicesCodeBehind> Logger
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

        public SetDiskAnalysisServicesUserDataResponsePayload SetDiskAnalysisServicesUserDataResponse {
            get;
            set;
        }
    #endregion PropertiesUserData

    #region PropertiesConfigurationData
    public string HomeAway_API_URI { get; set; }

        public string Google_API_URI { get; set; }

        public bool ConfigurationDataSave { get; set; }

        public SetDiskAnalysisServicesConfigurationDataResponsePayload SetDiskAnalysisServicesConfigurationDataResponse {
            get;
            set;
        }
    #endregion PropertiesConfigurationData

    #region PropertiesInitialization
    public DiskAnalysisServicesInitializationResponse DiskAnalysisServicesInitializationResponse { get; set; }

        public bool DiskAnalysisServicesInitializationResponseOK = false;
        public string DiskAnalysisServicesInitializationRequestParameters = "None";
    #endregion PropertiesInitialization

    #region ReadDisk
        public ReadDiskResponse ReadDiskResponse {
            get;
            set;
        }
    #endregion ReadDisk
  }
}
