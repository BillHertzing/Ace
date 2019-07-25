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
using Ace.AceGUI.HttpClientExtenssions;
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

        #region StringConstants:UserData
        public const string labelForPostBaseServicesUserDataButton = "Submit";
        public const string labelForGetBaseServicesUserDataButton = "Get";
        #endregion
        #endregion


        #region ConfigurationData UserData Initialization Handler
        protected async Task InitConfigurationDataAsync() {
            Log.LogDebug($"Starting BaseServices.InitConfigurationDataAsync");
            // ToDo: analyze code paths to be sure there is no way this can be called before local storage is initialized
            // IsInitialized=await LStorage.GetItemAsync<bool>("BaseServices.IsInitialized");
            // ToDo: test and throw an error if local storage is not yet initialized
            // if (!IsInitialized) {}

            // initialize BaseServices.ConfigurationData property with data from local Storage
            ConfigurationData=await LStorage.GetItemAsync<ConfigurationData>("BaseServices.ConfigurationData");
            // ToDo: maybe move to a BaseServices.UserData compilation unit
            UserData=await LStorage.GetItemAsync<UserData>("BaseServices.UserData");

            Log.LogDebug($"Leaving BaseServices.InitConfigurationDataAsync");
        }
        #endregion

        #region PostGetBaseServicesConfigurationData 
        public async Task PostGetBaseServicesConfigurationData(int placeholder) {
            Log.LogDebug($"Starting PostGetBaseServicesConfigurationData");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            GetConfigurationDataRequest getConfigurationDataRequest = new GetConfigurationDataRequest();
            //Log.LogDebug($"Calling PostJsonAsyncSS<GetConfigurationDataResponse> with GetConfigurationDataRequest = {getConfigurationDataRequest}");
            GetConfigurationDataResponse getConfigurationDataResponse =
      await HttpClient.PostJsonAsyncSS<GetConfigurationDataResponse>("/GetBaseServicesConfigurationData", getConfigurationDataRequest);
            //Log.LogDebug($"Returned from PostJsonAsyncSS<GetConfigurationDataResponse> with GetConfigurationDataResponse = {GetConfigurationDataResponse}");
            ConfigurationData=getConfigurationDataResponse.ConfigurationData;
            Log.LogDebug($"Leaving PostGetBaseServicesConfigurationData");
        }
        #endregion

        #region PostGetBaseServicesUserData
        public async Task PostGetBaseServicesUserData(int placeholder) {
            //Log.LogDebug($"Starting PostGetBaseServicesUserData");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            GetUserDataRequest getUserDataRequest = new GetUserDataRequest();
            //Log.LogDebug($"Calling PostJsonAsyncSS<GetUserDataResponse> with getUserDataRequest = {getUserDataRequest}");
            GetUserDataResponse getUserDataResponse =
      await HttpClient.PostJsonAsyncSS<GetUserDataResponse>("/GetBaseServicesUserData", getUserDataRequest);
            //Log.LogDebug($"Returned from PostJsonAsyncSS<GetUserDataResponse> with GetUserDataResponse = {getUserDataResponse}");
            UserData=getUserDataResponse.UserData;
            //Log.LogDebug($"Leaving PostGetBaseServicesUserData");
        }
        #endregion


        #region Properties
        #region Properties:ConfigurationData
        public ConfigurationData ConfigurationData { get; set; }
        #endregion
        #region Properties:UserData
        public UserData UserData { get; set; }
        #endregion
        #endregion
        
        public int testint { get; set; }

    }
}