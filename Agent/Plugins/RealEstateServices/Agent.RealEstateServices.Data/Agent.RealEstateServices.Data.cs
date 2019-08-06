using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using ATAP.Utilities.ConcurrentObservableCollections;
using ServiceStack;
using ServiceStack.Caching;

namespace Ace.Plugin.RealEstateServices
{
    public class RealEstateServicesData : IDisposable {

        #region PublicStaticFields
        #region PublicStaticFields:configKeyPrefix
        // Surface the configKeyPrefix for this namespace
        public static string configKeyPrefix = MethodBase.GetCurrentMethod().DeclaringType.Namespace+".Config";
        #endregion
        #endregion

        // private field for the cacheClient, populated by the constructor
        ICacheClient cacheClient;

        NotifyCollectionChangedEventHandler onNestedCollectionChanged;
        PropertyChangedEventHandler onNestedPropertyChanged;

        NotifyCollectionChangedEventHandler onPluginRootCODCollectionChanged;
        PropertyChangedEventHandler onPluginRootCODPropertyChanged;

        // constructor with event handlers
        public RealEstateServicesData(IAppHost appHost, ConcurrentObservableDictionary<string, decimal> pluginRootCOD, NotifyCollectionChangedEventHandler onPluginRootCODCollectionChanged, PropertyChangedEventHandler onPluginRootCODPropertyChanged) {
      
            cacheClient = appHost.GetContainer().Resolve<ICacheClient>();
            PluginRootCOD = pluginRootCOD;
            this.onPluginRootCODCollectionChanged = onPluginRootCODCollectionChanged;
            this.onPluginRootCODPropertyChanged = onPluginRootCODPropertyChanged;
            pluginRootCOD.CollectionChanged += this.onPluginRootCODCollectionChanged;
            pluginRootCOD.PropertyChanged += this.onPluginRootCODPropertyChanged;
        }

        public Uri UriGoogle_API_URI
        {
            get
            { return cacheClient.Get<Uri>(configKeyPrefix + "UriGoogle_API_URI"); }
            set
            { cacheClient.Set<Uri>(configKeyPrefix + "UriGoogle_API_URI", value); }
        }

    public string Google_API_URI {
      get { return cacheClient.Get<string>(configKeyPrefix + "Google_API_URI"); }
      set { cacheClient.Set<string>(configKeyPrefix + "Google_API_URI", value); }
    }

    public string GoogleAPIKeyEncrypted
        {
            get
            { return cacheClient.Get<string>(configKeyPrefix + "GoogleAPIKeyEncrypted"); }
            set
            { cacheClient.Set<string>(configKeyPrefix + "GoogleAPIKeyEncrypted", value); }
        }

        public string GoogleAPIKeyPassPhrase
        {
            get
            { return cacheClient.Get<string>(configKeyPrefix + "GoogleAPIKeyPassPhrase"); }
            set
            { cacheClient.Set<string>(configKeyPrefix + "GoogleAPIKeyPassPhrase", value); }
        }

        public Uri UriHomeAway_API_URI
        {
            get
            { return cacheClient.Get<Uri>(configKeyPrefix + "UriHomeAway_API_URI"); }
            set
            { cacheClient.Set<Uri>(configKeyPrefix + "UriHomeAway_API_URI", value); }
        }

    public string HomeAway_API_URI {
      get { return cacheClient.Get<string>(configKeyPrefix + "HomeAway_API_URI"); }
      set { cacheClient.Set<string>(configKeyPrefix + "HomeAway_API_URI", value); }
    }

    public string HomeAwayAPIKeyEncrypted
        {
            get
            { return cacheClient.Get<string>(configKeyPrefix + "HomeAwayAPIKeyEncrypted"); }
            set
            { cacheClient.Set<string>(configKeyPrefix + "HomeAwayAPIKeyEncrypted", value); }
        }

        public string HomeAwayAPIKeyPassPhrase
        {
            get
            { return cacheClient.Get<string>(configKeyPrefix + "HomeAwayAPIKeyPassPhrase"); }
            set
            { cacheClient.Set<string>(configKeyPrefix + "HomeAwayAPIKeyPassPhrase", value); }
        }

        public ConcurrentObservableDictionary<string, decimal> PluginRootCOD { get; set; }

    #region IDisposable Support
        public void TearDown() {
            PluginRootCOD.CollectionChanged -= this.onPluginRootCODCollectionChanged;
            PluginRootCOD.PropertyChanged -= this.onPluginRootCODPropertyChanged;
            /*
var enumerator = pluginRootCOD.Keys.GetEnumerator();
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
