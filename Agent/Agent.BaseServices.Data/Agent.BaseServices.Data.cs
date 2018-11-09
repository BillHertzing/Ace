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

namespace Ace.Agent.BaseServices {
    public class BaseServicesData : IDisposable {
        // Constructor with just AppHost parameter
        public BaseServicesData(IAppHost appHost) {
            Log.Debug("Entering BaseServicesData ctor");
            AppHost = appHost;
            Container = appHost.GetContainer();

            // If the Redis configuration key exists, register Redis as a name:value pair cache
            if(AppHost.AppSettings
                .Exists(configKeyPrefix + appSettingsConfigKeyRedisConnectionString)) {
                var appSettingsConfigValueRedisConnectionString = AppHost.AppSettings
                    .GetString(configKeyPrefix +
                    appSettingsConfigKeyRedisConnectionString);
                Container.Register<IRedisClientsManager>(c => new RedisManagerPool(appSettingsConfigValueRedisConnectionString));
                Container.Register(c => c.Resolve<IRedisClientsManager>()
                    .GetCacheClient());
                cacheClient = Container.Resolve<ICacheClient>();
            } else {
                throw new NotImplementedException(RedisConnectionStringKeyNotFoundExceptionMessage);
            }

            // Ensure that the cacheClient is running
            try
            {
              var test = cacheClient.GetKeysStartingWith(configKeyPrefix);
            } catch ( ServiceStack.Redis.RedisException Ex){
              if (Ex.InnerException.Message.Contains(ListeningServiceNotRunningInnerExceptionMessage) ) {
                throw new Exception($"{RedisNotRunningExceptionMessage} on {Ex.Message}", Ex); }
            } 


      // Key names in the cache for the Application's Base configuration settings consist of this namespace and the string .Config
      // This prefix is available as a static method on this class
      var cacheConfigKeys = cacheClient.GetKeysStartingWith(configKeyPrefix);
      var appSettingsConfigKeys = AppHost.AppSettings.GetAllKeys();
      //var fullAppSettingsConfigKeys = appSettingsConfigKeys.Select(x => x.IndexOf(configKeyPrefix) >= 0? x: configKeyPrefix + x);

      // See if the MySQL configuration key exists, if so register MySQL as the RDBMS behind ORMLite
      if (AppHost.AppSettings
          .Exists(configKeyPrefix + appSettingsConfigKeyMySqlConnectionString))
      {
        var appSettingsConfigValueMySqlConnectionString = AppHost.AppSettings
            .GetString(configKeyPrefix +
            appSettingsConfigKeyMySqlConnectionString);
        // Configure OrmLiteConnectionFactory and register it
        Container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(appSettingsConfigValueMySqlConnectionString, MySqlDialect.Provider));
        // Access the OrmLiteConnectionFactory
        var dbFactory = Container.TryResolve<IDbConnectionFactory>();
        // Try to open the RDBMS to ensure the RDBMS is listening and the connection string is correct
        try
        {
          using (var db = dbFactory.Open())
          {
            // do nothing, just open a connection to the registered  RDBMS
            Log.Debug($"Successfully opened connection to RDBMS");
          }
        }
        catch (Exception e)
        {
          Log.Debug($"Exception when trying to connect to the RDBMS: Message = {e.Message}");
          throw new Exception(MySqlCannotConnectExceptionMessage, e);
        }
      }
      else
      {
        throw new NotImplementedException(MySqlConnectionStringKeyNotFoundExceptionMessage);
      }

      // Register an Auth Repository
      Container.Register<IAuthRepository>(c => new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));
      /// Create the  UserAuth and UserAuthDetails tables in the RDBMS if they do not already exist
      Container.Resolve<IAuthRepository>().InitSchema();


      // Populate the application's Base Gateways
      // Location of the files will depend on running as LifeCycle Production/QA/Dev as well as Debug and Release settings
      Gateways = new MultiGatewaysBuilder()
    // Command line flags have highest priority
    // next in priority are  Environment variables
    //.AddEnvironmentalVariables()
    // next in priority are Configuration settings in a text file relative to the current working directory at the point in time when this method executes.
    //.AddTextFile(pluginGatewaysTextFileName)
    // Builtin (compiled in) have the lowest priority
    //.AddDictionarySettings(DefaultGateways.Configuration())
    .Build();

      // temporarly manually populate the collection with one Gateway
      var geb = new GatewayEntryBuilder();
      geb.AddName("ReverseGeoCode");
      geb.AddRUri("geocode/json");
      geb.AddReqDataPayloadType(typeof(Base_GoogleMapsGeoCoding_ReverseGeoCode_ReqDTO));
      geb.AddRspDataPayloadType(typeof(Base_GoogleMapsGeoCoding_ReverseGeoCode_RspDTO));
      var ge = geb.Build();

