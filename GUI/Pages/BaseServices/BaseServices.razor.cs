// Required for the HttpClient
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ace.Agent.BaseServices;
// Required for the Log/logging
//using Blazor.Extensions.Logging;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
// Required for the Log/logging
using Microsoft.Extensions.Logging;
// Required for ComputerInventory used in BaseServices
using ATAP.Utilities.DiskDrive;
using Swordfish.NET.Collections;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.LongRunningTasks;

//using Stateless;

namespace Ace.AceGUI.Pages {
    public partial class BaseServicesCodeBehind : ComponentBase {

        #region StringConstants
        // Eventually replace with localization
        #region StringConstants:ConfigurationData
        public const string labelForRedisCacheConnectionString = "Redis Cache Connection String";
        public const string placeHolderForRedisCacheConnectionString = "Localhost:6xxx";
        public const string labelForMySqlConnectionString = "MySql Connection String";
        public const string placeHolderForMySqlConnectionString = "Localhost:6xxx";
        public const string labelForPostGetBaseServicesConfigurationData = "Get Base Services Configuration Data";
        #endregion

        #region Gateway serviced data (string constants)
        public const string labelForLatitude = "Latitude";
        public const string labelForLongitude = "Longitude";
        public const string labelForAddress = "Address";
        public const string labelForAddressToLatLngButton = "Click to convert the Address to a Latitude and Longitude";
        public const string labelForLatLngToAddressButton = "Click to convert the Latitude and Longitude to an Address ";
        public const string latitudePlaceHolder = "Enter +- decimal number";
        public const string longitudePlaceHolder = "Enter +- decimal number";
        public const string addressPlaceHolder = "Enter full street address";
        #endregion

        #region Gateway definitions
        public const string labelForGatewayNameString = "Gateway Name";
        public const string placeHolderForGatewayNameString = "Enter the name of a Gateway to use for Lat/Lng to Address translation";
        public const string labelForGatewayEntryAPIKeyString = "API Key for Lat/Lng to Address translation";
        public const string placeHolderForGatewayEntryAPIKeyString = "Enter the API Key needed to use the Lat/Lng to Address translation service";
        #endregion Gateway definitions

        #region StringConstants:TaskStatus
        //public const string labelForGetLongRunningTasksStatusButton = "Press to Get Task Status";
        #endregion

        #region User data (string constants)
        public const string labelForPostBaseServicesUserDataButton = "Submit";
        public const string labelForGetBaseServicesUserDataButton = "Get";
        #endregion string constants
        #endregion

        #region Access Objects registered in the DI container
        // This syntax adds to the class a Method that accesses the DI container, and retrieves the instance having the specified type from the DI container.
        // Access the builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type registered in the DI container
        [Inject]
        HttpClient HttpClient { get; set; }

        // Access the Logging extensions registered in the DI container
        //[Inject]
        //public ILogger<BaseServicesCodeBehind> Log { get; set; }

        //[Inject]
        //protected SessionStorage sessionStorage;

        //[Inject]
        //protected LocalStorage localStorage;

        #endregion

        #region Page Initialization Handler
        protected override async Task OnInitAsync() {
            //Log.LogDebug($"Starting OnInitAsync");
            ////Log.LogDebug($"Initializing IServiceClient");
            ////IServiceClient client = new JsonHttpClient("http://localhost:21100");
            ////Log.LogDebug($"client is null: {client == null}");
            /* // from teh stateless statemachine compiler project
            const string on = "On";
            const string off = "Off";
            const char space = ' ';
            var onOffSwitch = new StateMachine<string, char>(off);
            onOffSwitch.Configure(off).Permit(space, on);
            onOffSwitch.Configure(on).Permit(space, off);
            */
            var initializationRequest = new InitializationRequest(new InitializationRequestPayload(new InitializationData("BaseVersionXX", "MachineIDXX","userIDxx")));
            //Log.LogDebug($"Calling PostJsonAsync<InitializationResponse> with InitializationRequest = {InitializationRequest}");
            InitializationResponse = await HttpClient.PostJsonAsync<InitializationResponse>("/BaseServicesInitialization",
                                                                                                                        initializationRequest);
            //Log.LogDebug($"Returned from PostJsonAsync<InitializationResponse>, InitializationResponse = {InitializationResponse}");
            ConfigurationData=InitializationResponse.InitializationResponsePayload.ConfigurationData;
            UserData=InitializationResponse.InitializationResponsePayload.UserData;
            PartitionInfoExs=new PartitionInfoExs();
            LongRunningTasksCOD=new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskStatus>();
            //Log.LogDebug($"Leaving OnInitAsync");
        }
        #endregion

        #region IsAlive
        public async Task IsAlive() {
            //Log.LogDebug($"Starting IsAlive");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            IsAliveReqPayload isAliveReqPayload = new IsAliveReqPayload { };
            //Log.LogDebug($"Calling PostJsonAsync<isAliveRspPayload> with IsAliveReqPayload = {isAliveReqPayload}");
            IsAliveRspPayload=
      await HttpClient.PostJsonAsync<IsAliveRspPayload>("/IsAlive?format=json", isAliveReqPayload);
            //Log.LogDebug($"Returned from PostJsonAsync<LatLngToAddressRspPayload> with LatLngToAddressRspPayload = {LatLngToAddressRspPayload}");
            //Log.LogDebug($"Leaving IsAlive");
        }
        public IsAliveRspPayload IsAliveRspPayload {
            get;
            set;
        }
        #endregion

