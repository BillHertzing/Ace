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
using ATAP.Utilities.ETW;
using ServiceStack.Messaging;
using ServiceStack.RabbitMq;


namespace Ace.PlugIn.AMQPServices {
#if TRACE
    [ETWLogAttribute]
#endif
    public class AMQPServicesPlugin : IPlugin, IPreInitPlugin {

        // Surface the configKeyPrefix for this namespace
        public static string myNamespace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;

        public Microsoft.Extensions.Hosting.IHostEnvironment HostEnvironment { get; set; }

        public void Configure(IAppHost appHost) {

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
            // ToDo: extend appSettingsDictionary to allow for adding a plugIn's AppSettings instance; needs to account for both readonly and ChangeNotify
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

            // ToDo: Confirm the AMQPConnectionString is present and not empty
            var aMQPConnectionString = pluginAppSettings.GetString("AMQPConnectionString");
            Log.Debug("in AMQPServicesPlugin.Configure, AMQPConnectionString = {AMQPConnectionString}", aMQPConnectionString);

            // create the plugIn's data object
            AMQPServicesData aMQPServicesData = new AMQPServicesData(pluginAppSettings);

            // copy the most recent configuration settings to the Data
            // ToDo: hmm should be a way to make sure the Data has a Property for each configuration setting, and to populate the initial Data with the cache value

            // Pass the Plugin's data structure to the container so it will be available to every other module and services
            appHost.GetContainer().Register<AMQPServicesData>(d => aMQPServicesData);

            // ToDo: Get the MQServer to use based on values in the AppSettings
            // Create and register a RabbitMqServer using the connectionString from the AppSettings
            appHost.Register<IMessageService>(new RabbitMqServer(aMQPConnectionString));

            // Register the VerifyAMQPReqDTO message handler to the AMQP server
            var mqServer = appHost.GetContainer().Resolve<IMessageService>();
            //var aMQPServiceController = new AMQPServiceController(appHost);
            mqServer.RegisterHandler<VerifyAMQPReqDTO>(appHost.ExecuteMessage);
            mqServer.RegisterHandler<RecordRootReqDTOMessage>(appHost.ExecuteMessage);
            // Wait for other plugins to initialize, then start the mqServer
            appHost.AfterInitCallbacks.Add(appHost => mqServer.Start());

            // ToDo: enable the mechanisms that monitors each plugin-specific data sensor, and start them running

        }

        /// <summary>
        /// Register this plugin with the appHost
        /// Configure its observableDataStructures and event handlers
        /// </summary>
        /// <param name="appHost">The ASP.Net Host</param>
        public void Register(IAppHost appHost) {
            // ToDo: Create static string for exception message
            if (null==appHost) { throw new ArgumentNullException(nameof(appHost)); }
            this.Configure(appHost);
        }

        public void BeforePluginsLoaded(IAppHost appHost) {
        }
    }
}