      var defaultPolicy = Policy.Handle<Exception>().RetryAsync(3, (exception, attempt) =>
      {
        // This is the  exception handler for this policy deaultpolicy
        Log.Debug($"Policy logging: {exception.Message} : attempt = {attempt}");
        //retries++;

      });

      var gb = new GatewayBuilder();
      gb.AddName("GoogleMapsGeoCoding");
      gb.AddBaseUri(new Uri("https://maps.googleapis.com/maps/api"));
      gb.AddDefaultPolicy(defaultPolicy);
      gb.AddGatewayEntry(ge);
      var gw = gb.Build();
      Gateways.Add("GoogleMapsGeoCoding", gw);
     //Gateways.Add("GoogleMapsGeoCoding", new GatewayBuilder().AddName("GoogleMapsGeoCoding").AddBaseUri(new Uri("https://maps.googleapis.com/maps/api").AddDefaultPolicy(new Polly.Policy()).Build());

      // Create a collection of GatewayMonitors for BaseServices based on the collection of Gateways defined by the Base services
      var gatewayMonitorsBuilder = new GatewayMonitorsBuilder("Base");
      //gatewayMonitorsBuilder.AddGatewayMonitor(new GatewayMonitor(Gateways.Get("GoogleMapsGeoCoding")));
      GatewayMonitors = gatewayMonitorsBuilder.Build();
      // temporarly manually populate the collection with one GatewayMonitor
      
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
      // ToDo: support AppSettings to control the enable/disable of Postman
      // Enable Postman integration
      AppHost.Plugins.Add(new PostmanFeature());


      // ToDo: support AppSettings to control the enable/disable of CorsFeature
      // Enable CORS support
      AppHost.Plugins.Add(new CorsFeature(allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
                                  allowedOrigins: "*",
                                  allowCredentials: true,
                                  allowedHeaders: "content-type, Authorization, Accept"));

      // ToDo: support AppSettings to control the enable/disable of Metadata Feature
      AppHost.Config
          .EnableFeatures = Feature.All
          .Remove(Feature.Metadata);

      // Turn debug mode for the ACEAgent depending if running in debug mode or release mode
#if Debug
      AppHost.Config.DebugMode = true;
#else
      AppHost.Config
          .DebugMode = false;
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

      #region string constants
      #region Configuration Key strings
    public const string appSettingsConfigKeyAceAgentListeningOnString = "Ace.Agent.ListeningOn";
    public const string appSettingsConfigKeyRedisConnectionString = "RedisConnectionString";
    public const string appSettingsConfigKeyMySqlConnectionString = "MySqlConnectionString";
    #endregion Configuration Key strings
    #region Exception Messages (string constants)
    const string RedisNotRunningExceptionMessage = "Redis Connection string found, but Redis not running as cacheClient."; 
    const string RedisConnectionStringKeyNotFoundExceptionMessage = "RedisConnectionString Key not found in Application's Configuration settings and no other ICache implemenation is supported. Add the RedisConnectionString Key and Value to the Application Configuration, and retry.";
    const string MySqlConnectionStringKeyNotFoundExceptionMessage = "MySqlConnectionString Key not found in Application's Configuration settings and no other ORMLite implemenation is supported. Add the MySqlConnectionString Key and Value to the Application Configuration, and retry.";
    const string ListeningServiceNotRunningInnerExceptionMessage = "No connection could be made because the target machine actively refused it ";
    const string MySqlCannotConnectExceptionMessage = "MySqlConnectionString Key found, but cannot connect to it. Ensure that the service is running, and that you have supplied the correct credentials";

    #endregion Exception Messages (string constants)
    #region File Name string constants
    const string dummy = "";
    #endregion File Name string constants
    #endregion string constants

    #region static fields
    // Surface the configKeyPrefix for this namespace
    public static string configKeyPrefix =
  MethodBase
  .GetCurrentMethod()
            .DeclaringType
            .Namespace +
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

    #region Public Properties
    public IAppHost AppHost { get; set; }

        public Funq.Container Container { get; set; }

        public IGateways Gateways {
            get;
            set;
        }


    public IGatewayMonitors GatewayMonitors { get; set; }
    #endregion Public Properties

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
            if(!disposedValue) {
                if(disposing) {
                    // dispose managed state (managed objects).
                    TearDown();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

                disposedValue = true;
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
