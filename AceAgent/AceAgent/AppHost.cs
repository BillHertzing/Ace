using Funq;
using ServiceStack;
using AceAgent.BaseServiceInterface;
using ServiceStack.Configuration;
using System.Collections.Generic;

namespace AceAgent
{
    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptyWindowService
    public class AppHost : AppSelfHostBase
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
            // Create the overall application instance of the configuration settings
            // The location should be part of the container IOC injection, (hopefully observable and creating a stream that can be subscribed to by an authorized  client of this service )
            AppSettings = new MultiAppSettingsBuilder()
                 .AddAppSettings()
                // BaseService Builtin Configuration properties and values.
                .AddDictionarySettings(new Dictionary<string, string>() {
                    { "AceService:UserName", System.Environment.UserName } ,
                    { "AceService:ConfigFilePath", "./config.txt"}, //ToDo get from registry or from "$ENV:ProgramData/Ace/"
                    { "AceGUI::MainWindow:Height", "400"},
                    { "AceGUI::MainWindow:Width", "600"},
                    { "AceGUI::MainWindow:Top", "0"},
                    { "AceGUI::MainWindow:Left", "0"}
                })
                // Superseded by an optional configuration file for the base Services' configuration settings
                // .AddTextFile("./AceServiceSettings.txt".MapProjectPath())
                // Superseded by an optional configuration file that contains 'recently used' configuration settings
                //.AddTextFile("./AceServiceLastUsedSettings.txt".MapProjectPath())
                // Superseded by Environment variables
                .AddEnvironmentalVariables()
                // ToDo Superseded by command line variables
                // ToDo Superseded by command line variables
                .Build();
            // Add any plugins specified in the configuration
            // Add that plugin's configuration properties to the current application'instance of the configuration settings
            // place a static, deep-copy of the current application'instance of the configuration settings as the first object in the application's configuration settings history list.

            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());
        }
    }
}
