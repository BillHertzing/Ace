using Ace.Agent.BaseServices;
using Ace.Agent.GUIServices;
using Ace.Plugin.DiskAnalysisServices;
using Ace.Plugin.MinerServices;
using ATAP.Utilities.ETW;
using ATAP.Utilities.ServiceStack;
using Funq;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Security;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using Ace.Plugin.RealEstateServices;

namespace Ace.Agent.Host {
    [ATAP.Utilities.ETW.ETWLogAttribute]
    public class SSAppHost : AppHostBase {

        // Make the HostEnvironment available to this class, use constructor injection to populate it
        public IHostEnvironment HostEnvironment { get; }

        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// Inject an implementation of an IWebHostEnvironment via the constructor
        /// </summary>
        public SSAppHost(IHostEnvironment hostEnvironment) : base("BaseServices", typeof(BaseServices.BaseServices).Assembly) {
            HostEnvironment=hostEnvironment;
            // Load it into the Container
            Container.AddSingleton<IHostEnvironment>(HostEnvironment);
        }


        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources, in particular the AppSettings.
        /// </summary>
        public override void Configure(Container container) {

            // get the Environment value from the WebHostenvironment injected by the constructor
            string envName = this.HostEnvironment.EnvironmentName;  
            Log.Debug("in SSAppHost.Configure, envName = {envName}", envName);

            // AppSettings is a first class object on the Container, so it will be auto-wired
            // In every other assembly, AppSettings is read-only, so it must be populated in this assembly
            // SSAppHost is hosted within a WebServerHost hosted within a GenericHost, so the location of all configuration files for SSAppHost will be under the ContentRoot of the WebServerHost

            // ToDo: It would be nice if ServiceStack implemented the User Secrets pattern that ASP Core provides
            /*
			// ToDo: Implement an indirect Settings.txt file feature allowing for configuration values in a file specified on command line or environment variables
            // The name and path to an external text file to add to the AppSettings may be specified in an Environment variable or command line argument
            string indirectSettingsTextFilepath;
            try {
                indirectSettingsTextFilepath=Environment.GetEnvironmentVariable(agentEnvironmentIndirectSettingsTextFileNameKey);
            }
            catch (SecurityException e) {
                Log.Error($"{cannotReadEnvironmentVariablesSecurityExceptionMessage}: {e.Message}");
                throw new SecurityException(cannotReadEnvironmentVariablesSecurityExceptionMessage, e);
            }

            // ToDo: If the command line or environment variables do define a location for another text fle, ensure it can be read
            if (indirectSettingsTextFilepath!=null) {
                try { }
                catch { }
            }
			*/

            // Create the MultiAppSettingsBuilder
            var multiAppSettingsBuilder = new Ace.Agent.Host.MultiAppSettingsBuilder();
            // Highest priority is any command line variable values, so add command line arguments to the AppSettingsBuilder
            // ToDo: Implement command line variables by getting from the .Net Core ConfigurationRoot command line provider and putting them here at high priority
            // Next in priority are all environment values with a specific prefix, so append AddEnvironmentalVariables to the AppSettingsBuilder
            // ToDo: implement an optional prefix filter to SS's AddEnvironmentalVariables provider, then get just those environment variables that match the prefix
            // ToDo: implement a settings provider that combines the the .Net Core ConfigurationRoot environment variable provider's data with the modified AddEnvironmentalVariables provider
            multiAppSettingsBuilder.AddEnvironmentalVariables();
            // Next in priority are any configuration settings in a text file named on the command line.
            // ToDo: if a configuration settings text file is specified on the command line, ensure we have permission to read it
            // ToDo: append AddTextFile for configuration settings in a text file specified on the command line to the AppSettingsBuilder
            // if (indirectSettingsTextFilepathCommandLine!=null) { multiAppSettingsBuilder.AddTextFile(indirectSettingsTextFilepathCommandLine); }
            // Next in priority are contents of any indirect files mentioned in the environment variables (e.g. User Secrets )
            // ToDo: append AddTextFile for configuration settings in a text file specified in an environment variable
            // if (indirectSettingsTextFilepath!=null) { multiAppSettingsBuilder.AddTextFile(indirectSettingsTextFilepath); }
            // Next in priority are the Agent.BaseServices settings text files. Environment-specific settings text files have a higher priority than the default (production) settings text files
            // Location of the text file is relative to the ContentRoot of the WebServerHost.
            // If this file does not exist, it is not considered an error, but if it does exist, not having read permission is an error
            // ToDo: if a configuration settings text file is specified as a constant string in the app, ensure it exists and the ensure we have permission to read it
            // ToDo: Security: There is something called a Time-To-Check / Time-To-Use vulnerability, ensure the way we check then access the text file does not open the program to this vulnerability
            // ToDo: if it exists, append AddTextFile for configuration settings in a text file specified as a constant string in the app to the AppSettingsBuilder
            // ToDo: Investigate SS's MapHostAbsolutePath
            //  new TextFileSettings("~/appsettings.txt".MapHostAbsolutePath(),
            if (!this.HostEnvironment.IsProduction()) {
                var settingsTextFileName = StringConstants.agentSettingsTextFileName + '.' + envName + StringConstants.settingsTextFileSuffix;
                // ToDo: ensure it exists and the ensure we have permission to read it
                // ToDo: Security: There is something called a Time-To-Check / Time-To-Use vulnerability, ensure the way we check then access the text file does not open the program to this vulnerability
                multiAppSettingsBuilder.AddTextFile(settingsTextFileName);
            }
            // Add the production Agent.BaseServices settings text file if it exists
            // ToDo: ensure it exists and the ensure we have permission to read it
            // ToDo: Security: There is something called a Time-To-Check / Time-To-Use vulnerability, ensure the way we check then access the text file does not open the program to this vulnerability
            multiAppSettingsBuilder.AddTextFile(StringConstants.agentSettingsTextFileName+StringConstants.settingsTextFileSuffix);
            // Next in priority are the SSAppHost settings text files. Environment-specific settings text files have a higher priority than the default (production) settings text files
            if (!this.HostEnvironment.IsProduction()) {
                var settingsTextFileName = StringConstants.sSAppHostSettingsTextFileName+'.'+envName+StringConstants.settingsTextFileSuffix;
                // ToDo: ensure it exists and the ensure we have permission to read it
                // ToDo: Security: There is something called a Time-To-Check / Time-To-Use vulnerability, ensure the way we check then access the text file does not open the program to this vulnerability
                multiAppSettingsBuilder.AddTextFile(settingsTextFileName);
            }
            // Add the production SSAppHost settings text file if it exists
            // ToDo: ensure it exists and the ensure we have permission to read it
            // ToDo: Security: There is something called a Time-To-Check / Time-To-Use vulnerability, ensure the way we check then access the text file does not open the program to this vulnerability
            multiAppSettingsBuilder.AddTextFile(StringConstants.sSAppHostSettingsTextFileName+StringConstants.settingsTextFileSuffix);
            // ToDo: Investigate the web.config file and see when it makes sense to include it in the genericHost ConfigurationRoot
            // ToDo: Investigate the .exe.config file and see when it makes sense to include it in the genericHost ConfigurationRoot
            // multiAppSettingsBuilder.AddAppSettings()
            // Next in priority are Built-in (compiled in) Production configuration settings for Agent.BaseServices
            multiAppSettingsBuilder.AddDictionarySettings(Ace.Agent.BaseServices.DefaultConfiguration.Production);
            // Next in priority are Built-in (compiled in) Production configuration settings for SSAppHost
            multiAppSettingsBuilder.AddDictionarySettings(SSAppHostDefaultConfiguration.Production);
            // Last in priority are the AppSettings inherited from genericHost
            multiAppSettingsBuilder.AddNetCore(Configuration);

            //Build the AppSettings that is the first-class citizen on the SSAppHost, and available to all SS Middleware via the ``
            AppSettings=multiAppSettingsBuilder.Build();

            // Create the BaseServices data structure and register it in the container
            //  The SSAppHost (here, ServiceStack running as a Windows service) has some configuration that is common
            //  to both Frameworks (.Net and .Net Core), which will be setup in a common assembly, so this instance of
            //  the appHost is being passed to the BaseServicesData constructor.
            //  this also registers a BaseServicesData instance in the container 
            // ToDo: implement Singleton pattern for BaseServicesData in the DI Container
            var baseServicesData = new BaseServicesData(this);
            container.Register<BaseServicesData>(c => baseServicesData);

            // Create an empty AppSettingsDictionary for plugins and register it
            AppSettingsDictionary appSettingsDictionary = new AppSettingsDictionary();
            container.Register<AppSettingsDictionary>(c => appSettingsDictionary);

            // ToDo: Get the list of plugin names to install from the configuration settings
            // ToDo: probe for assemblies that have a class that matches the name and implements IPlugIn
            // ToDo: Load the matching assemblies into the runspace
            // ToDo: Security: ensure the assemblies being loaded matches some external registry of SHA codes to prove non-tamperable and authenticity
            // ToDo: Add each PlugIn to the SSAppHost
            // Create the list of PlugIns to load
            var plugInList = new List<IPlugin>() {
               new GUIServicesPlugin(),
               new RealEstateServicesPlugin(),
               new MinerServicesPlugin(),
               new DiskAnalysisServicesPlugin(),
            };

            // Load each plugin here. 
            foreach (var pl in plugInList) {
                Plugins.Add(pl);
            }

            // ToDo: See Issue #8
            // ToDo place a static, deep-copy of the current application'instance of the configuration settings as the first object in the application's configuration settings history list.

            // start all the timers
            Log.Debug("In SSAppHost.Configure: starting all timers");
            var timers = Container.Resolve<Dictionary<string, System.Timers.Timer>>();
            var longRunningTasksCheckTimer = timers[Ace.Agent.BaseServices.BaseServicesData.LongRunningTasksCheckTimerName];
            longRunningTasksCheckTimer.Start();

            /* 
            // ToDo: create a NotifyIcon equivalent for a Windows Service or Linux Daemon. Notifiy Icon itself will not work, as that is for WinForms only
            Log.Debug("in SSAppHost.Configure: Create a NotifyIcon for AceCommander");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            NotifyIcon notifyIcon1 = new NotifyIcon();
            ContextMenu contextMenu1 = new ContextMenu();
            MenuItem menuItem1 = new MenuItem();
            contextMenu1.MenuItems
                .AddRange(new MenuItem[] { menuItem1 });
            menuItem1.Index=0;
            menuItem1.Text="E&xit";
            menuItem1.Click+=new EventHandler(menuItem1_Click);
            notifyIcon1.Icon=new Icon("atap.ico");
            notifyIcon1.Text="AceCommander";
            notifyIcon1.ContextMenu=contextMenu1;
            notifyIcon1.Visible=true;
            // Log.Debug("Calling a Web Forms Application instance's static Run method");
            // Application.Run();
            // notifyIcon1.Visible = false;
            Log.Debug("NotifyIcon for AceCommander created");
            */
        }

