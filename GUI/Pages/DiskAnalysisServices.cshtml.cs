// Required for the HttpClient
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ace.Agent.DiskAnalysisServices;
// Required for the logger/logging
//using Blazor.Extensions.Logging;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Ace.AceGUI.Pages
{
  public class DiskAnalysisServicesCodeBehind : ComponentBase
  {
    public const string labelForConfigurationData = "Configuration Data";
    public const string labelForConfigurationDataSave = "save the DiskAnalysis Configuration Data?";
    // Eventually replace with localization
    public const string labelForUserData = "User Data";
    public const string labelForUserDataSave = "save the User Data?";
    // Add constant structures for configuration data and user data to be used when the GUI is displayed before it can initialize with the agent
    // Eventually localized
    public static ConfigurationData configurationDataPlaceholder = new ConfigurationData("Configuration Data pre-init placeholder");

    public static UserData userDataPlaceholder = new UserData("User Data pre-init placeholder");

    // This syntax adds to the class a Method that accesses the DI container, and retrieves the instance having the specified type from the DI container. In this case, we are accessing a builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type. This method call returns that object from the DI container  
    [Inject]
    HttpClient HttpClient {
      get;
      set;
    }

    // Access the Logging extensions registered in the DI container
    // [Inject]
    // public ILogger<DiskAnalysisServicesCodeBehind> Logger
    // {
    //      get;
    //     set;
    //  }

    protected override async Task OnInitAsync()
    {
      //Logger.LogDebug($"Starting OnInitAsync");
      Console.WriteLine($"Starting OnInitAsync");
      InitializationData initializationData = new InitializationData();
      InitializationRequestPayload initializationRequestPayload = new InitializationRequestPayload(initializationData);
      InitializationRequest = new InitializationRequest() { InitializationRequestPayload = initializationRequestPayload } ;
      //Logger.LogDebug($"Calling PostJsonAsync<InitializationResponse> with InitializationRequest ={InitializationRequest}");
      InitializationResponse = await HttpClient.PostJsonAsync<InitializationResponse>("DiskAnalysisServicesInitialization",
                                InitializationRequest);
      //Logger.LogDebug($"Returned from GetJsonAsync<InitializationResponse>, InitializationResponse = {initializationResponse}");
      InitializationResponsePayload initializationResponsePayload = InitializationResponse.InitializationResponsePayload;
      //Logger.LogDebug($"InitializationResponsePayload = {initializationResponsePayload}");
      ConfigurationData configurationData = initializationResponsePayload.ConfigurationData;
      //Logger.LogDebug($"ConfigurationData = {configurationData}");
      UserData userData = initializationResponsePayload.UserData;
      //ToDo: trigger screen refresh
      //Logger.LogDebug($"Leaving OnInitAsync");
    }

    public async Task ReadDisk(int dummyint)
    {
      //Logger.LogDebug($"Starting ReadDisk");
      //Console.WriteLine("Starting ReadDisk");
      // Create the payload for  the Post
      ReadDiskRequestData readDiskRequestData = new ReadDiskRequestData("placeholder");
      //ToDo: add a cancellation token
      ReadDiskRequestPayload readDiskRequestPayload = new ReadDiskRequestPayload(readDiskRequestData);
      ReadDiskRequest readDiskRequest = new ReadDiskRequest(readDiskRequestPayload);
      //Logger.LogDebug($"Calling PostJsonAsync<ReadDiskResponse>");
      // change the ReadDisk button's color
      ReadDiskResponse =
    await HttpClient.PostJsonAsync<ReadDiskResponse>("/ReadDisk?format=json",
                                                           readDiskRequest);
      //Logger.LogDebug($"Returned from PostJsonAsync<ReadDiskResponse>");
      
      // This should be a URL and and ID for connecting to a SSE, and the next step
      // is to draw a base result, then hookup a local task that monitors the SSE and updates the local copy of the COD

      //Logger.LogDebug($"Leaving ReadDisk");
    }

    public async Task SetConfigurationData()
    {
      //Logger.LogDebug($"Starting SetConfigurationData");
      // Create the payload for the Post
      // ToDo: Validators on the input field will make this better
      // ToDo: wrap in a try catch block and handle errors with a model dialog
      SetConfigurationDataRequestPayload setDiskAnalysisServicesConfigurationRequestPayload = new SetConfigurationDataRequestPayload(ConfigurationData, ConfigurationDataSave);
      SetConfigurationDataRequest setConfigurationDataRequest = new SetConfigurationDataRequest();
      setConfigurationDataRequest.SetConfigurationDataRequestPayload = setDiskAnalysisServicesConfigurationRequestPayload;
      //Logger.LogDebug($"Calling GetJsonAsync<SetConfigurationDataResponse> with SetConfigurationDataRequest = {setConfigurationDataRequest}");
      SetConfigurationDataResponse setConfigurationDataResponse =
await HttpClient.PostJsonAsync<SetConfigurationDataResponse>("/SetConfigurationData?format=json",
                                                                                     setConfigurationDataRequest);
      //Logger.LogDebug($"Returned from GetJsonAsync<SetConfigurationDataResponse> with setConfigurationDataResponse = {setConfigurationDataResponse}");
      //Logger.LogDebug($"Leaving SetConfigurationData");
    }

    public async Task SetUserData()
    {
      //Logger.LogDebug($"Starting SetUserData");
      // Create the payload for the Post
      SetUserDataRequestPayload setDiskAnalysisServicesUserRequestPayload = new SetUserDataRequestPayload(UserData,
UserDataSave);
      SetUserDataRequest setUserDataRequest = new SetUserDataRequest();
      setUserDataRequest.SetUserDataRequestPayload = setDiskAnalysisServicesUserRequestPayload;
      //Logger.LogDebug($"Calling PostJsonAsync<SetUserDataResponsePayload>");
      SetUserDataResponse =
await HttpClient.PostJsonAsync<SetUserDataResponse>("/SetUserData?format=json",
                                                                      setUserDataRequest);
      //Logger.LogDebug($"Returned from PostJsonAsync<SetUserDataResponsePayload>");
      ////Logger.LogDebug($"Returned from PostJsonAsync<SetUserDataResponsePayload> with SetConfigurationDataResponsePayload.Result = {SetConfigurationDataResponsePayload.Result}");
      //Logger.LogDebug($"Leaving SetUserData");
    }


    #region UserData
    public UserData UserData { get; set; } = userDataPlaceholder;


    public bool UserDataSave { get; set; }

    public SetUserDataResponse SetUserDataResponse {
      get;
      set;
    }
    #endregion UserData

    #region ConfigurationData
    public ConfigurationData ConfigurationData { get; set; } = configurationDataPlaceholder;

    public bool ConfigurationDataSave { get; set; }

    public SetConfigurationDataResponsePayload SetConfigurationDataResponse {
      get;
      set;
    }
    #endregion ConfigurationData

    #region Initialization
    public InitializationRequest InitializationRequest { get; set; }
    public InitializationResponse InitializationResponse { get; set; }

    public bool InitializationResponseOK = false;
    public string InitializationRequestParameters = "None";
    #endregion Initialization

    #region ReadDisk
    public ReadDiskResponse ReadDiskResponse {
      get;
      set;
    }
    #endregion ReadDisk
  }
}
