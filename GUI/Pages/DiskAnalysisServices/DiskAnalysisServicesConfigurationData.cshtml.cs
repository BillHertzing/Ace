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

namespace Ace.AceGUI.Pages
{
    public partial class DiskAnalysisServicesCodeBehind : ComponentBase
    {
        // ToDo: Eventually replace with localization

        public const string labelForConfigurationDataBlockSize = "File Read Block Size";
        public const string labelForConfigurationDataSave = "save the DiskAnalysis Configuration Data?";

        // Add constant structures for configuration data and user data to be used when the GUI is displayed before it can initialize with the agent
        // Eventually localized
        public static Ace.Agent.DiskAnalysisServices.ConfigurationData configurationDataPlaceholder = new Ace.Agent.DiskAnalysisServices.ConfigurationData(-1);

        public async Task SetConfigurationData()
        {
            //Logger.LogDebug($"Starting SetConfigurationData");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            SetConfigurationDataRequest setConfigurationDataRequest = new SetConfigurationDataRequest() { ConfigurationData = ConfigurationData, ConfigurationDataSave = ConfigurationDataSave };
            //Logger.LogDebug($"Calling GetJsonAsync<SetConfigurationDataResponse> with SetConfigurationDataRequest = {setConfigurationDataRequest}");
            SetConfigurationDataResponse setConfigurationDataResponse =
      await HttpClient.PostJsonAsync<SetConfigurationDataResponse>("/SetConfigurationData?format=json",
                                                                                           setConfigurationDataRequest);
            //Logger.LogDebug($"Returned from GetJsonAsync<SetConfigurationDataResponse> with setConfigurationDataResponse = {setConfigurationDataResponse}");
            //Logger.LogDebug($"Leaving SetConfigurationData");
        }



        #region Properties

        #region Properties:ConfigurationData
        public Ace.Agent.DiskAnalysisServices.ConfigurationData ConfigurationData { get; set; } = configurationDataPlaceholder;

        public bool ConfigurationDataSave { get; set; }

        #endregion Properties:ConfigurationData

        #endregion
    }
}