        /* this was part of teh now obsolete "winForms" way of doing a notify icon
        // ToDo: Findout how to display a Notify Icon, and respond to an 'exit' selection 
        // Stop the entire program
        void menuItem1_Click(object Sender, EventArgs e) {
            Log.Debug("AceCommander NotifyIcon menuItem1_Click event handler started");
            this.Stop();
            Log.Debug("AceCommander NotifyIcon menuItem1_Click event handler started");
        }
        */

        /* in ASP.Net Core, there must be some other method or event handler used to shutdown middleware
        // ToDo: Findout how to shutdown SS gracefully ad dispose of it's stuff
        /// <summary>
        /// Shut down the SS Middleware
        /// </summary>
        public override void Stop() {
            var container = base.Container;

            // It is possible that the Stop method is called during service startup, because the service could be failing because of a problem during startup,
            //  so need to check that each of container's disposable items actually exist before disposing of them

            // iterate everything in the Container that implements IDisposable, and dispose of it

            // Stop and dispose of all timers
            Dictionary<string, System.Timers.Timer> timers;
            try {
                Log.Debug("In SSAppHost.Stop method, trying to resolve the timers dictionary");
                timers=container.TryResolve(typeof(Dictionary<string, System.Timers.Timer>)) as Dictionary<string, System.Timers.Timer>;
            }
            catch (Exception ex) {
                Log.Debug($"In SSAppHost.Stop method, resolving the timers dictionary threw exception message: {ex.Message}");
                throw;
            }

            foreach (var t in timers.Values) {
                t.Stop();
                t.Dispose();
            }
            // call the ServiceStack AppHostBase Stop method
            Log.Debug("Entering the ServiceStack AppHostBase Stop Method");
            base.Stop();
        }

        */
    }
}
