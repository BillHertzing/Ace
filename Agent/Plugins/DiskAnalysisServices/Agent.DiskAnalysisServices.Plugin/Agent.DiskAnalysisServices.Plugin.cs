using System;
using System.Collections;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Caching;
using Swordfish.NET.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.ComponentModel;
using ServiceStack.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Ace.Agent.DiskAnalysisServices
{
  public class DiskAnalysisServicesPlugin : IPlugin, IPreInitPlugin
  {
    #region string constants
    #region Configuration Key strings
    #endregion Configuration Key strings
    #region Exception Messages (string constants)
    #endregion Exception Messages (string constants)
    #region File Name string constants
    const string pluginSettingsTextFileName = "Agent.DiskAnalysisServices.settings.txt";
    #endregion File Name string constants
    #endregion string constants

    // Create a logger for this class
    public static ILog Log = LogManager.GetLogger(typeof(DiskAnalysisServicesPlugin));

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
      Log.Debug("starting DiskAnalysisServicesPlugin.Configure");

      // Populate this Plugin's Application Configuration Settings
      // Location of the files will depend on running as LifeCycle Production/QA/Dev as well as Debug and Release settings
      var pluginAppSettings =new MultiAppSettingsBuilder()
    // Command line flags have highest priority
    // next in priority are  Environment variables
    //.AddEnvironmentalVariables()
    // next in priority are Configuration settings in a text file relative to the current working directory at the point in time when this method executes.
    .AddTextFile(pluginSettingsTextFileName)
    // Builtin (compiled in) have the lowest priority
    .AddDictionarySettings(DefaultConfiguration.Configuration())
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


      // Populate this Plugin's Gateways
      // Location of the files will depend on running as LifeCycle Production/QA/Dev as well as Debug and Release settings
      var pluginGateways = new MultiGatewaysBuilder()
    // Command line flags have highest priority
    // next in priority are  Environment variables
    //.AddEnvironmentalVariables()
    // next in priority are Configuration settings in a text file relative to the current working directory at the point in time when this method executes.
    //.AddTextFile(pluginGatewaysTextFileName)
    // Builtin (compiled in) have the lowest priority
    //.AddDictionarySettings(DefaultGateways.Configuration())
    .Build();



      // Create a Gateways collection from the txt file
      ConcurrentObservableDictionary<string, IGateway> gateways = new ConcurrentObservableDictionary<string, IGateway>();
      gateways.Add("GoogleMapsGeoCoding", new GatewayBuilder().Build());
      // Create the Plugin's data structure. There should only be a single instance.
      // Every Property matching a ConfigKey gets/sets the value of the matching ConfigKey in the cache
      // ConfigKey Properties do not have to be set in the constructor because the cache was setup before calling the constructor


      DiskAnalysisServicesData DiskAnalysisServicesData = new DiskAnalysisServicesData(appHost, new ConcurrentObservableDictionary<string, decimal>(), onPluginRootCollectionChanged, onPluginRootPropertyChanged);

      // copy the most recent configuration settings to the Data
      // hmm should be a way to make sure the Data has a Property for each configuration setting, and to populate the initial Data with the cache value
      DiskAnalysisServicesData.GoogleAPIKeyEncrypted = cacheClient.Get<string>(configKeyPrefix + "GoogleAPIKeyEncrypted");
      DiskAnalysisServicesData.HomeAwayAPIKeyEncrypted = cacheClient.Get<string>(configKeyPrefix + "HomeAwayAPIKeyEncrypted");
      DiskAnalysisServicesData.HomeAway_API_URI = cacheClient.Get<string>(configKeyPrefix + "HomeAway_API_URI");
      DiskAnalysisServicesData.Google_API_URI = cacheClient.Get<string>(configKeyPrefix + "Google_API_URI");
      DiskAnalysisServicesData.UriHomeAway_API_URI = cacheClient.Get<Uri>(configKeyPrefix + "UriHomeAway_API_URI");
      DiskAnalysisServicesData.UriGoogle_API_URI = cacheClient.Get<Uri>(configKeyPrefix + "UriGoogle_API_URI");

      // and pass the Plugin's data structure to the container so it will be available to every other module and services
      appHost.GetContainer()
      .Register<DiskAnalysisServicesData>(x => DiskAnalysisServicesData);




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
      Log.Debug("starting DiskAnalysisServicesPlugin.Register");

      if (null == appHost) { throw new ArgumentNullException("appHost"); }
      appHost.RegisterService<DiskAnalysisServices>();
      this.Configure(appHost);
    }

  }
}
