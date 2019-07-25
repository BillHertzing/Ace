// Required for the HttpClient
using System.Net.Http;
using System.Threading.Tasks;
using Ace.Agent.BaseServices;
// Required for Blazor
using Microsoft.AspNetCore.Components;
// Required for Browser Console Logging
using Microsoft.Extensions.Logging;
using Blazor.Extensions.Logging;
// Required for Blazor LocalStorage
// Required for ComputerInventory used in BaseServices
using ServiceStack.Text;
using ATAP.Utilities.GeoLocationData;
using Ace.AceGUI.HttpClientExtenssions;

//using Stateless;

namespace Ace.AceGUI.Pages {
    public partial class BaseServicesCodeBehind : ComponentBase {

        #region StringConstants
        // Eventually replace with localization
        #region StringConstants:GatewayServicedData
        public const string labelForLatitude = "Latitude";
        public const string labelForLongitude = "Longitude";
        public const string labelForAddress = "Address";
        public const string labelForAddressToLatLngButton = "Click to convert the Address to a Latitude and Longitude";
        public const string labelForLatLngToAddressButton = "Click to convert the Latitude and Longitude to an Address ";
        public const string latitudePlaceHolder = "Enter +- decimal number";
        public const string longitudePlaceHolder = "Enter +- decimal number";
        public const string addressPlaceHolder = "Enter full street address";
        #endregion

        #region StringConstants:Gateway Definitions
        public const string labelForGatewayNameString = "Gateway Name";
        public const string placeHolderForGatewayNameString = "Enter the name of a Gateway to use for Lat/Lng to Address translation";
        public const string labelForGatewayEntryAPIKeyString = "API Key for Lat/Lng to Address translation";
        public const string placeHolderForGatewayEntryAPIKeyString = "Enter the API Key needed to use the Lat/Lng to Address translation service";
        #endregion

        #endregion

        #region GeoLocation Initialization Handler
        protected async Task InitGeoLocationDataAsync() {
            Log.LogDebug($"Starting BaseServices.InitGeoLocationDataAsync");
            // ToDo: analyze code paths to be sure there is no way this can be called before local storage is initialized
            // IsInitialized=await LStorage.GetItemAsync<bool>("BaseServices.IsInitialized");
            // ToDo: test and throw an error if local storage is not yet initialized
            // if (!IsInitialized) {}

            // initialize BaseServices.InitGeoLocationData property with data from local Storage
            GeoLocationData=await LStorage.GetItemAsync<GeoLocationData>("BaseServices.GeoLocationData");

            Log.LogDebug($"Leaving BaseServices.InitGeoLocationDataAsync");
        }

        #endregion

        #region Lat/Lng to Address and Address to Lat/Lng
        public async Task GetAddressFromLatLong() {
            Log.LogDebug($"Starting GetAddressFromLatLong");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            var geoLocationData = new GeoLocationData(GeoLocationData.Latitude, GeoLocationData.Longitude, string.Empty);
            var getAddressFromLatLongRequest = new GetAddressFromLatLongRequest(geoLocationData);
            Log.LogDebug($"Calling PostJsonAsyncSS<GetAddressFromLatLongResponse> with getAddressFromLatLongRequest = {getAddressFromLatLongRequest.Dump()}");
            var getAddressFromLatLongResponse=
      await HttpClient.PostJsonAsyncSS<GetAddressFromLatLongResponse>("/GetAddressFromLatLong", getAddressFromLatLongRequest);
            Log.LogDebug($"Returned from PostJsonAsyncSS<GetAddressFromLatLongResponse> with getAddressFromLatLongResponse = {getAddressFromLatLongResponse.Dump()}");
            var longRunningTaskStartupInfo = getAddressFromLatLongResponse.LongRunningTaskStartupInfo;

            Log.LogDebug($"Leaving GetAddressFromLatLongResponse");
        }

        public async Task GetLatLongFromAddress() {
            Log.LogDebug($"Starting async GetLatLongFromAddress");
            // Create the payload for the Post

            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            var getLatLongFromAddressRequest = new GetLatLongFromAddressRequest(new GeoLocationData(decimal.Zero, decimal.Zero, GeoLocationData.Address));
            Log.LogDebug($"Calling PostJsonAsyncSS<GetLatLongFromAddressResponse> with getLatLongFromAddressRequest = {getLatLongFromAddressRequest.Dump()}");
            var longRunningTaskStartupInfo =
      await HttpClient.PostJsonAsyncSS<GetLatLongFromAddressResponse>("/GetLatLongFromAddress", getLatLongFromAddressRequest);
            Log.LogDebug($"Returned from PostJsonAsyncSS<GetLatLongFromAddressResponse> with longRunningTaskStartupInfo = {longRunningTaskStartupInfo.Dump()}");
            //Latitude=AddressToLatLngRspPayload.Latitude;
            //Longitude=AddressToLatLngRspPayload.Longitude;
            Log.LogDebug($"Leaving GetLatLongFromAddress");
        }


        #endregion

        #region Properties

        #region Properties:GeoLocationData
        public GeoLocationData GeoLocationData { get; set; }
        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Address { get; set; }

        public GetLatLongFromAddressResponse AddressToLatLngRspPayload { get; set; }

        public GetAddressFromLatLongResponse LatLngToAddressRspPayload {
            get;
            set;
        }
        #endregion
        #endregion

    }
}
