// Required for the HttpClient
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ace.Agent.BaseServices;
using Ace.Agent.DiskAnalysisServices;
using ATAP.Utilities.DiskDrive;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
// Required for the logger/logging
//using Blazor.Extensions.Logging;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Ace.AceGUI.Pages {
    public partial class DiskAnalysisServicesCodeBehind : ComponentBase {
        // ToDo: Eventually replace with localization
        public const string labelForConfigurationDataBlockSize = "File Read Block Size";
        public const string labelForConfigurationDataSave = "save the DiskAnalysis Configuration Data?";
        public const string labelForUserData = "User Data";
        public const string labelForUserDataSave = "save the User Data?";

        // Add constant structures for configuration data and user data to be used when the GUI is displayed before it can initialize with the agent
        // Eventually localized
        public static Ace.Agent.DiskAnalysisServices.ConfigurationData configurationDataPlaceholder = new Ace.Agent.DiskAnalysisServices.ConfigurationData(-1);
        public static Ace.Agent.DiskAnalysisServices.UserData userDataPlaceholder = new Ace.Agent.DiskAnalysisServices.UserData("User Data pre-init placeholder");

        // Add constant structures for configuration data and user data to be used when the GUI is displayed before it can initialize with the agent
        // Eventually localized
        protected override async Task OnInitAsync() {
            //Logger.LogDebug($"Starting OnInitAsync");

            var initializationRequest = new Ace.Agent.DiskAnalysisServices.InitializationRequest(new Ace.Agent.DiskAnalysisServices.InitializationRequestPayload(new Ace.Agent.BaseServices.InitializationData()));
            //Logger.LogDebug($"Calling PostJsonAsync<InitializationResponse> with InitializationRequest ={initializationRequest}");
            InitializationResponse=await HttpClient.PostJsonAsync<Ace.Agent.DiskAnalysisServices.InitializationResponse>("/DiskAnalysisServicesInitialization",
                                initializationRequest);
            //Logger.LogDebug($"Returned from GetJsonAsync<Agent.BaseServices.InitializationResponse>, InitializationResponse = {InitializationResponse}");

            ConfigurationData=InitializationResponse.InitializationResponsePayload.ConfigurationData;
            UserData=InitializationResponse.InitializationResponsePayload.UserData;
            //ToDo: trigger screen refresh ?
            //Logger.LogDebug($"Leaving OnInitAsync");
        }

        public async Task SetConfigurationData() {
            //Logger.LogDebug($"Starting SetConfigurationData");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            SetConfigurationDataRequest setConfigurationDataRequest = new SetConfigurationDataRequest() { ConfigurationData=ConfigurationData, ConfigurationDataSave=ConfigurationDataSave };
            //Logger.LogDebug($"Calling GetJsonAsync<SetConfigurationDataResponse> with SetConfigurationDataRequest = {setConfigurationDataRequest}");
            SetConfigurationDataResponse setConfigurationDataResponse =
      await HttpClient.PostJsonAsync<SetConfigurationDataResponse>("/SetConfigurationData?format=json",
                                                                                           setConfigurationDataRequest);
            //Logger.LogDebug($"Returned from GetJsonAsync<SetConfigurationDataResponse> with setConfigurationDataResponse = {setConfigurationDataResponse}");
            //Logger.LogDebug($"Leaving SetConfigurationData");
        }

        public async Task SetUserData() {
            //Logger.LogDebug($"Starting SetUserData");
            // Create the payload for the Post
            var setUserDataRequest = new SetUserDataRequest(new SetUserDataRequestPayload(UserData,
      UserDataSave));

            //Logger.LogDebug($"Calling PostJsonAsync<SetUserDataResponsePayload>");
            SetUserDataResponse=
      await HttpClient.PostJsonAsync<SetUserDataResponse>("/SetUserData?format=json",
                                                                            setUserDataRequest);
            //Logger.LogDebug($"Returned from PostJsonAsync<SetUserDataResponsePayload>");
            ////Logger.LogDebug($"Returned from PostJsonAsync<SetUserDataResponsePayload> with SetUserDataResponsePayload.Result = {SetUserDataResponsePayload.Result}");
            //Logger.LogDebug($"Leaving SetUserData");
        }

        #region Properties

        #region Properties:Initialization
        public Ace.Agent.DiskAnalysisServices.InitializationRequest InitializationRequest { get; set; }
        public Ace.Agent.DiskAnalysisServices.InitializationResponse InitializationResponse { get; set; }

        public bool InitializationResponseOK = false;
        public string InitializationRequestParameters = "None";
        #endregion

        #region Properties:ConfigurationData
        public Ace.Agent.DiskAnalysisServices.ConfigurationData ConfigurationData { get; set; } = configurationDataPlaceholder;

        public bool ConfigurationDataSave { get; set; }

        #endregion

        #region Properties:UserData
        public Ace.Agent.DiskAnalysisServices.UserData UserData { get; set; } = userDataPlaceholder;

        public bool UserDataSave { get; set; }

        public SetUserDataRequest SetUserDataRequest { get; set; }
        public SetUserDataResponse SetUserDataResponse { get; set; }
        #endregion


        #endregion
    }
}
