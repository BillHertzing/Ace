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
    public class DiskAnalysisServicesCodeBehind : ComponentBase {
        public const string labelForConfigurationDataBlockSize = "File Read Block Size";
        public const string labelForConfigurationDataSave = "save the DiskAnalysis Configuration Data?";
        // Eventually replace with localization
        public const string labelForUserData = "User Data";
        public const string labelForUserDataSave = "save the User Data?";
        // Add constant structures for configuration data and user data to be used when the GUI is displayed before it can initialize with the agent
        // Eventually localized
        public static Ace.Agent.DiskAnalysisServices.ConfigurationData configurationDataPlaceholder = new Ace.Agent.DiskAnalysisServices.ConfigurationData(-1);

        public static Ace.Agent.DiskAnalysisServices.UserData userDataPlaceholder = new Ace.Agent.DiskAnalysisServices.UserData("User Data pre-init placeholder");

        // This syntax adds to the class a Method that accesses the DI container, and retrieves the instance having the specified type from the DI container. In this case, we are accessing a builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type. This method call returns that object from the DI container  
        [Inject]
        HttpClient HttpClient { get; set; }

        // Access the Logging extensions registered in the DI container
        // [Inject]
        // public ILogger<DiskAnalysisServicesCodeBehind> Logger { get; set; }


        protected override async Task OnInitAsync() {
            //Logger.LogDebug($"Starting OnInitAsync");
            
            var initializationRequest=new Ace.Agent.DiskAnalysisServices.InitializationRequest(new Ace.Agent.DiskAnalysisServices.InitializationRequestPayload(new Ace.Agent.BaseServices.InitializationData()));
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
            var setUserDataRequest=new SetUserDataRequest(new SetUserDataRequestPayload(UserData,
      UserDataSave));

            //Logger.LogDebug($"Calling PostJsonAsync<SetUserDataResponsePayload>");
            SetUserDataResponse=
      await HttpClient.PostJsonAsync<SetUserDataResponse>("/SetUserData?format=json",
                                                                            setUserDataRequest);
            //Logger.LogDebug($"Returned from PostJsonAsync<SetUserDataResponsePayload>");
            ////Logger.LogDebug($"Returned from PostJsonAsync<SetUserDataResponsePayload> with SetUserDataResponsePayload.Result = {SetUserDataResponsePayload.Result}");
            //Logger.LogDebug($"Leaving SetUserData");
        }

        public async Task AnalyzeDiskDrive(string computerName, int diskDriveNumber) {
            //Logger.LogDebug($"Starting AnalyzeDiskDrive");
            //ToDo: add a cancellation token
            var cancellationToken = new CancellationToken(false);
            var analyzeDiskDriveRequestPayload = new AnalyzeDiskDriveRequestPayload(new DiskDriveSpecifier() { ComputerName=computerName, DiskDriveNumber=diskDriveNumber, DiskDrivePartitionIdentifier=new DiskDrivePartitionIdentifier() }, cancellationToken);
            var analyzeDiskDriveRequest = new AnalyzeDiskDriveRequest(analyzeDiskDriveRequestPayload);
            //Logger.LogDebug($"Calling PostJsonAsync<ReadDiskResponse>");
            // ToDo: deactivate the AnalyzeDiskDrive button
            var analyzeDiskDriveResponse =
          await HttpClient.PostJsonAsync<AnalyzeDiskDriveResponse>("/AnalyzeDiskDrive?format=json",
                                                                 analyzeDiskDriveRequest);
            //Logger.LogDebug($"Returned from PostJsonAsync<AnalyzeDiskDriveResponse>");
            // record the TaskID
            AnalyzeDiskDriveLongRunningTaskIDs.AddRange(analyzeDiskDriveResponse.AnalyzeDiskDriveResponsePayload.LongRunningTaskIds);
            // This should be a URL and and ID for connecting to a SSE, and the next step
            // is to draw a base result, then hookup a local task that monitors the SSE and updates the local copy of the COD
            // ToDo: Activate the AnalyzeDiskDrive button

            //Logger.LogDebug($"Leaving AnalyzeDiskDrive");
        }

        public async Task GetLongRunningTaskState(List<Id<LongRunningTaskInfo>> longRunningTaskIDs) {
            //Logger.LogDebug($"Starting GetLongRunningTaskState");
            //ToDo: add a cancellation token
            //Logger.LogDebug($"Calling PostJsonAsync<ReadDiskResponse>");
            // change the ReadDisk button's color
            GetLongRunningTaskStateResponse=
          await HttpClient.PostJsonAsync<GetLongRunningTaskStateResponse>("/GetLongRunningTaskState?format=json",
                                                                 new GetLongRunningTaskStateRequest(longRunningTaskIDs));
            //Logger.LogDebug($"Returned from PostJsonAsync<ReadDiskResponse>");
            AnalyzeDiskDriveLongRunningTaskState=GetLongRunningTaskStateResponse.LongRunningTaskState;
            // This should be a URL and and ID for connecting to a SSE, and the next step
            // is to draw a base result, then hookup a local task that monitors the SSE and updates the local copy of the COD

            //Logger.LogDebug($"Leaving ReadDisk");
        }

        #region Properties:Initialization
        public Ace.Agent.DiskAnalysisServices.InitializationRequest InitializationRequest { get; set; }
        public Ace.Agent.DiskAnalysisServices.InitializationResponse InitializationResponse { get; set; }

        public bool InitializationResponseOK = false;
        public string InitializationRequestParameters = "None";
        #endregion Properties:Initialization

        #region Properties:UserData
        public Ace.Agent.DiskAnalysisServices.UserData UserData { get; set; } = userDataPlaceholder;

        public bool UserDataSave { get; set; }

        public SetUserDataRequest SetUserDataRequest { get; set; }
        public SetUserDataResponse SetUserDataResponse { get; set; }
        #endregion Properties:UserData

        #region Properties:ConfigurationData
        public Ace.Agent.DiskAnalysisServices.ConfigurationData ConfigurationData { get; set; } = configurationDataPlaceholder;

        public bool ConfigurationDataSave { get; set; }

        #endregion Properties:ConfigurationData

        #region Properties:AnalyzeDiskDrive
        public AnalyzeDiskDriveResponse AnalyzeDiskDriveResponse { get; set; }
        public List<Id<LongRunningTaskInfo>> AnalyzeDiskDriveLongRunningTaskIDs { get; set; }
        #endregion

        #region Properties:GetLongRunningTaskState
        public GetLongRunningTaskStateResponse GetLongRunningTaskStateResponse { get; set; }
        public string AnalyzeDiskDriveLongRunningTaskState { get; set; }
        #endregion
    }
}
