// Required for the HttpClient
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ace.Agent.BaseServices;
// Required for Blazor
using Microsoft.AspNetCore.Components;
// Required for Browser Console Logging
using Microsoft.Extensions.Logging;
using Blazor.Extensions.Logging;
// Required for Blazor LocalStorage
using Blazored.LocalStorage;
// Required for ComputerInventory used in BaseServices
using ATAP.Utilities.DiskDrive;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.LongRunningTasks;
using ServiceStack.Text;
using System.Collections.Generic;
using System.Diagnostics;
using Ace.AceGUI.HttpClientExtenssions;
using System.Text;
using ATAP.Utilities.ConcurrentObservableCollections;

//using Stateless;

namespace Ace.AceGUI.Pages {
	// Use the Custom BlazorLog Attribute to specify logging of all method boundaries via the BlazorLogProvider class
    [Ace.GUI.LogMethodBoundariesInBlazor]
    public partial class BaseServicesCodeBehind : ComponentBase {

        #region Access Objects registered in the DI container
        // This syntax adds to the class a Method that accesses the DI container, and retrieves the instance having the specified type from the DI container.
        // Access the builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type registered in the DI container
        [Inject]
        HttpClient HttpClient { get; set; }

        // Access the Logging extensions registered in the DI container
        [Inject]
        public ILoggerFactory LoggerFactory { get; set; }

        [Inject]
        public ILogger<BaseServicesCodeBehind> Log { get; set; }

        // Access the LocalStorage extensions registered in the DI container
        [Inject]
        public ILocalStorageService LStorage { get; set; }

        // Access the IUriHelper registered in the DI container
        //[Inject]
        //IUriHelper UriHelper { get; set; }

        // Access the INavigationManager registered in the DI container
        [Inject] 
        NavigationManager NavigationManager { get; set; }

        private ILogger<BaseServicesCodeBehind> Logger { get; set; }

