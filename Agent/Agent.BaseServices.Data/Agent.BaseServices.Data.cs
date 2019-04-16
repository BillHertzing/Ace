using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using ATAP.Utilities.Http;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Logging;
using ServiceStack.Redis;
using Swordfish.NET.Collections;
using Polly;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace Ace.Agent.BaseServices {
    public partial class BaseServicesData : IDisposable {
        // Constructor with just AppHost parameter
        public BaseServicesData(IAppHost appHost) {
            Log.Debug("Entering BaseServicesData .ctor");
            Container=appHost.GetContainer();

            // Blazor requires the delivery of static files ending in certain file suffixs.
            // SS disallows many of these by default, so here we tell SS to allow certain file suffixs
            appHost.Config.AllowFileExtensions.Add("dll");
            appHost.Config.AllowFileExtensions.Add("json");
            appHost.Config.AllowFileExtensions.Add("pdb");

            // If the Redis configuration key exists, register Redis as a name:value pair cache
            if (appHost.AppSettings.Exists(configKeyPrefix+configKeyRedisConnectionString)) {
                var redisConnectionString = appHost.AppSettings.GetString(configKeyPrefix+configKeyRedisConnectionString);
                Container.Register<IRedisClientsManager>(c => new RedisManagerPool(redisConnectionString));
                Container.Register(c => c.Resolve<IRedisClientsManager>().GetCacheClient());
                cacheClient=Container.Resolve<ICacheClient>();
            } else {
                throw new NotImplementedException(RedisConnectionStringKeyNotFoundExceptionMessage);
            }

            // Ensure that the cacheClient is running
            try {
                var test = cacheClient.GetKeysStartingWith(configKeyPrefix);
            }
            catch (ServiceStack.Redis.RedisException Ex) {
                if (Ex.InnerException.Message.Contains(ListeningServiceNotRunningInnerExceptionMessage)) {
                    throw new Exception($"In BaseServicesData .ctor: {RedisNotRunningExceptionMessage} on {Ex.Message}", Ex);
                }
            }

            // ToDo: Validate the cache and the appSettings Config keys/values for the BaseService align

            // Key names in the cache for this namespace's configuration settings consist of this namespace and the string .Config
            // This prefix is available as a static method on this class
            var cacheConfigKeysForThisNamespace = cacheClient.GetKeysStartingWith(configKeyPrefix);
            //var appSettingsConfigKeysForThisNamespace = appHost.AppSettings.GetAllKeys().Select(x => x.IndexOf(configKeyPrefix) >= 0? x: configKeyPrefix + x);
            //ToDo: record any discrepencies, and report them in a log, and also report to the GUI when BaseServices get initialized.

            ///Temp: This stores the values from the AppSettings into the CacheClient (Redis currently) via the properties setter for teh following Config settings
            MySqlConnectionString=appHost.AppSettings
                          .GetString(configKeyPrefix+
                          configKeyMySqlConnectionString);
            RedisCacheConnectionString=appHost.AppSettings
                          .GetString(configKeyPrefix+
                          configKeyRedisConnectionString);
            // At this point the Redis cache should match the current run's AppConfigurationSettings 

            // See if the MySQL configuration key exists, if so register MySQL as the RDBMS behind ORMLite
            if (appHost.AppSettings
                .Exists(configKeyPrefix+configKeyMySqlConnectionString)) {
                var appSettingsConfigValueMySqlConnectionString = appHost.AppSettings
                    .GetString(configKeyPrefix+
                    configKeyMySqlConnectionString);
                // Configure OrmLiteConnectionFactory and register it
                Container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(appSettingsConfigValueMySqlConnectionString, MySqlDialect.Provider));
                // Access the OrmLiteConnectionFactory
                var dbFactory = Container.TryResolve<IDbConnectionFactory>();
                // Try to open the RDBMS to ensure the RDBMS is listening and the connection string is correct
                try {
                    using (var db = dbFactory.Open()) {
                        // do nothing, just open a connection to the registered  RDBMS
                        Log.Debug($"In BaseServicesData .ctor: Successfully opened connection to RDBMS");
                    }
                }
                catch (Exception e) {
                    Log.Debug($"In BaseServicesData .ctor: Exception when trying to connect to the MySQL RDBMS: Message = {e.Message}");
                    throw new Exception(MySqlCannotConnectExceptionMessage, e);
                }
            } else {
                throw new NotImplementedException(MySqlConnectionStringKeyNotFoundExceptionMessage);
            }

            // Add a dictionary of timers and a dictionary to hold "long-running tasks" to the IoC container
            #region create a dictionary of timers and register it in the IoC container
            var timers = new Dictionary<string, System.Timers.Timer>();
            Container.Register<Dictionary<string, System.Timers.Timer>>(c => timers);
            #endregion create a dictionary of timers and register it in the IoC container

            #region create a dictionary of tasks that is intended to hold "long running" tasks
            // put it into a property, and register the list in the IoC container
            
            LongRunningTasks = new Dictionary<Guid,LRTaskInfo>();
            Container.Register<Dictionary<Guid, LRTaskInfo>>(c => LongRunningTasks);
            #endregion create a list of tasks that is intended to hold "long running" tasks and register the list in the IoC container

            // Add a timer to check the status of long running tasks, and attach a callback to the timer
            #region create longRunningTasksCheckTimer, connect callback, and store in container's timers
            Log.Debug("In BaseServicesData .ctor: creating longRunningTasksCheckTimer");
            var longRunningTasksCheckTimer = new System.Timers.Timer(1000);
            longRunningTasksCheckTimer.AutoReset=true;
            longRunningTasksCheckTimer.Elapsed+=new ElapsedEventHandler(LongRunningTasksCheckTimer_Elapsed);
            timers.Add(LongRunningTasksCheckTimerName, longRunningTasksCheckTimer);
            #endregion create longRunningTasksCheckTimer, connect callback, and store in container's timers

            // Populate the ConfigurationData property
            // ConfigurationData=new ConfigurationData(RedisCacheConnectionString, MySqlConnectionString);

            // Register an Auth Repository
            Container.Register<IAuthRepository>(c => new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));
            /// Create the  UserAuth and UserAuthDetails tables in the RDBMS if they do not already exist
            Container.Resolve<IAuthRepository>().InitSchema();


            // Populate the application's Base Gateways
            // Location of the files will depend on running as LifeCycle Production/QA/Dev as well as Debug and Release settings
            Gateways=new MultiGatewaysBuilder()
          // Command line flags have highest priority
          // next in priority are  Environment variables
          //.AddEnvironmentalVariables()
          // next in priority are Configuration settings in a text file relative to the current working directory at the point in time when this method executes.
          //.AddTextFile(pluginGatewaysTextFileName)
          // Builtin (compiled in) have the lowest priority
          //.AddDictionarySettings(DefaultGateways.Configuration())
          .Build();

            // temporary manually populate the collection with one Gateway
            // Build a Gateway entry for Google Maps GeoCode and ReverseGeoCode REST endpoints
            var geb = new GatewayEntryBuilder();
            geb.AddName("ReverseGeoCode");
            geb.AddRUri("geocode/json");
            geb.AddReqDataPayloadType(typeof(Base_GoogleMapsGeoCoding_ReverseGeoCode_ReqDTO));
            geb.AddRspDataPayloadType(typeof(Base_GoogleMapsGeoCoding_ReverseGeoCode_RspDTO));
            var ge = geb.Build();

            var defaultPolicy = Policy.Handle<Exception>().RetryAsync(3, (exception, attempt) => {
                // This is the  exception handler for this defaultPolicy
                Log.Debug($"Policy logging: {exception.Message} : attempt = {attempt}");
                //retries++;

            });

            // Build a Gateway for Google Maps GeoCode Gateway
            // ToDo replace DefaultAPIKEy auth with a more robust and extendable solution
            string defaultAPIKey = appHost.AppSettings
                  .GetString(configKeyPrefix+
                  configKeyGoogleMapsAPIKey);
            var gb = new GatewayBuilder();
            gb.AddName("GoogleMaps");
            gb.AddBaseUri(new Uri("https://maps.googleapis.com/maps/api/"));
            //gb.AddDefaultPolicy(defaultPolicy);
            // ToDo replace DefaultAPIKEy auth with a more robust and extendable solution
            gb.AddDefaultAPIKey(defaultAPIKey);
            gb.AddGatewayEntry(ge);
            var gw = gb.Build();
            Gateways.Add("GoogleMaps", gw);
            //Gateways.Add("GoogleMapsGeoCoding", new GatewayBuilder().AddName("GoogleMapsGeoCoding").AddBaseUri(new Uri("https://maps.googleapis.com/maps/api").AddDefaultPolicy(new Polly.Policy()).Build());

            // Create a collection of GatewayMonitors for BaseServices based on the collection of Gateways defined by the Base services
            var gatewayMonitorsBuilder = new GatewayMonitorsBuilder("Base");
            //gatewayMonitorsBuilder.AddGatewayMonitor(new GatewayMonitor(Gateways.Get("GoogleMapsGeoCoding")));
            GatewayMonitors=gatewayMonitorsBuilder.Build();
            // temporary manually populate the collection with one GatewayMonitor

            var gemb = new GatewayEntryMonitorBuilder();
            gemb.AddName("GeoCode");
            var gem = gemb.Build();
            var gmb = new GatewayMonitorBuilder();
            gmb.AddName("GoogleMapsGeoCoding");
            gmb.AddGatewayEntryMonitor(gem);
            var gm = gmb.Build();
            GatewayMonitors.GatewayMonitorColl.Add("Manual", gm);

            //   GatewayMonitors.GatewayMonitorColl.Add("Manual", new GatewayMonitorBuilder().AddName("GoogleMapsGeoCoding").AddGatewayEntryMonitor(new GatewayEntryMonitorBuilder().AddName("GeoCode").Build()).Build());

            // Store the collection of Gateway Monitor in the Base Data structure

            // Populate the specific per-user data instance for this user
            UserData=new UserData();
            // ToDo: support AppSettings to control the enable/disable of Postman
            // Enable Postman integration
            //  AppHost.Plugins.Add(new PostmanFeature());


            // ToDo: support AppSettings to control the enable/disable of CORS Feature
            // Enable CORS support
            appHost.Plugins.Add(new CorsFeature(allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
                                        allowedOrigins: "*",
                                        allowCredentials: true,
                                        allowedHeaders: "content-type, Authorization, Accept"));

            // ToDo: support AppSettings to control the enable/disable of Metadata Feature
            /*
            appHost.Config
                .EnableFeatures = Feature.All
                .Remove(Feature.Metadata);
            */
            // Turn debug mode for the ACEAgent depending if running in debug mode or release mode
#if Debug
      AppHost.Config.DebugMode = true;
#else
            appHost.Config
                .DebugMode=false;
#endif


            Log.Debug("Leaving BaseServicesData ctor");
        }
        // constructor with event handlers
        /*
        public BaseServicesData(IAppHost appHost, ConcurrentObservableDictionary<string, GatewayMonitor> baseCOD, NotifyCollectionChangedEventHandler onBaseCODCollectionChanged, PropertyChangedEventHandler onBaseCODPropertyChanged) {
            Log.Debug("Entering BaseServicesData ctor");
            cacheClient = appHost.GetContainer()
                .Resolve<ICacheClient>();
            GatewayMonitorCOD = baseCOD;
            this.onBaseCODCollectionChanged = onBaseCODCollectionChanged;
            this.onBaseCODPropertyChanged = onBaseCODPropertyChanged;
            GatewayMonitorCOD.CollectionChanged += this.onBaseCODCollectionChanged;
            GatewayMonitorCOD.PropertyChanged += this.onBaseCODPropertyChanged;
            Log.Debug("Leaving BaseServicesData ctor");
        }
    */
    // ToDo: figure out how to create/access a method to add a task to the LongRuningTasks dictionary in the container
    /*
        public Guid AddLongRunningTask(LRTaskInfo lRTaskInfo, bool newID = true) {
            if (newID)
                lRTaskInfo.ID=Guid.NewGuid();
            Container.Resolve(typeof(Dictionary<Guid, LRTaskInfo>)) as Dictionary<Guid, LRTaskInfo>.Add(lRTaskInfo.ID, lRTaskInfo);
            return lRTaskInfo.ID;
        }
        public Guid AddLongRunningTask(Task task) {
            LRTaskInfo lRTaskInfo = new LRTaskInfo(Guid.NewGuid(), task);
            Container.Resolve(typeof(Dictionary<Guid, LRTaskInfo>)) as Dictionary<Guid, LRTaskInfo>.Add(lRTaskInfo.ID, lRTaskInfo);
            return lRTaskInfo.ID;
        }
        */

        #region EventHandlers
        void LongRunningTasksCheckTimer_Elapsed(object sender, ElapsedEventArgs e) {
            //Log.Debug("Entering the appHost.LongRunningTasksCheckTimer_Elapsed Method");
            Dictionary<string, System.Timers.Timer> timers = Container.TryResolve(typeof(Dictionary<string, System.Timers.Timer>)) as Dictionary<string, System.Timers.Timer>;
            timers[LongRunningTasksCheckTimerName].Stop();
            //Log.Debug("checking for existence of any longRunningTasks");
            Dictionary<Guid, LRTaskInfo> longRunningTaskList = Container.Resolve(typeof(Dictionary<Guid, LRTaskInfo>)) as Dictionary<Guid, LRTaskInfo>;
            //Log.Debug($"There are {longRunningTaskList.Count} tasks in the longRunningTaskList");
            timers[LongRunningTasksCheckTimerName].Start();
            //Log.Debug("Leaving the appHost.LongRunningTasksCheckTimer_Elapsed Method");
        }


        #endregion

        #region String Constants
        #region String Constants:Configuration Key strings
        public const string configKeyAceAgentListeningOnString = "Ace.Agent.ListeningOn";
        public const string configKeyRedisConnectionString = "RedisConnectionString";
        public const string configKeyMySqlConnectionString = "MySqlConnectionString";
        public const string configKeyGoogleMapsAPIKey = "GoogleMapsAPIKey";
        #endregion 

        #region String Constants:Exception Messages
        const string RedisNotRunningExceptionMessage = "Redis Connection string found, but Redis not running as cacheClient.";
        const string RedisConnectionStringKeyNotFoundExceptionMessage = "RedisConnectionString Key not found in Application's Configuration settings and no other ICache implemenation is supported. Add the RedisConnectionString Key and Value to the Application Configuration, and retry.";
        const string MySqlConnectionStringKeyNotFoundExceptionMessage = "MySqlConnectionString Key not found in Application's Configuration settings and no other ORMLite implemenation is supported. Add the MySqlConnectionString Key and Value to the Application Configuration, and retry.";
        const string ListeningServiceNotRunningInnerExceptionMessage = "No connection could be made because the target machine actively refused it ";
        const string MySqlCannotConnectExceptionMessage = "MySqlConnectionString Key found, but cannot connect to it. Ensure that the service is running, and that you have supplied the correct credentials";

        #endregion

        #region String Constants:File Names
        const string dummy = "";
        #endregion

        #region String Constants:Timer Names
        public const string LongRunningTasksCheckTimerName = "LongRunningTasksCheckTimer";
        #endregion

        #endregion string constants

        #region static fields
        // Surface the configKeyPrefix for this namespace
        public static string configKeyPrefix = MethodBase.GetCurrentMethod()
                .DeclaringType
                .Namespace+
            ".Config.";

        // Create a logger for this class
        public static ILog Log = LogManager.GetLogger(typeof(BaseServicesData));

        #endregion static fields

        #region private fields 
        // private field for the ICacheClient, populated by the constructor
        ICacheClient cacheClient;

        NotifyCollectionChangedEventHandler onBaseCODCollectionChanged;
        PropertyChangedEventHandler onBaseCODPropertyChanged;

        NotifyCollectionChangedEventHandler onNestedCollectionChanged;
        PropertyChangedEventHandler onNestedPropertyChanged;
        #endregion private fields 

        #region Properties
        #region Properties:Container
        public Funq.Container Container { get; set; }
        #endregion
        #region Properties:Gateways and GatewayMonitors
        public IGateways Gateways { get; set; }
        public IGatewayMonitors GatewayMonitors { get; set; }
        #endregion
        #region Properties:Configuration Data
        public ConfigurationData ConfigurationData;
        public string RedisCacheConnectionString {
            get { return cacheClient.Get<string>(configKeyPrefix+configKeyRedisConnectionString); }
            set { cacheClient.Set<string>(configKeyPrefix+configKeyRedisConnectionString, value); }
        }
        public string MySqlConnectionString {
            get { return cacheClient.Get<string>(configKeyPrefix+configKeyMySqlConnectionString); }
            set { cacheClient.Set<string>(configKeyPrefix+configKeyMySqlConnectionString, value); }
        }
        #endregion
        #region Properties:UserData
        public UserData UserData;

        #endregion

        #endregion

        #region IDisposable Support
        public void TearDown() {
            //TearDownGateways
            //TearDownGatewayMonitors
            /*
            GatewayMonitorCOD.CollectionChanged -= this.onBaseCODCollectionChanged;
            GatewayMonitorCOD.PropertyChanged -= this.onBaseCODPropertyChanged;
      var enumerator = GatewayMonitorCOD.Keys.GetEnumerator();
      try
      {
      while (enumerator.MoveNext())
      {
      var key = enumerator.Current;
      calculatedResults[key].CollectionChanged -= this.onNestedCollectionChanged;
      calculatedResults[key].PropertyChanged -= this.onNestedPropertyChanged;
      }
      }
      finally
      {
      enumerator.Dispose();
      }
      */
        }

        bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // dispose managed state (managed objects).
                    TearDown();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue=true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~WithObservableConcurrentDictionaryAndEventHandlers() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }
        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
