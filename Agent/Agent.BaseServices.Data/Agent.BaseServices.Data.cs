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
        var cacheConfigKeys = cacheClient.GetKeysStartingWith(configKeyPrefix);
      } catch ( ServiceStack.Redis.RedisException Ex){
        if (Ex.InnerException.Message.Contains(RedisNotRunningInnerExceptionMessage) ) {
          throw new Exception($"{RedisNotRunningExceptionMessage} on {Ex.Message}", Ex); }
      } finally
      {
        // shutdown cleanly
      }

            // placeholder for the eventual COD structure that holds all the base services data, which can be monitored by the GUI
            BaseCOD = new ConcurrentObservableDictionary<string, object>();

            // Key names in the cache for the Application's Base configuration settings consist of this namespace and the string .Config
      // This prefix is available as a static method on this class
      //var appSettingsConfigKeys = appSettings.GetAllKeys();
      //var fullAppSettingsConfigKeys = appSettingsConfigKeys.Select(x => x.IndexOf(configKeyPrefix) >= 0? x: configKeyPrefix + x);
      // ToDo:Create the ATAP.Utilities.HTTP web client and register it
      //container.Register<GenericWebGet>(c => new GenericWebGet());

            // Create the collection of Gateways
      Gateways = new ConcurrentObservableDictionary<string, IGateway>();
            Gateways.Add("test", new GatewayBuilder().Build());
      // populate the collection with some common web APIs
      // Start with the gateways defined in the AceService.BaseService builtin (compile-time) file
      // Add (Superseding any previous values) gateways from the optional Gateway file for BaseService gateways. This is a text file in the program directory (AKA Ace.Gateways.txt )

      // First from the builtin
      //

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
        public BaseServicesData(IAppHost appHost, ConcurrentObservableDictionary<string, object> baseCOD, NotifyCollectionChangedEventHandler onBaseCODCollectionChanged, PropertyChangedEventHandler onBaseCODPropertyChanged) {
            Log.Debug("Entering BaseServicesData ctor");
            cacheClient = appHost.GetContainer()
                .Resolve<ICacheClient>();
            BaseCOD = baseCOD;
            this.onBaseCODCollectionChanged = onBaseCODCollectionChanged;
            this.onBaseCODPropertyChanged = onBaseCODPropertyChanged;
            BaseCOD.CollectionChanged += this.onBaseCODCollectionChanged;
            BaseCOD.PropertyChanged += this.onBaseCODPropertyChanged;
            Log.Debug("Leaving BaseServicesData ctor");
        }

    #region string constants
    #region Configuration Key strings
    public const string appSettingsConfigKeyRedisConnectionString = "RedisConnectionString";
        public const string appSettingsConfigKeyAceAgentListeningOnString = "Ace.Agent.ListeningOn";
    #endregion Configuration Key strings
    #region Exception Messages (string constants)
    const string RedisNotRunningExceptionMessage = "Redis Connection string found, but Redis not running as cacheClient.";
      
          const string RedisConnectionStringKeyNotFoundExceptionMessage = "RedisConnectionString Key not found in Application's Configuration settings and no other ICache implemenation is supported. Add the RedisConnectionString Key and Value to the Application Configuration, and retry.";
    const string RedisNotRunningInnerExceptionMessage = "No connection could be made because the target machine actively refused it ";
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

        public ConcurrentObservableDictionary<string, object> BaseCOD { get; set; }

        public Funq.Container Container { get; set; }

        public ConcurrentObservableDictionary<string, IGateway> Gateways {
            get;
            set;
        }
    #endregion Public Properties

    #region IDisposable Support
    public void TearDown() {
        BaseCOD.CollectionChanged -= this.onBaseCODCollectionChanged;
        BaseCOD.PropertyChanged -= this.onBaseCODPropertyChanged;
        /*
  var enumerator = BaseCOD.Keys.GetEnumerator();
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