        #endregion
        #region Testing
        // private method to test SS Serializer for 
        private void TestIDTypeSerialization() {

            // Testing for serialization of ID<T> instances
            // Temporary testing
            Log.LogDebug($"Temporary Testing");
            var Id = new Id<LongRunningTaskInfo>(Guid.NewGuid());
            Log.LogDebug($"Id = {Id.Dump()}");
            var IdStr = ServiceStack.Text.JsonSerializer.SerializeToString(Id);
            Log.LogDebug($"IdStr from ServiceStack.Text.JsonSerializer.SerializeToString(Id)= {IdStr.Dump()}");
            var roundTripId = ServiceStack.Text.JsonSerializer.DeserializeFromString<Id<LongRunningTaskInfo>>(IdStr);
            Log.LogDebug($"roundTripId from ServiceStack.Text.JsonSerializer.DeserializeFromString<Id<LongRunningTaskInfo>>(IdStr) = {roundTripId.Dump()}");
            IdStr=Newtonsoft.Json.JsonConvert.SerializeObject(Id);
            Log.LogDebug($"IdStr from Newtonsoft.Json.JsonConvert.SerializeObject(Id) = {IdStr.Dump()}");
            roundTripId=Newtonsoft.Json.JsonConvert.DeserializeObject<Id<LongRunningTaskInfo>>(IdStr);
            Log.LogDebug($"roundTripId from Newtonsoft.Json.JsonConvert.DeserializeObject<Id<LongRunningTaskInfo>> = {roundTripId.Dump()}");
        }
        private  void TestComplexTypeSerialization() {
            var cData = new Ace.Agent.BaseServices.ConfigurationData();
            Log.LogDebug($"cData = {cData.Dump()}");
            var cDataStr = ServiceStack.Text.JsonSerializer.SerializeToString(cData);
            Log.LogDebug($"cDataStr from ServiceStack.Text.JsonSerializer.SerializeToString(Id)= {cDataStr}");
            // ToDo: ask SS to Fix SS Deserializer in SS V5.5.1+
            //var roundTripcData = ServiceStack.Text.JsonSerializer.DeserializeFromString<Ace.Agent.BaseServices.ConfigurationData>(cDataStr);
            //Log.LogDebug($"roundTripcData from ServiceStack.Text.JsonSerializer.DeserializeFromString<Ace.Agent.BaseServices.ConfigurationData>(cDataStr) = {roundTripcData.Dump()}");
            Log.LogDebug($"End Temporary Testing");
        }
        private void TestLocalStorageSSExtensions() {

        }
        #endregion
        #region Page Initialization Handler
        protected override async Task OnInitializedAsync()  {
            //Logger=LoggerFactory.CreateLogger<BaseServicesCodeBehind>();
            //Logger.LogDebug($"Starting BaseServices.OnInitAsync, LoggerFactory");
            Log.LogDebug($"Starting BaseServices.OnInitAsync");

            //// Testing for serialization of ID<T> instances
            //TestIDTypeSerialization();
            //// Testing for serialization of complex data types
            //TestComplexTypeSerialization();
            //Log.LogDebug($"IsInitialized 1: {IsInitialized.Dump()}");
            IsInitialized=await LStorage.GetItemAsync<bool>("BaseServices.IsInitialized");
            //Log.LogDebug($"IsInitialized 2: {IsInitialized.Dump()}");
            //Log.LogDebug($"LastInitialized 1: {LastInitialized.Dump()}");
            LastInitialized=await LStorage.GetItemAsync<DateTime>("BaseServices.LastInitialized");
            //Log.LogDebug($"LastInitialized 2: {LastInitialized.Dump()}");
            // if the key is not found, the bool value will be false (default)
            if (!IsInitialized) {
                // Calling server for BaseServices initialization data 
                ////Log.LogDebug($"Initializing IServiceClient");
                ////IServiceClient client = new JsonHttpClient("http://localhost:21100");
                ////Log.LogDebug($"client is null: {client == null}");
                /* // from the stateless statemachine compiler project
                const string on = "On";
                const string off = "Off";
                const char space = ' ';
                var onOffSwitch = new StateMachine<string, char>(off);
                onOffSwitch.Configure(off).Permit(space, on);
                onOffSwitch.Configure(on).Permit(space, off);
                */
                var initializationRequest = new InitializationRequest(new InitializationRequestPayload(new InitializationData("BaseVersionXX", "MachineIDXX", "userIDxx")));
                Log.LogDebug($"initializationRequest: {initializationRequest.Dump()}");

                Log.LogDebug($"NavigationManager.ToAbsoluteUri(BaseServicesInitialization).ToString(): {NavigationManager.ToAbsoluteUri("BaseServicesInitialization").ToString()}");
                //Log.LogDebug($"UriHelper.ToAbsoluteUri(BaseServicesInitialization).ToString(): {UriHelper.ToAbsoluteUri("BaseServicesInitialization").ToString()}");

                Log.LogDebug($"Calling PostJsonAsync<InitializationResponse> with initializationRequest = {initializationRequest.Dump()}");
                var initializationResponse = await HttpClient.PostJsonAsync<InitializationResponse>(NavigationManager.ToAbsoluteUri("BaseServicesInitialization").ToString(), initializationRequest); // use the DOT NET CORE Blazor Built-In JSON serializer library
                //var initializationResponse = await HttpClient.PostJsonAsync<InitializationResponse>(UriHelper.ToAbsoluteUri("BaseServicesInitialization").ToString(), initializationRequest); // use the DOT NET CORE Blazor Built-In JSON serializer library
                //var initializationResponse = await HttpClient.PostJsonAsyncSS<InitializationResponse>(UriHelper.ToAbsoluteUri("BaseServicesInitialization").ToString(), initializationRequest); // use the ServiceStack JSON serializer library
                Log.LogDebug($"Returned from PostJsonAsync<InitializationResponse>, initializationResponse = {initializationResponse.Dump()}");
                //Log.LogDebug($"Returned from PostJsonAsyncSS<InitializationResponse>, initializationResponse = {initializationResponse.Dump()}");
                await LStorage.SetItemAsync("BaseServices.IsInitialized", true);
                await LStorage.SetItemAsync("BaseServices.LastInitialized", DateTime.Now);
                // Initialize Local Storage with data structures for BaseServices Configuration and User Data
                await LStorage.SetItemAsync("BaseServices.ConfigurationData", initializationResponse.InitializationResponsePayload.ConfigurationData);
                await LStorage.SetItemAsync("BaseServices.UserData", initializationResponse.InitializationResponsePayload.UserData);

                // ToDo: Move to LongRunningTasks initialization compilation unit
                // Testing for serialization of ID<T> instances
                var tID = new Id<LongRunningTaskInfo>();
                Log.LogDebug($"tID: {tID.Dump()}");
                var tLRTS = new LongRunningTaskStatus(tID,new TaskStatus(),0);
                var D1 = new Dictionary<Id<LongRunningTaskInfo>, LongRunningTaskStatus>();
                D1.Add(tID, tLRTS);
                Log.LogDebug($"D1: {D1.Dump()}");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                await LStorage.SetItemAsync("BaseServices.D1", D1);
                stopwatch.Stop();
                Log.LogDebug($"SetItemAsync took {stopwatch.ElapsedMilliseconds} milliseconds");
                stopwatch.Restart();
                var D2 = await LStorage.GetItemAsync<Dictionary<Id<LongRunningTaskInfo>, LongRunningTaskStatus>>("BaseServices.D1");
                stopwatch.Stop();
                Log.LogDebug($"GetItemAsync took {stopwatch.ElapsedMilliseconds} milliseconds");
                Log.LogDebug($"D2: {D2.Dump()}");

                // Initialize Local Storage with data structures for LongRunningTasks
                var longRunningTasksCOD = new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskStatus>();
       
                Log.LogDebug($"tID: {tID.Dump()}");
                
                longRunningTasksCOD.Add(tID, tLRTS);
                Log.LogDebug($"longRunningTasksCOD: {longRunningTasksCOD.Dump()}");
                await LStorage.SetItemAsync("BaseServices.LongRunningTasksCOD", longRunningTasksCOD);
                var longRunningTasksCOD2 = await LStorage.GetItemAsync<ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskStatus>>("BaseServices.LongRunningTasksCOD");
                Log.LogDebug($"longRunningTasksCOD2: {longRunningTasksCOD2.Dump()}");

                // ToDo: Move to diskanalysis initialization compilation unit
                // Initialize Local Storage with data structures for DiskDriveAndFileSystemAnalysis
                await LStorage.SetItemAsync("DiskAnalysisServices.PartitionInfoExs", new PartitionInfoExs());

                IsInitialized=await LStorage.GetItemAsync<bool>("BaseServices.IsInitialized");
            }

            // initialize this page's Properties with data from local Storage
            LastInitialized=await LStorage.GetItemAsync<DateTime>("BaseServices.LastInitialized");
            // Properties related to ConfigurationData and UserData initialization
            await InitConfigurationDataAsync();
            // Properties related to LongRunningTasks initialization
            await InitLongRunningTasksStatusAsync();
            // Properties related to GeoLocation initialization
            await InitGeoLocationDataAsync();

            //ToDo: move to DiskAnalysis CSB feature initialization
            PartitionInfoExs=await LStorage.GetItemAsync<PartitionInfoExs>("DiskAnalysisServices.PartitionInfoExs");
            testint =await LStorage.GetItemAsync<int>("testint");
            testint++;
            await LStorage.SetItemAsync("testint", testint);
            Log.LogDebug($"Leaving BaseServices.OnInitAsync");
        }
        #endregion

