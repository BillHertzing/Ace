using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Logging;
using Swordfish.NET.Collections;
using Ace.Agent.BaseServices;

namespace Ace.Agent.DiskAnalysisServices
{
    public class DiskAnalysisServicesData : IDisposable {
    #region string constants
    #region Configuration Key strings
    #endregion Configuration Key strings
    #region Exception Messages (string constants)
    #endregion Exception Messages (string constants)
    #region File Name string constants
    #endregion File Name string constants
    #endregion string constants

    // Surface the configKeyPrefix for this namespace
    public static string configKeyPrefix =
    MethodBase
    .GetCurrentMethod()
                .DeclaringType
                .Namespace +
            ".Config";
        // Create a logger for this class
        public static ILog Log = LogManager.GetLogger(typeof(DiskAnalysisServicesData));
        // private field for the cacheClient, populated by the constructor
        ICacheClient cacheClient;

        NotifyCollectionChangedEventHandler onNestedCollectionChanged;
        PropertyChangedEventHandler onNestedPropertyChanged;

        NotifyCollectionChangedEventHandler onPluginRootCODCollectionChanged;
        PropertyChangedEventHandler onPluginRootCODPropertyChanged;

        // constructor with event handlers
        public DiskAnalysisServicesData(IAppHost appHost, ConcurrentObservableDictionary<string, decimal> pluginRootCOD, NotifyCollectionChangedEventHandler onPluginRootCODCollectionChanged, PropertyChangedEventHandler onPluginRootCODPropertyChanged) {
      
            cacheClient = appHost.GetContainer().Resolve<ICacheClient>();
            BaseServicesData=appHost.GetContainer().Resolve<BaseServicesData>();
            PluginRootCOD = pluginRootCOD;
            this.onPluginRootCODCollectionChanged = onPluginRootCODCollectionChanged;
            this.onPluginRootCODPropertyChanged = onPluginRootCODPropertyChanged;
            pluginRootCOD.CollectionChanged += this.onPluginRootCODCollectionChanged;
            pluginRootCOD.PropertyChanged += this.onPluginRootCODPropertyChanged;
            // ToDo: Get the Configuration data into the COD, and populate it from "plugin configuration data load"
            ConfigurationData=new DiskAnalysisServices.ConfigurationData(4096);
            UserData=new DiskAnalysisServices.UserData();
        }

        public BaseServicesData BaseServicesData { get; set; }
        public ConcurrentObservableDictionary<string, decimal> PluginRootCOD { get; set; }
        public ConfigurationData ConfigurationData { get; set; }
        public UserData UserData { get; set; }

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
