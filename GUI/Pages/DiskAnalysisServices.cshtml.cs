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
    public class DiskAnalysisServicesCodeBehind : ComponentBase
  {
        public const string labelForDiskAnalysisConfigurationDataSave = "save the DiskAnalysis Configuration data?";
        public const string labelForDiskAnalysisConfigurationDataPlaceholder = "Placeholder";
        // Eventually replace with localization
        public const string labelForSavingDiskAnalysisConfigurationDataPlaceholder = "save the DiskAnalysisConfigurationDataPlaceholder for this application?";
        public const string labelForSavingDiskAnalysisUserDataPlaceholder = "save the User data?";
    // Add constant structures for configuration data and user data to be used when the GUI is displayed before it can initialize with the agent
    // Eventually localized

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
            DiskAnalysisServicesInitializationRequestPayload diskAnalysisServicesInitializationRequestPayload = new DiskAnalysisServicesInitializationRequestPayload(diskAnalysisServicesInitializationData);
            Logger.LogDebug($"Calling PostJsonAsync<DiskAnalysisServicesInitializationResponse> with DiskAnalysisServicesInitializationRequestPayload ={diskAnalysisServicesInitializationData}");
            DiskAnalysisServicesInitializationResponse = await HttpClient.PostJsonAsync<DiskAnalysisServicesInitializationResponse>("DiskAnalysisServicesInitialization",
                                                                                                                                            diskAnalysisServicesInitializationData);
            Logger.LogDebug($"Returned from GetJsonAsync<DiskAnalysisServicesInitializationResponse>, DiskAnalysisServicesInitializationResponse = {DiskAnalysisServicesInitializationResponse}, DiskAnalysisServicesInitializationResponsePayload = {DiskAnalysisServicesInitializationResponse.DiskAnalysisServicesInitializationResponsePayload}, DiskAnalysisServicesConfigurationData = {DiskAnalysisServicesInitializationResponse.DiskAnalysisServicesInitializationResponsePayload.DiskAnalysisServicesConfigurationData}, DiskAnalysisServicesUserData = {DiskAnalysisServicesInitializationResponse.DiskAnalysisServicesInitializationResponsePayload.DiskAnalysisServicesUserData}");
      DiskAnalysisServicesUserData = DiskAnalysisServicesInitializationResponse.DiskAnalysisServicesInitializationResponsePayload.DiskAnalysisServicesUserData;
      DiskAnalysisServicesConfigurationData = DiskAnalysisServicesInitializationResponse.DiskAnalysisServicesInitializationResponsePayload.DiskAnalysisServicesConfigurationData;
      //ToDo: trigger screen refresh
            Logger.LogDebug($"Leaving OnInitAsync");
        }

        public async Task ReadDisk() {
            Logger.LogDebug($"Starting ReadDisk");

      // Create the payload for  the Post
      ReadDiskRequestData readDiskRequestData = new ReadDiskRequestData("placeholder");
      //ToDo: add a cancellation token
      ReadDiskRequestPayload readDiskRequestPayload = new ReadDiskRequestPayload(readDiskRequestData);
            ReadDiskRequest readDiskRequest = new ReadDiskRequest();
            Logger.LogDebug($"Calling PostJsonAsync<ReadDiskResponse>");
      ReadDiskResponse readDiskResponse =
    await HttpClient.PostJsonAsync<ReadDiskResponse>("/ReadDisk?format=json",
                                                           readDiskRequest);
            Logger.LogDebug($"Returned from PostJsonAsync<ReadDiskResponse>");
            // Do something with the results
            // This should be a URL and and ID for connecting to a SSE, and the next step
            // is to draw a base result, then hookup a local task that monitors the SSE and updates the local copy of the COD

            Logger.LogDebug($"Leaving ReadDisk");
        }

        public async Task SetDiskAnalysisServicesConfigurationData() {
            Logger.LogDebug($"Starting SetDiskAnalysisServicesConfigurationData");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            SetDiskAnalysisServicesConfigurationDataRequestPayload setDiskAnalysisServicesConfigurationRequestPayload = new SetDiskAnalysisServicesConfigurationDataRequestPayload(DiskAnalysisServicesConfigurationData, ConfigurationDataSave);
            SetDiskAnalysisServicesConfigurationDataRequest setDiskAnalysisServicesConfigurationDataRequest = new SetDiskAnalysisServicesConfigurationDataRequest();
            setDiskAnalysisServicesConfigurationDataRequest.SetDiskAnalysisServicesConfigurationDataRequestPayload = setDiskAnalysisServicesConfigurationRequestPayload;
            Logger.LogDebug($"Calling GetJsonAsync<SetDiskAnalysisServicesConfigurationDataResponse> with SetDiskAnalysisServicesConfigurationDataRequest = {setDiskAnalysisServicesConfigurationDataRequest}");
      SetDiskAnalysisServicesConfigurationDataResponse setDiskAnalysisServicesConfigurationDataResponse =
await HttpClient.PostJsonAsync<SetDiskAnalysisServicesConfigurationDataResponse>("/SetDiskAnalysisServicesConfigurationData?format=json",
                                                                                     setDiskAnalysisServicesConfigurationDataRequest);
            Logger.LogDebug($"Returned from GetJsonAsync<SetDiskAnalysisServicesConfigurationDataResponse> with setDiskAnalysisServicesConfigurationDataResponse = {setDiskAnalysisServicesConfigurationDataResponse}");
            Logger.LogDebug($"Leaving SetDiskAnalysisServicesConfigurationData");
        }

        public async Task SetDiskAnalysisServicesUserData() {
            Logger.LogDebug($"Starting SetDiskAnalysisServicesUserData");
            // Create the payload for the Post
           SetDiskAnalysisServicesUserDataRequestPayload setDiskAnalysisServicesUserRequestPayload = new SetDiskAnalysisServicesUserDataRequestPayload(DiskAnalysisServicesUserData,
UserDataSave);
            SetDiskAnalysisServicesUserDataRequest setDiskAnalysisServicesUserDataRequest = new SetDiskAnalysisServicesUserDataRequest();
            setDiskAnalysisServicesUserDataRequest.SetDiskAnalysisServicesUserDataRequestPayload = setDiskAnalysisServicesUserRequestPayload;
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

    #region DiskAnalysisServicesUserData
    public DiskAnalysisServicesUserData DiskAnalysisServicesUserData { get; set; }


        public bool UserDataSave { get; set; }

        public SetDiskAnalysisServicesUserDataResponsePayload SetDiskAnalysisServicesUserDataResponse {
            get;
            set;
        }
    #endregion DiskAnalysisServicesUserData

    #region ConfigurationData
    public DiskAnalysisServicesConfigurationData DiskAnalysisServicesConfigurationData { get; set; } = new DiskAnalysisServicesConfigurationData();

        public bool ConfigurationDataSave { get; set; }

        public SetDiskAnalysisServicesConfigurationDataResponsePayload SetDiskAnalysisServicesConfigurationDataResponse {
            get;
            set;
        }
    #endregion PropertiesConfigurationData

    #region Initialization
    public DiskAnalysisServicesInitializationResponse DiskAnalysisServicesInitializationResponse { get; set; }

        public bool DiskAnalysisServicesInitializationResponseOK = false;
        public string DiskAnalysisServicesInitializationRequestParameters = "None";
    #endregion Initialization

    #region ReadDisk
        public ReadDiskResponse ReadDiskResponse {
            get;
            set;
        }
    #endregion ReadDisk
  }
}
