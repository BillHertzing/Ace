using Funq;
using ServiceStack;
using Ace.AceService.BaseServiceInterface;
using ServiceStack.Configuration;
using System.Collections.Generic;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;

namespace Ace.AceService
{
    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptyWindowService
    public  class AppHost : AppSelfHostBase
    {
        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        public AppHost()
            : base("AceService", typeof(BaseServices).Assembly) { }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container)
        {
            //inject the concrete logging implementation
            LogManager.LogFactory = new NLogFactory();

            // Create the overall application instance of the configuration settings
            // The location should be part of the container IOC injection, (hopefully observable and creating a stream that can be subscribed to by an authorized  client of this service )
            AppSettings = new MultiAppSettingsBuilder()
                // Start with any appsettings in the app.config file
                 .AddAppSettings()
                // BaseService Builtin Configuration properties and values.
                .AddDictionarySettings(DefaultConfiguration.GetIt())
                // Superseded by an optional configuration file for the base Services' configuration settings
                 .AddTextFile("./AceService.BaseService.Settings.txt")
                // Superseded by an optional configuration file that contains settings for the miningRig plugin
                .AddTextFile("./AceService.MinerPlugin.Settings.txt")
                // ToDo Superseded by an optional configuration file that contains 'recently used' configuration settings
                // Superseded by Environment variables
                .AddEnvironmentalVariables()
                // ToDo Superseded by command line variables
                // ToDo Superseded by command line variables
                .Build();
            // ToDo Validate the final set of settings
            // Validate config file location, Logs file location
            // Add any plugins specified in the configuration
            // Add that plugin's configuration properties to the current application'instance of the configuration settings
            Plugins.Add(new Ace.AceService.MinerServicePlugin.MinerServicePlugin());
            // ToDo Validate the final set of settings for the plugin
            // ToDo place a static, deep-copy of the current application'instance of the configuration settings as the first object in the application's configuration settings history list.

            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());
            
        }
    }
}
