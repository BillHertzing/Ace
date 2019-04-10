// Required for the HttpClient
using System.Net.Http;
using System.Threading.Tasks;
using Ace.Agent.BaseServices;
// Required for the logger/logging
//using Blazor.Extensions.Logging;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
// Required for the logger/logging
using Microsoft.Extensions.Logging;

namespace Ace.AceGUI.Pages {
    public class BaseServicesCodeBehind : ComponentBase
  {
    #region Configuration Data
    #endregion

    #region string constants
    // Eventually replace with localization
    #region Configuration Data (string constants)
    public const string labelForRedisCacheConnectionString = "Redis Cache Connection String";
        public const string placeHolderForRedisCacheConnectionString = "Localhost:6xxx";
    public const string labelForMySqlConnectionString = "MySql Connection String";
    public const string placeHolderForMySqlConnectionString = "Localhost:6xxx";
    public const string labelForPostBaseServicesConfigurationDataButton = "Post Base Services Configuration Data";
    public const string labelForGetBaseServicesConfigurationDataButton = "Get Base Services Configuration Data";
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

    #region User data (string constants)
    public const string labelForPostBaseServicesUserDataButton = "Submit";
    public const string labelForGetBaseServicesUserDataButton = "Get";
    #endregion string constants
    #endregion
    #region Access Objects registerd in the DI container
    // This syntax adds to the class a Method that accesses the DI container, and retrieves the instance having the specified type from the DI container.
    // Access the builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type registered in the DI container
    [Inject]
    HttpClient HttpClient
    {
        get;
        set;
    }

    // Access the Logging extensions registered in the DI container
    [Inject]
    public ILogger<BaseServicesCodeBehind> Logger {
        get;
        set;
    }

    //[Inject]
    //protected SessionStorage sessionStorage;

    //[Inject]
    //protected LocalStorage localStorage;

    #endregion

    #region Page Initialization Handler
    protected override async Task OnInitAsync() {
      //Logger.LogDebug($"Starting OnInitAsync");
      ////Logger.LogDebug($"Initializing IServiceClient");
      //IServiceClient client = new JsonHttpClient("http://localhost:21100");
      ////Logger.LogDebug($"client is null: {client == null}");
      BaseServicesInitializationReqPayload baseServicesInitializationReqPayload = new BaseServicesInitializationReqPayload();

        //Logger.LogDebug($"Calling PostJsonAsync<BaseServicesInitializationRspPayload> with baseServicesInitializationReqPayload ={baseServicesInitializationReqPayload}");
        BaseServicesInitializationRspPayload = await HttpClient.PostJsonAsync<BaseServicesInitializationRspPayload>("BaseServicesInitialization",
                                                                                                                    baseServicesInitializationReqPayload);
        //Logger.LogDebug($"Returned from PostJsonAsync<BaseServicesInitializationRspPayload>, BaseServicesInitializationRspPayload = {BaseServicesInitializationRspPayload}");
      //Logger.LogDebug($"Leaving OnInitAsync");
    }
    public BaseServicesInitializationRspPayload BaseServicesInitializationRspPayload {            get;            set;        }

    #endregion

    #region IsAlive
    public async Task IsAlive() {
        //Logger.LogDebug($"Starting IsAlive");
        // Create the payload for the Post
        // ToDo: Validators on the input field will make this better
        // ToDo: wrap in a try catch block and handle errors with a model dialog
        IsAliveReqPayload isAliveReqPayload = new IsAliveReqPayload { };
        //Logger.LogDebug($"Calling PostJsonAsync<isAliveRspPayload> with IsAliveReqPayload = {isAliveReqPayload}");
        IsAliveRspPayload =
await HttpClient.PostJsonAsync<IsAliveRspPayload>("/IsAlive?format=json", isAliveReqPayload);
        //Logger.LogDebug($"Returned from PostJsonAsync<LatLngToAddressRspPayload> with LatLngToAddressRspPayload = {LatLngToAddressRspPayload}");
        //Logger.LogDebug($"Leaving IsAlive");
    }
        public IsAliveRspPayload IsAliveRspPayload {
            get;
            set;
        }
    #endregion

