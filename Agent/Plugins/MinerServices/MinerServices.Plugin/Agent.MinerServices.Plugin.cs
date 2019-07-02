
using Serilog;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Caching;
using Swordfish.NET.Collections;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

using ATAP.Utilities.ServiceStack;

using Microsoft.Extensions.Hosting;

using ATAP.Utilities.Http;
using System.IO;

namespace Ace.Agent.MinerServices {

    public class MinerServicesPlugin : IPlugin, IPreInitPlugin {

        // Surface the configKeyPrefix for this namespace
        public static string myNamespace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;

        public Microsoft.Extensions.Hosting.IHostEnvironment HostEnvironment { get; set; }

        // Declare Event Handlers for the Plugin Root COD
        // These event handler will be attached/detached from the ObservableConcurrentDictionary via that class' constructor and dispose method
        public void onPluginRootCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            Log.Debug($"event onPluginRootCollectionChanged, args e = {e}");
            // send a SSE message to all subscribers
        }

        public void onPluginRootPropertyChanged(object sender, PropertyChangedEventArgs e) {
            Log.Debug($"event onPluginRootPropertyChanged, args e = {e}");
            // send a SSE message to all subscribers
        }

        public void Configure(IAppHost appHost) {

            Log.Debug("in MinerServicesPlugin.Configure, appHost = {appHost}", appHost);

            // Load the local property of IHostEnvironment type
            HostEnvironment=appHost.GetContainer().Resolve<Microsoft.Extensions.Hosting.IHostEnvironment>();
            // Determine the environment this PlugIn has been activated in
            string envName = HostEnvironment.EnvironmentName;

            // Populate this PlugIn's AppSettings Configuration Settings and place it in the appSettingsDictionary

            // Location of the files are at the ContentRoot. ToDo: figure out how to place them / resolve them relative to the location of the PlugIn assembly

            var pluginAppSettingsBuilder = new MultiAppSettingsBuilder();
            // ToDo: command line settings
            // Environment variables have 2nd highest priority
            // ToDo: specific prefix
            pluginAppSettingsBuilder.AddEnvironmentalVariables();
            // Location of the files are at the ContentRoot. ToDo: figure out how to place them / resolve them relative to the location of the PlugIn assembly
            // Non-Production Configuration settings in a text file
            if (!this.HostEnvironment.IsProduction()) {
                var settingsTextFileName = StringConstants.PluginSettingsTextFileName+'.'+envName+StringConstants.PluginSettingsTextFileSuffix;
                // ToDo: ensure it exists and the ensure we have permission to read it
                // ToDo: Security: There is something called a Time-To-Check / Time-To-Use vulnerability, ensure the way we check then access the text file does not open the program to this vulnerability
                pluginAppSettingsBuilder.AddTextFile(settingsTextFileName);
            }
            // Production Configuration settings in a text file
            pluginAppSettingsBuilder.AddTextFile(StringConstants.PluginSettingsTextFileName+StringConstants.PluginSettingsTextFileSuffix);
            // BuiltIn (compiled in) have the lowest priority
            pluginAppSettingsBuilder.AddDictionarySettings(DefaultConfiguration.Production);

            // Create the appSettings from the builder
            var pluginAppSettings = pluginAppSettingsBuilder.Build();

            // Register them so other PlugIns can see them
            AppSettingsDictionary appSettingsDictionary = appHost.TryResolve<AppSettingsDictionary>();
            if (appSettingsDictionary==null) {
                // ToDo: Log:Error
                throw new InvalidOperationException("ToDo: exception message");
            }
            // ToDo: extend appSettingsDictionary to allow for adding a plugIn's AppSettings instance; needs to account for both readonly and changenotify
            //appSettingsDictionary.

            // Key names in the cache for Configuration settings for a Plugin consist of the namespace and the string .Config
            // Key names in the appSettings of a Plugin for Configuration settings may or may not have the prefix
            // Compare the two lists of Configuration setting keys...

            // create a copy of the keys from AppSettings ensuring all are prefixed with the namespace and .Config
            var configKeyPrefix = myNamespace+".Config.";
            var lengthConfigKeyPrefix = configKeyPrefix.Length;
            var appSettingsConfigKeys = pluginAppSettings.GetAllKeys();
            var fullAppSettingsConfigKeys = appSettingsConfigKeys.Select(x => x.IndexOf(configKeyPrefix)>=0 ? x : configKeyPrefix+x);

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

            // create the plugIn's data object
            MinerServicesData minerServicesData = new MinerServicesData(appHost, new ConcurrentObservableDictionary<string, decimal>(), onPluginRootCollectionChanged, onPluginRootPropertyChanged);

            // copy the most recent configuration settings to the Data
            // ToDo: hmm should be a way to make sure the Data has a Property for each configuration setting, and to populate the initial Data with the cache value
            // current computer inventory
            // Get the latest known current configuration, and use that information to populate the data structures
            // appHost.AppSettings.Get<string>("Ace.AceService.MinerPlugin.InstalledMinerSW");
            // appHost.AppSettings.Get<string>("Ace.AceService.MinerPlugin.InstalledGPUs");

            // Pass the Plugin's data structure to the container so it will be available to every other module and services
            appHost.GetContainer().Register<MinerServicesData>(d => minerServicesData);

            // ToDo: enable the mechanisms that monitors each plugin-specific data sensor, and start them running

        }

        /// <summary>
        /// Register this plugin with the appHost
        /// Configure its observableDataStructures and event handlers
        /// </summary>
        /// <param name="appHost">The ASP.Net Host</param>
        public void Register(IAppHost appHost) {
            // ToDo: Create static string for exception message
            if (null==appHost) { throw new ArgumentNullException("appHost"); }
            appHost.RegisterService<Ace.Agent.MinerServices.MinerServicesPlugin>();
            this.Configure(appHost);
        }
        public void BeforePluginsLoaded(IAppHost appHost) { }

    }
}
