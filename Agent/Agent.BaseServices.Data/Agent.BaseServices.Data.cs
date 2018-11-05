using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using Polly;
using Polly.Timeout;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Logging;
using ServiceStack.Redis;
using Swordfish.NET.Collections;
//using ATAP.Utilities.HTTP;

namespace Ace.AceAgent.BaseServices {

  public interface IGatewayEntry
  {
    string Name { get; }
    Type RequestDataPayload { get; set; }
    Type ResponseDataPayload { get; set; }
    string RUriStr { get; set; }
  }
  public class GatewayEntry : IGatewayEntry
  {
    string name;
    public GatewayEntry(string name)
    {
      this.name = name;
    }
    public string Name { get => name; }
    public Type RequestDataPayload { get; set; }

    public Type ResponseDataPayload { get; set; }

    public string RUriStr { get; set; }
  }

  public interface IGateway
  {
    Uri BaseUri { get; }
    Policy DefaultPolicy { get; set; }
    string Name { get; }
	Dictionary<string, IGatewayEntry> GatewayEntries  { get; set; }
  }
  public abstract class Gateway : IGateway
  {
    string name;
    Uri baseUri;

    protected Gateway(string name, Uri baseUrI)
    {
      this.name = name;
      this.baseUri = baseUrI;
    }
    public string Name { get => name; }
    public Uri BaseUri { get => baseUri; }
    public Policy DefaultPolicy { get; set; }

    public Dictionary<string, IGatewayEntry> GatewayEntries { get; set; }

  }

  public class HomeAwayGateway :Gateway
  {
    public HomeAwayGateway() : base("HomeAway", new Uri("https://ws.homeaway.com/")) {
      DefaultPolicy = Policy.TimeoutAsync(new TimeSpan(0, 0, 30), TimeoutStrategy.Optimistic);
      GatewayEntries = new Dictionary<string, IGatewayEntry>();
      GatewayEntry gw = new GatewayEntry("search")
      {
        RUriStr = "/public/search",
        RequestDataPayload = typeof(string),
        ResponseDataPayload = typeof(string)
      };
      GatewayEntries.Add("search", gw);
      }

  }



    public class BaseServicesData : IDisposable {
  #region AppSettings Configuration Keys (string constants)
    public const string appSettingsConfigKeyRedisConnectionString = "RedisConnectionString";
    public const string appSettingsConfigKeyAceAgentListeningOnString = "Ace.Agent.ListeningOn";
    #endregion AppSettings Configuration Keys
    #region Exception Messages (string constants)
    const string RedisConnectionStringKeyNotFoundExceptionMessage = "RedisConnectionString Key not found in Application's Configuration settings and no other ICache implemenation is supported. Add the RedisConnectionString Key and Value to the Application Configuration, and retry.";
  #endregion Exception Messages (string constants)

        // Constructor with just AppHost parameter
        public BaseServicesData(IAppHost appHost) {
            Log.Debug("Entering BaseServicesData ctor");
            AppHost = appHost;
            Container = appHost.GetContainer();
            // placeholder for the eventual COD structure that holds all the base services data, which can be monitored by the GUI
            BaseCOD = new ConcurrentObservableDictionary<string, object>();

            // Key names in the cache for the Application's Base configuration settings consist of this namespace and the string .Config
    // This prefix is available as a static method on this class
    //var appSettingsConfigKeys = appSettings.GetAllKeys();
    //var fullAppSettingsConfigKeys = appSettingsConfigKeys.Select(x => x.IndexOf(configKeyPrefix) >= 0? x: configKeyPrefix + x);

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

      // ToDo:Create the ATAP.Utilities.HTTP web client and register it
      //container.Register<GenericWebGet>(c => new GenericWebGet());

      // Create the collection of Gateways
      Gateways = new ConcurrentObservableDictionary<string, IGateway>();
      // populate the collection with the common web APIs
      // Start with the AceService.BaseService builtin (compile-time) common APIs
      // Add (Superseding any previous values) the optional Gateway file for BaseService gateway settings from a text file in the program directory (AKA Ace.Gateways.txt )

      // First from the builtin
      // 
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

  // private field for the ICacheClient, populated by the constructor
  #region private fields 
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
