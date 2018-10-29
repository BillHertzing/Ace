using System;
using System.Collections;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Caching;
//using Ace.AceCommon.Plugin.RealEstateSearchServices.Interfaces;
//using Ace.AceCommon.Plugin.RealEstateSearchServices.PluginData;
using Swordfish.NET.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.ComponentModel;
using ServiceStack.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Ace.AceCommon.Plugin.RealEstateSearchServices
{
  

  public class RealEstateSearchServicesPlugin : IPlugin, IPreInitPlugin
  {
    // Create a logger for this class
    public static ILog Log = LogManager.GetLogger(typeof(RealEstateSearchServicesPlugin));

    // Surface the configKeyPrefix for this namespace
    public static string myNamespace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;

    // AppSettings will be auto-wired by ServiceStack to the IAppSettings in the IOC
    public IAppSettings AppSettings { get; set; }

    public Funq.Container Container { get; set; }
    public IAppHost appHost { get; set; }
    // Declare Event Handlers for the Plugin Root COD
    // These event handler will be attached/detached from the ObservableConcurrentDictionary via that class' constructor and dispose method
    public void onPluginRootCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      Log.Debug($"event onPluginRootCollectionChanged, args e = {e}");
      // send a SSE message to all subscribers
    }

    public void onPluginRootPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      Log.Debug($"event onPluginRootPropertyChanged, args e = {e}");
      // send a SSE message to all subscribers
    }
    public void Configure(IAppHost appHost)
    {
      Log.Debug("starting RealEstateSearchServicesPlugin.Configure");

      // Populate this Plugin's Application Configuration Settings
      // Location of the files will depend on Production/DebugLogFactory, as
      var pluginAppSettings =new MultiAppSettingsBuilder()
      // Builtin (compiled in)
    .AddDictionarySettings(DefaultConfiguration.Configuration())
    // Configuration settings in a text file in the program directory
    .AddTextFile("RealEstateSearchServices.settings.txt")
    //.AddEnvironmentalVariables()
    .Build();

      // Key names in the cache for Configuration settings for a Plugin consist of the namespace and the string .Config
      // Key names in the appSettings of a Plugin for Configuration settings may or may not have the prefix
      // Compare the two lists of Configuration setting keys...

      // create a copy of the keys from AppSettings ensuring all are prefixed with the namespace and .Config
      var configKeyPrefix = myNamespace + ".Config.";
      var lengthConfigKeyPrefix = configKeyPrefix.Length;
      var appSettingsConfigKeys = pluginAppSettings.GetAllKeys();
      var fullAppSettingsConfigKeys = appSettingsConfigKeys.Select(x => x.IndexOf(configKeyPrefix) >= 0? x: configKeyPrefix + x);

      // Get this namespaces configuration settings from the cache
      var cacheClient = appHost.GetContainer().Resolve<ICacheClient>();
      var cacheConfigKeys = cacheClient.GetKeysStartingWith(configKeyPrefix);

      // Compare the configuration retrieved from the cache with the configuration from the built-in defaults, the text files, and the environment variables
      // If the cache is missing a ConfigKey, update the cache with the ConfigKey and its value from the appSettings
      var excludedConfigKeys = new HashSet<string>(cacheConfigKeys);
      var additionalConfigKeys = fullAppSettingsConfigKeys.Where(ck => !excludedConfigKeys.Contains(ck));
      foreach (var ck in additionalConfigKeys) { cacheClient.Set<string>(ck, pluginAppSettings.GetString(ck.Substring(lengthConfigKeyPrefix))); }
      // If both the cache and the appSettings have the same ConfigKey, and the same value, do nothing
      // If both the cache and the appSettings have the same ConfigKey, and the values are different,
      // If the cache has a ConfigKey and the appSettings does not have that ConfigKey
      // Set a flag indicating the need to dialog with the user to resolve the cache vs. appSettings discrepancies
      bool configurationsMatch = true; // (appSettingsConfigkeys.Count == cacheConfigkeys.Count) ;

      // Create the Plugin's data structure. There should only be a single instance.
      // Every Property matching a ConfigKey gets/sets the value of the matching ConfigKey in the cache
      // ConfigKey Properties do not have to be set in the constructor because the cache was setup before calling the constructor

      RealEstateSearchServicesPluginData realEstateSearchServicesPluginData = new RealEstateSearchServicesPluginData(appHost, new ConcurrentObservableDictionary<string, decimal>(), onPluginRootCollectionChanged, onPluginRootPropertyChanged);

      // copy the most recent configuration settings to the PluginData
      // hmm should be a way to make sure the PluginData has a Property for each configuration setting, and to populate the initial PluginData with the cache value
      realEstateSearchServicesPluginData.GoogleAPIKeyEncrypted = cacheClient.Get<string>(configKeyPrefix + "GoogleAPIKeyEncrypted");
      realEstateSearchServicesPluginData.HomeAwayAPIKeyEncrypted = cacheClient.Get<string>(configKeyPrefix + "HomeAwayAPIKeyEncrypted");
      realEstateSearchServicesPluginData.HomeAway_API_URI = cacheClient.Get<string>(configKeyPrefix + "HomeAway_API_URI");
      realEstateSearchServicesPluginData.Google_API_URI = cacheClient.Get<string>(configKeyPrefix + "Google_API_URI");
      realEstateSearchServicesPluginData.UriHomeAway_API_URI = cacheClient.Get<Uri>(configKeyPrefix + "UriHomeAway_API_URI");
      realEstateSearchServicesPluginData.UriGoogle_API_URI = cacheClient.Get<Uri>(configKeyPrefix + "UriGoogle_API_URI");

      // and pass the Plugin's data structure to the container so it will be available to every other module and services
      appHost.GetContainer()
      .Register<RealEstateSearchServicesPluginData>(x => realEstateSearchServicesPluginData);




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
      appHost.RegisterService<Ace.AceCommon.Plugin.RealEstateSearchServices.RealEstateSearchServices>();
      this.Configure(appHost);
    }

#if DEBUG
    const string apipath = @"https://www.realtor.com";
#else
  const string apipath = @"https://www.realtor.com";
#endif
  }
}
