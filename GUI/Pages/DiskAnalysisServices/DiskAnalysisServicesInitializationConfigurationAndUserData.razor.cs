// Required for the HttpClient
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ace.AceGUI.HttpClientExtensions;
using Ace.Agent.BaseServices;
using Ace.Plugin.DiskAnalysisServices;
// Required for the logger/logging
//using Blazor.Extensions.Logging;
// Required for Blazor
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ServiceStack.Text;

namespace Ace.AceGUI.Pages {
    public partial class DiskAnalysisServicesCodeBehind : ComponentBase {
        // ToDo: Eventually replace with localization
        public const string labelForConfigurationDataBlockSize = "File Read Block Size";
        public const string labelForConfigurationDataSave = "save the DiskAnalysis Configuration Data?";
        public const string labelForUserData = "User Data";
        public const string labelForUserDataSave = "save the User Data?";

        // Add constant structures for configuration data and user data to be used when the GUI is displayed before it can initialize with the agent
        // Eventually localized
        public static Ace.Plugin.DiskAnalysisServices.ConfigurationData configurationDataPlaceholder = new Ace.Plugin.DiskAnalysisServices.ConfigurationData(-1);
        public static Ace.Plugin.DiskAnalysisServices.UserData userDataPlaceholder = new Ace.Plugin.DiskAnalysisServices.UserData("User Data pre-init placeholder");
        protected override async Task OnInitAsync() {
            Logger.LogDebug($"Starting DiskAnalysisServices.OnInitAsync");

            var initializationRequest = new Ace.Plugin.DiskAnalysisServices.InitializationRequest(new Ace.Plugin.DiskAnalysisServices.InitializationRequestPayload(new Ace.Agent.BaseServices.InitializationData()));
            UriBuilder.Path="DiskAnalysisServicesInitialization";
            Logger.LogDebug($"Calling PostJsonAsyncIJ<InitializationResponse> with InitializationRequest ={initializationRequest.Dump()}");
            InitializationResponse=await HttpClient.PostJsonAsyncIJ<Ace.Plugin.DiskAnalysisServices.InitializationResponse>(UriBuilder.Uri.ToString(),
                                initializationRequest);
            Logger.LogDebug($"Returned from GetJsonAsync<Agent.BaseServices.InitializationResponse>, InitializationResponse = {InitializationResponse.Dump()}");

            ConfigurationData=InitializationResponse.InitializationResponsePayload.ConfigurationData;
            UserData=InitializationResponse.InitializationResponsePayload.UserData;
            //ToDo: trigger screen refresh ?
            Logger.LogDebug($"Leaving DiskAnalysisServices.OnInitAsync");
        }

        public async Task SetConfigurationData() {
            Logger.LogDebug($"Starting DiskAnalysisServices.SetConfigurationData");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            SetConfigurationDataRequest setConfigurationDataRequest = new SetConfigurationDataRequest() { ConfigurationData=ConfigurationData, ConfigurationDataSave=ConfigurationDataSave };
            UriBuilder.Path="SetConfigurationData";
            Logger.LogDebug($"Calling GetJsonAsync<SetConfigurationDataResponse> with SetConfigurationDataRequest = {setConfigurationDataRequest.Dump()}");
            SetConfigurationDataResponse setConfigurationDataResponse =
      await HttpClient.PostJsonAsyncIJ<SetConfigurationDataResponse>(UriBuilder.Uri.ToString(),
                                                                                           setConfigurationDataRequest);
            Logger.LogDebug($"Returned from GetJsonAsync<SetConfigurationDataResponse> with setConfigurationDataResponse = {setConfigurationDataResponse.Dump()}");
            Logger.LogDebug($"Leaving SetConfigurationData");
        }

        public async Task SetUserData() {
            //Logger.LogDebug($"Starting SetUserData");
            // Create the payload for the Post
            var setUserDataRequest = new SetUserDataRequest(new SetUserDataRequestPayload(UserData,
      UserDataSave));

            //Logger.LogDebug($"Calling PostJsonAsyncIJ<SetUserDataResponsePayload>");
            SetUserDataResponse=
      await HttpClient.PostJsonAsyncIJ<SetUserDataResponse>("/SetUserData?format=json",
                                                                            setUserDataRequest);
            //Logger.LogDebug($"Returned from PostJsonAsyncIJ<SetUserDataResponsePayload>");
            ////Logger.LogDebug($"Returned from PostJsonAsyncIJ<SetUserDataResponsePayload> with SetUserDataResponsePayload.Result = {SetUserDataResponsePayload.Result}");
            //Logger.LogDebug($"Leaving SetUserData");
        }

        #region Properties

        #region Properties:Initialization
        public Ace.Plugin.DiskAnalysisServices.InitializationRequest InitializationRequest { get; set; }
        public Ace.Plugin.DiskAnalysisServices.InitializationResponse InitializationResponse { get; set; }

        public bool InitializationResponseOK = false;
        public string InitializationRequestParameters = "None";
        #endregion

        #region Properties:ConfigurationData
        public Ace.Plugin.DiskAnalysisServices.ConfigurationData ConfigurationData { get; set; } = configurationDataPlaceholder;

        public bool ConfigurationDataSave { get; set; }

        #endregion

        #region Properties:UserData
        public Ace.Plugin.DiskAnalysisServices.UserData UserData { get; set; } = userDataPlaceholder;

        public bool UserDataSave { get; set; }

        public SetUserDataRequest SetUserDataRequest { get; set; }
        public SetUserDataResponse SetUserDataResponse { get; set; }
        #endregion


        #endregion
    }
}
