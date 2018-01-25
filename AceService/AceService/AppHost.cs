using Funq;
using ServiceStack;
using Ace.AceService.BaseServiceInterface;
using ServiceStack.Configuration;
using System.Collections.Generic;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using OpenHardwareMonitor.Hardware;
using System.IO;
using System.Linq;

namespace Ace.AceService
{
    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptyWindowService
    public  class AppHost : AppSelfHostBase
    {
        Computer computer;
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
            // inject the concrete logging implementation
            LogManager.LogFactory = new NLogFactory();

            // Create the overall application instance of the configuration settings
            // The location should be part of the container IOC injection, (hopefully observable and creating a stream that can be subscribed to by an authorized  client of this service )

            var appSettingsBuilder = new MultiAppSettingsBuilder()
                 // Start with any appsettings in the App.config file AKA AceAgent.exe.config at runtime
                 .AddAppSettings()
                // Add the AceService.BaseService builtin (compile-time) configuration settings
                .AddDictionarySettings(DefaultConfiguration.GetIt())
                // Add (Superseding any previous values) the optional configuration file for BaseService configuration settings AKA AceAgent.config
                .AddTextFile("./AceService.config");
            //.AddTextFile(AppDomain.CurrentDomain.BaseDirectory ".config"));

            var htmlFormat = base.Plugins.First(x => x is ServiceStack.Formats.HtmlFormat) as ServiceStack.Formats.HtmlFormat;

            // ToDo Validate any plugin settings in the configuration settings
            var plugInList = new List<IPlugin>() { new Ace.AceService.MinerServicePlugin.MinerServicePlugin() };
            // Add configuration settings () specific to a plugin
            foreach (var  pl in plugInList)
            {
                // ToDo: Add the plugIns' builtin (compile-time) configuration settings
                //appSettingsBuilder
                // Superseded by an optional configuration file that contains settings for the plugin
            }
            appSettingsBuilder.AddTextFile("./AceService.MinerPlugin.config");

            // ToDo Superseded by an optional configuration file that contains 'recently used' configuration settings
            // Superseded by Environment variables
            appSettingsBuilder.AddEnvironmentalVariables();
                // ToDo Superseded by command line variables
                // ToDo Superseded by command line variables
                // ToDo Validate the final set of settings
                // Validate config file location, Logs file location
                // Build the final appsettings
            AppSettings = appSettingsBuilder.Build();
            // ToDo eventually consider making the computer hw monitor into a plugin
            computer = new Computer
            {
                MainboardEnabled = true,
                CPUEnabled = true,
                //GPUEnabled = true
            };
            computer.Open();

            
            Plugins.Add(new Ace.AceService.MinerServicePlugin.MinerServicePlugin());
            
            // ToDo place a static, deep-copy of the current application'instance of the configuration settings as the first object in the application's configuration settings history list.

            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());
            
        }
        public override void Stop()
        {
            computer.Close();
            base.Stop();
        }

    }
}