    #region Post and Get Configuration data
    public async Task PostBaseServicesConfigurationData()
    {
      //Logger.LogDebug($"Starting PostBaseServicesConfigurationData");
      // Create the payload for the Post
      // ToDo: Validators on the input field will make this better
      // ToDo: wrap in a try catch block and handle errors with a model dialog
      BaseServicesConfigurationDataReqDTO baseServicesConfigurationDataReqDTO = new BaseServicesConfigurationDataReqDTO
      {RedisCacheConnectionString = this.RedisCacheConnectionString};
      //Logger.LogDebug($"Calling PostJsonAsync<BaseServicesConfigurationDataRspDTO> with BaseServicesConfigurationDataReqDTO = {baseServicesConfigurationDataReqDTO}");
      BaseServicesConfigurationDataRspDTO =
await HttpClient.PostJsonAsync<BaseServicesConfigurationDataRspDTO>("/BaseServicesConfigurationData?format=json", baseServicesConfigurationDataReqDTO);
      //Logger.LogDebug($"Returned from PostJsonAsync<BaseServicesConfigurationDataRspDTO> with BaseServicesConfigurationDataRspDTO = {baseServicesConfigurationDataReqDTO}");
      //Logger.LogDebug($"Leaving PostBaseServicesConfigurationData");
    }
    public async Task GetBaseServicesConfigurationData()
    {
      //Logger.LogDebug($"Starting GetBaseServicesConfigurationData");
      // Create the payload for the Get
      // ToDo: wrap in a try catch block and handle errors with a model dialog
      BaseServicesConfigurationDataReqDTO baseServicesConfigurationDataReqDTO = new BaseServicesConfigurationDataReqDTO      { };
      //Logger.LogDebug($"Calling GetJsonAsync<BaseServicesConfigurationDataRspDTO> with BaseServicesConfigurationDataReqDTO = {baseServicesConfigurationDataReqDTO}");
      BaseServicesConfigurationDataRspDTO =
await HttpClient.GetJsonAsync<BaseServicesConfigurationDataRspDTO>("/BaseServicesConfigurationData?format=json");
      //Logger.LogDebug($"Returned from GetJsonAsync<BaseServicesConfigurationDataRspDTO> with BaseServicesConfigurationDataRspDTO = {baseServicesConfigurationDataReqDTO}");
      RedisCacheConnectionString = BaseServicesConfigurationDataRspDTO.RedisCacheConnectionString;
      MySqlConnectionString = BaseServicesConfigurationDataRspDTO.MySqlConnectionString;
      //Logger.LogDebug($"Leaving GetBaseServicesConfigurationData");
    }
    public BaseServicesConfigurationDataRspDTO BaseServicesConfigurationDataRspDTO { get; set; }
    public string RedisCacheConnectionString { get; set; }
    public string MySqlConnectionString { get; set; }

    #endregion

    #region Post and Get User data
    public async Task PostBaseServicesUserData()
    {
      //Logger.LogDebug($"Starting PostBaseServicesUserData");
      // Create the payload for the Post
      // ToDo: Validators on the input field will make this better
      // ToDo: wrap in a try catch block and handle errors with a model dialog
      BaseServicesUserDataReqPayload baseServicesUserDataReqPayload = new BaseServicesUserDataReqPayload
      { GatewayNameString = this.GatewayNameString, GatewayEntryAPIKeyString = this.GatewayEntryAPIKeyString };
      //Logger.LogDebug($"Calling PostJsonAsync<BaseServicesUserDataRspPayload> with BaseServicesUserDataReqPayload = {baseServicesUserDataReqPayload}");
      BaseServicesUserDataRspPayload = new BaseServicesUserDataRspPayload { GatewayEntryAPIKeyString = this.GatewayEntryAPIKeyString };
await HttpClient.PostJsonAsync<BaseServicesUserDataRspPayload>("/BaseServicesUserData?format=json", baseServicesUserDataReqPayload);
      //Logger.LogDebug($"Returned from PostJsonAsync<BaseServicesUserDataRspPayload> with BaseServicesUserDataRspPayload = {baseServicesUserDataReqPayload}");
      this.GatewayNameString = BaseServicesUserDataRspPayload.GatewayNameString;
      this.GatewayEntryAPIKeyString = BaseServicesUserDataRspPayload.GatewayEntryAPIKeyString;
      //Logger.LogDebug($"Leaving PostBaseServicesUserData");
    }
    public BaseServicesUserDataRspPayload BaseServicesUserDataRspPayload { get; set; }
    public string GatewayNameString { get; set; }
    public string GatewayEntryAPIKeyString { get; set; }

    #endregion

    #region Lat/Lng to Address and Address to Lat/Lng
    public async Task LatLngToAddress() {
        //Logger.LogDebug($"Starting LatLngToAddress");
        // Create the payload for the Post
        // ToDo: Validators on the input field will make this better
        // ToDo: wrap in a try catch block and handle errors with a model dialog
        LatLngToAddressReqPayload latLngToAddressReqPayload = new LatLngToAddressReqPayload { Latitude = this.Latitude
            , Longitude = this.Longitude
        };
        //Logger.LogDebug($"Calling PostJsonAsync<LatLngToAddressRspPayload> with LatLngToAddressReqPayload = {latLngToAddressReqPayload}");
        LatLngToAddressRspPayload =
await HttpClient.PostJsonAsync<LatLngToAddressRspPayload>("/LatLngToAddress?format=json", latLngToAddressReqPayload);
        //Logger.LogDebug($"Returned from PostJsonAsync<LatLngToAddressRspPayload> with LatLngToAddressRspPayload = {LatLngToAddressRspPayload}");
        //Logger.LogDebug($"Leaving LatLngToAddress");
    }

        public async Task AddressToLatLng() {
            //Logger.LogDebug($"Starting AddressToLatLng");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            AddressToLatLngReqPayload addressToLatLngReqPayload = new AddressToLatLngReqPayload { Address = this.Address
            };
            //Logger.LogDebug($"Calling PostJsonAsync<AddressToLatLngRspPayload> with AddressToLatLngReqPayload = {addressToLatLngReqPayload}");
            AddressToLatLngRspPayload =
await HttpClient.PostJsonAsync<AddressToLatLngRspPayload>("/AddressToLatLng?format=json", addressToLatLngReqPayload);
            //Logger.LogDebug($"Returned from PostJsonAsync<AddressToLatLngRspPayload> with AddressToLatLngRspPayload = {AddressToLatLngRspPayload.ToString()}");
            Latitude = AddressToLatLngRspPayload.Latitude;
            Longitude = AddressToLatLngRspPayload.Longitude;
            //Logger.LogDebug($"Leaving AddressToLatLng");
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

  }
}
