using System;
using ServiceStack.Logging;
using Swordfish.NET.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.ComponentModel;
namespace Ace.AceService.RealEstateSearchServices.PluginData
{
    public class RealEstateSearchServicesPluginData : IDisposable
    {
        NotifyCollectionChangedEventHandler onPluginRootCODCollectionChanged;
        PropertyChangedEventHandler onPluginRootCODPropertyChanged;
        NotifyCollectionChangedEventHandler onNestedCollectionChanged;
        PropertyChangedEventHandler onNestedPropertyChanged;
        ConcurrentObservableDictionary<string, decimal> pluginRootCOD;
        // Create the plugIn's observable data structures based on the configuration settings
        public RealEstateSearchServicesPluginData() : this(new ConcurrentObservableDictionary<string, decimal>())
        {
        }

        // Create the plugIn's observable data structures by specifying each in a constructor call
        public RealEstateSearchServicesPluginData(ConcurrentObservableDictionary<string, decimal> pluginRootCOD)
        {
            this.pluginRootCOD = pluginRootCOD;
        }

        // constructor with event handlers
        public RealEstateSearchServicesPluginData(ConcurrentObservableDictionary<string, decimal> pluginRootCOD, NotifyCollectionChangedEventHandler onPluginRootCODCollectionChanged, PropertyChangedEventHandler onPluginRootCODPropertyChanged)
        {
            this.pluginRootCOD = pluginRootCOD;
            this.onPluginRootCODCollectionChanged = onPluginRootCODCollectionChanged;
            this.onPluginRootCODPropertyChanged = onPluginRootCODPropertyChanged;
            pluginRootCOD.CollectionChanged += this.onPluginRootCODCollectionChanged;
            pluginRootCOD.PropertyChanged += this.onPluginRootCODPropertyChanged;
        }



        public ConcurrentObservableDictionary<string, decimal> PluginRootCOD => pluginRootCOD;
        #region IDisposable Support
        public void TearDown()
        {
            pluginRootCOD.CollectionChanged -= this.onPluginRootCODCollectionChanged;
            pluginRootCOD.PropertyChanged -= this.onPluginRootCODPropertyChanged;
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
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
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