        #region PostGetBaseServicesConfigurationData 
        public async Task PostGetBaseServicesConfigurationData(int placeholder) {
            // Log.LogDebug($"Starting PostGetBaseServicesConfigurationData");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            GetConfigurationDataRequest getConfigurationDataRequest = new GetConfigurationDataRequest();
            //Log.LogDebug($"Calling PostJsonAsync<GetConfigurationDataResponse> with GetConfigurationDataRequest = {getConfigurationDataRequest}");
            GetConfigurationDataResponse getConfigurationDataResponse =
      await HttpClient.PostJsonAsync<GetConfigurationDataResponse>("/GetBaseServicesConfigurationData", getConfigurationDataRequest);
            //Log.LogDebug($"Returned from PostJsonAsync<GetConfigurationDataResponse> with GetConfigurationDataResponse = {GetConfigurationDataResponse}");
            ConfigurationData=getConfigurationDataResponse.ConfigurationData;
            //Log.LogDebug($"Leaving PostGetBaseServicesConfigurationData");
        }
        #endregion

        #region PostGetBaseServicesUserData
        public async Task PostGetBaseServicesUserData(int placeholder) {
            //Log.LogDebug($"Starting PostGetBaseServicesUserData");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            GetUserDataRequest getUserDataRequest = new GetUserDataRequest();
            //Log.LogDebug($"Calling PostJsonAsync<GetUserDataResponse> with getUserDataRequest = {getUserDataRequest}");
            GetUserDataResponse getUserDataResponse =
      await HttpClient.PostJsonAsync<GetUserDataResponse>("/GetBaseServicesUserData", getUserDataRequest);
            //Log.LogDebug($"Returned from PostJsonAsync<GetUserDataResponse> with GetUserDataResponse = {getUserDataResponse}");
            UserData=getUserDataResponse.UserData;
            //Log.LogDebug($"Leaving PostGetBaseServicesUserData");
        }
        #endregion

        #region Lat/Lng to Address and Address to Lat/Lng
        public async Task LatLngToAddress() {
            //Log.LogDebug($"Starting LatLngToAddress");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            LatLngToAddressReqPayload latLngToAddressReqPayload = new LatLngToAddressReqPayload {
                Latitude=this.Latitude
                ,
                Longitude=this.Longitude
            };
            //Log.LogDebug($"Calling PostJsonAsync<LatLngToAddressRspPayload> with LatLngToAddressReqPayload = {latLngToAddressReqPayload}");
            LatLngToAddressRspPayload=
      await HttpClient.PostJsonAsync<LatLngToAddressRspPayload>("/LatLngToAddress?format=json", latLngToAddressReqPayload);
            //Log.LogDebug($"Returned from PostJsonAsync<LatLngToAddressRspPayload> with LatLngToAddressRspPayload = {LatLngToAddressRspPayload}");
            //Log.LogDebug($"Leaving LatLngToAddress");
        }

        public async Task AddressToLatLng() {
            //Log.LogDebug($"Starting AddressToLatLng");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            AddressToLatLngReqPayload addressToLatLngReqPayload = new AddressToLatLngReqPayload {
                Address=this.Address
            };
            //Log.LogDebug($"Calling PostJsonAsync<AddressToLatLngRspPayload> with AddressToLatLngReqPayload = {addressToLatLngReqPayload}");
            AddressToLatLngRspPayload=
      await HttpClient.PostJsonAsync<AddressToLatLngRspPayload>("/AddressToLatLng?format=json", addressToLatLngReqPayload);
            //Log.LogDebug($"Returned from PostJsonAsync<AddressToLatLngRspPayload> with AddressToLatLngRspPayload = {AddressToLatLngRspPayload.ToString()}");
            Latitude=AddressToLatLngRspPayload.Latitude;
            Longitude=AddressToLatLngRspPayload.Longitude;
            //Log.LogDebug($"Leaving AddressToLatLng");
        }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Address { get; set; }

        public AddressToLatLngRspPayload AddressToLatLngRspPayload { get; set; }

        public LatLngToAddressRspPayload LatLngToAddressRspPayload {
            get;
            set;
        }
        #endregion

        #region Properties
        #region Properties:ConfigurationData
        public ConfigurationData ConfigurationData { get; set; }
        #endregion
        #region Properties:UserData
        public UserData UserData { get; set; }
        #endregion
        #region Properties:InitializationData
        public InitializationResponse InitializationResponse { get; set; }
        public InitializationResponsePayload InitializationResponsePayload { get; set; }
        #endregion
        #region Properties:PartitionInfoExs
        public PartitionInfoExs PartitionInfoExs { get; set; }
        #endregion
        #region Properties:LongRunningTasks
        //public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo> LongRunningTasksCOD { get; set; }
        #endregion
        #endregion

    }
}