        #region IsAlive
        public async Task IsAlive() {
            Log.LogDebug($"Starting IsAlive");
            // Create the payload for the Post
            // ToDo: Validators on the input field will make this better
            // ToDo: wrap in a try catch block and handle errors with a model dialog
            IsAliveReqPayload isAliveReqPayload = new IsAliveReqPayload { };
            Log.LogDebug($"Calling PostJsonAsyncSS<isAliveRspPayload> with IsAliveReqPayload = {isAliveReqPayload}");
            var isAliveRspPayload=
              await HttpClient.PostJsonAsyncSS<IsAliveRspPayload>(NavigationManager.ToAbsoluteUri("IsAlive").ToString(), isAliveReqPayload);
              //await HttpClient.PostJsonAsyncSS<IsAliveRspPayload>(UriHelper.ToAbsoluteUri("IsAlive").ToString(), isAliveReqPayload);
            Log.LogDebug($"Returned from PostJsonAsyncSS<isAliveRspPayload> with isAliveRspPayload = {isAliveRspPayload}");
            Log.LogDebug($"Leaving IsAlive");
        }
 
        #endregion

        #region Properties
        #region Properties:InitializationData
        public DateTime LastInitialized { get; set; }
        public bool IsInitialized { get; set; }
        public InitializationResponse InitializationResponse { get; set; }
        public InitializationResponsePayload InitializationResponsePayload { get; set; }
        #endregion

        #region Properties:PartitionInfoExs
        public PartitionInfoExs PartitionInfoExs { get; set; }
        #endregion

        #endregion

    }
}