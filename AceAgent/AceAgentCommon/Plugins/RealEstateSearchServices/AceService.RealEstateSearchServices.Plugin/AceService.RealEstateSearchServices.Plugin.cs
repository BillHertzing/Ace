using System;
using ServiceStack;
using ServiceStack.Logging;
using Ace.AceService.RealEstateSearchServices.Interfaces;
using Ace.AceService.RealEstateSearchServices.PluginData;
using Swordfish.NET.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.ComponentModel;
namespace Ace.AceService.RealEstateSearchServices.Plugin
{
  

  public class RealEstateSearchServicesPlugin : IPlugin, IPreInitPlugin
  {
    public static ILog Log = LogManager.GetLogger(typeof(RealEstateSearchServicesPlugin));
    // Declare Event Handlers for the Plugin Root COD
    // These event handler will be attached/detached from the ObservableConcurrentDictionary via that class' constructor and dispose method
    public void onPluginRootCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      Log.Debug($"event onPluginRootCollectionChanged, args e = {e}");
      // send a SignalR message to all subscribers
    }

    public void onPluginRootPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      Log.Debug($"event onPluginRootPropertyChanged, args e = {e}");
      // send a SignalR message to all subscribers
    }
    public void Configure(IAppHost appHost)
    {
      Log.Debug("starting RealEstateSearchServicesPlugin.Configure");
      // Create the plugIn's observable data structures 
      // and pass it to the container so it will be available to every other module and services
      appHost.GetContainer()
      .Register<RealEstateSearchServicesPluginData>(observableDataStructures => new RealEstateSearchServicesPluginData(
        new ConcurrentObservableDictionary<string, decimal>(), onPluginRootCollectionChanged, onPluginRootPropertyChanged
        ));

      // ToDo: enable the mechanisms that monitors each GUI-specific data sensor, and start them running

    }

     
    /// <summary>
    /// Register this plugin with the appHost
    /// Configure its observableDataStructures and event handlers
    /// </summary>
    /// <param name="appHost">The hosting provider</param>
    /// 
    public void Register(IAppHost appHost)
    {
      Log.Debug("starting RealEstateSearchServicesPlugin.Register");

      if (null == appHost) { throw new ArgumentNullException("appHost"); }
      appHost.RegisterService<Ace.AceService.RealEstateSearchServices.Interfaces.RealEstateSearchServices>();
      this.Configure(appHost);
    }

#if DEBUG
    const string apipath = @"https://www.realtor.com";
#else
  const string apipath = @"https://www.realtor.com";
#endif
  }
}
