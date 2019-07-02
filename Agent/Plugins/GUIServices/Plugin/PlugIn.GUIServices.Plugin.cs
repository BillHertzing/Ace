using Serilog;

using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.VirtualPath;

using System;
using System.Linq;
using ATAP.Utilities.ServiceStack;
using System.IO;
using System.Collections.Generic;
using ServiceStack.Caching;

using Microsoft.Extensions.Hosting;


namespace Ace.Agent.GUIServices {
    public class GUIServicesPlugin : IPlugin, IPreInitPlugin {

        // Surface the configKeyPrefix for this namespace
        public static string myNamespace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;

        public Microsoft.Extensions.Hosting.IHostEnvironment HostEnvironment { get; set; }

        public void Configure(IAppHost appHost) {

            Log.Debug("in GUIServicesPlugin.Configure, appHost = {appHost}", appHost);

            // Load the local property of IHostEnvironment type
            HostEnvironment= appHost.GetContainer().Resolve<Microsoft.Extensions.Hosting.IHostEnvironment>();
            // Determine the environment this PlugIn has been activated in
            string envName = HostEnvironment.EnvironmentName;

            // Populate this PlugIn's AppSettings Configuration Settings and place it in the appSettingsDictionary

            // Location of the Configuration text files are at the ContentRoot. ToDo: figure out how to place them / resolve them relative to the location of the PlugIn assembly

            var pluginAppSettingsBuilder = new MultiAppSettingsBuilder();
            // ToDo: command line settings
            // Environment variables have 2nd highest priority
            // ToDo: specific prefix
            pluginAppSettingsBuilder.AddEnvironmentalVariables();
            // Location of the files are at the ContentRoot. ToDo: figure out how to place them / resolve them relative to the location of the PlugIn assembly
            // Non-Production Configuration settings in a text file
            if (!this.HostEnvironment.IsProduction()) {
                var settingsTextFileName = Ace.Agent.GUIServices.StringConstants.PluginSettingsTextFileName+'.'+envName+StringConstants.PluginSettingsTextFileSuffix;
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

            // ContentRoot from SS, and WebHostRoot (null?)
            // Hack: Get ContentRoot from AppHost somehow, instead of assuming CurrentDirectory
            var contentRoot = Directory.GetCurrentDirectory();

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

            // ToDo: For each GUI in GUIs. Create a linq query, define a static action, loop and map inputs to outputs
            // GUI(s) relative paths from Configuration; Production defaults for at least the first are in the DefaultConfiguration.Production dictionary
            if (!pluginAppSettings.Exists(StringConstants.RelativeToContentRootPathConfigKey)
                ||(pluginAppSettings.GetString(StringConstants.RelativeToContentRootPathConfigKey)==string.Empty)) {
                throw new Exception(StringConstants.RelativeToContentRootPathKeyOrValueNotFoundExceptionMessage);
            }
            var relativeRootPathValue = pluginAppSettings.GetString(StringConstants.RelativeToContentRootPathConfigKey);
            // ToDo: Security: this is a place where character strings from external sources are processed
            // Use these for checking for invalid characters in the appSettings RelativeRootPath values
            char[] invalidChars = Path.GetInvalidPathChars();
            bool valid = !relativeRootPathValue.Any(x => invalidChars.Contains(x));
            if (!valid) {
                throw new Exception(StringConstants.RelativeRootPathValueContainsIlegalCharacterExceptionMessage);
            }
            var physicalPath = Path.Combine(contentRoot, relativeRootPathValue);
            Log.Debug("in GUIServicesPlugin.Configure, physicalPath = {physicalPath}, contentRoot = {contentRoot} relativeRootPathValue = {relativeRootPathValue}", physicalPath, contentRoot, relativeRootPathValue);

            // the VirtualRootPath is relative to initial paths for each GUI. Get it from Configuration;
            string virtualRootPath;
            // verify key exists, else throw exception. String.Empty is allowed
            if (!pluginAppSettings.Exists(StringConstants.VirtualRootPathConfigKey)) {
                throw new Exception(StringConstants.VirtualRootPathKeyOrValueNotFoundExceptionMessage);
            }
            virtualRootPath=pluginAppSettings.GetString(StringConstants.VirtualRootPathConfigKey);

            // Map the Virtual root Dir to the physical path of the root of the GUI
            // Wrap in a try catch block in case the physicalRootPath does not exists
            // ToDo: test for failure condition instead of letting it throw an exception
            try {
                appHost.AddVirtualFileSources
                    .Add(new FileSystemMapping(virtualRootPath, physicalPath));
            }
            catch (Exception e) {
                // ToDo: research how best to log an exception with Serilog, ServiceStack and/or MS
                Log.Debug(e, "in GUIServicesPlugin.Configure, Adding a new VirtualFileSource failed with : {Message}", e.Message);
                // ToDo wrap in an Aggregate when doing loop
                throw;

                // ToDo: figure out how to log this and fallback to something useful
            }
            // This is the end of the 'loop over all GUIs'

            // Blazor requires the delivery of static files ending in certain file suffixes.
            // SS disallows many of these by default, so here we tell SS to allow certain file suffixes
            appHost.Config.AllowFileExtensions.Add("dll");
            appHost.Config.AllowFileExtensions.Add("json");
            appHost.Config.AllowFileExtensions.Add("pdb");

            // per conversation with Myth at SS, the default behavior of a web server, for URI "/" is to return the content of wwwroot/index.html
            // ToDo: better understanding how this is configured to ensure it behaves as expected in all WebMost models
            appHost.Config.DefaultRedirectPath="/index.html";

            // Remove the ServieStack metadata feature, as it overrides the default behavior expected when a request for a root resource arrives, and the root resource iis not found
            appHost.Config.EnableFeatures=Feature.All.Remove(Feature.Metadata);

            // Need to figure out if / how a PlugIn can add a PlugIn to the parent AppHost
/*
            // Blazor requires CORS support, enable the ServiceStack CORS feature
            appHost.Plugins.Add(new CorsFeature(
               allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
               allowedOrigins: "*",
               allowCredentials: true,
               allowedHeaders: "content-type, Authorization, Accept"));
*/
            // create the plugIn's data object
            GUIServicesData gUIServicesData = new GUIServicesData(pluginAppSettings);

            // copy the most recent configuration settings to the Data
            // ToDo: hmm should be a way to make sure the Data has a Property for each configuration setting, and to populate the initial Data with the cache value

            // Pass the Plugin's data structure to the container so it will be available to every other module and services
            appHost.GetContainer().Register<GUIServicesData>(d => gUIServicesData);

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
            appHost.RegisterService<GUIServices>();
            this.Configure(appHost);
        }

        public void BeforePluginsLoaded(IAppHost appHost) { }
    }
}
