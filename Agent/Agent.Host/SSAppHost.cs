using Ace.Agent.BaseServices;
using Ace.Agent.GUIServices;
using Ace.Agent.DiskAnalysisServices;
using Ace.Agent.MinerServices;
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
using Microsoft.AspNetCore.Hosting;
using Ace.Agent.RealEstateServices;

namespace Ace.Agent.Host {
    public class SSAppHost : AppHostBase {
        #region string constants default file names and Exception messages
        #region string constants: File Names 
        public const string settingsTextFileNameSuffix = ".txt";
        public const string sSAppHostSettingsTextFileNameString = "SSAppHostSettings";
        public const string agentSettingsTextFileNameString = "Agent.BaseServicesSettings";
        //It would be nice if ServiceStack implemented the User Secrets pattern that ASP Core provides
        // Without that, the following string constant identifies an Environmental variable that can be populated with the name of a file
        public const string agentEnvironmentIndirectSettingsTextFileNameKey = "Agent.BaseServices.IndirectSettings.Path";
        #endregion
        #region string constants: Exception Messages
        public const string cannotReadEnvironmentVariablesSecurityExceptionMessage = "Ace cannot read from the environment variables (Security)";
        public const string CouldNotCreateServiceStackVirtualFileMappingExceptionMessage = "Could not create ServiceStack Virtual File Mapping: ";
        #endregion
        public const string PhysicalRootPathConfigKey = "PhysicalRootPath";
        #endregion

        // Make the WebHostEnvironment available to this class, use constructor injection to populate it
        public IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// Inject an implementation of an IWebHostEnvironment via the constructor
        /// </summary>
        //public SSAppHost() : base("BaseServices", typeof(BaseServices.BaseServices).Assembly) {
        public SSAppHost(IWebHostEnvironment webHostEnvironment) : base("BaseServices", typeof(BaseServices.BaseServices).Assembly) {
            Log.Debug("Entering SSAppHost Ctor");
            WebHostEnvironment=webHostEnvironment;
            //Log.Debug("in SSAppHost .ctor, base.Configuration = {Configuration}", base.Configuration);
            Log.Debug("Leaving SSAppHost Ctor");
        }


        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources, in particular the AppSettings.
        /// </summary>
        public override void Configure(Container container) {
            Log.Debug("Entering SSAppHost.Configure method");

            //Log.Debug("in SSAppHost.Configure method, container is {container}", container);

            // Bring into AppSettings the ConfigurationRoot from the genericHost and get the Environment value
            IAppSettings hostingAppSettings = new NetCoreAppSettings(Configuration);
            //string envName = WebHostEnvironment.EnvironmentName;  // hack
            string envName = "Development";
            Log.Debug("in SSAppHost.Configure, envName = {envName}", envName);

            // AppSettings is a first class object on the Container, so it will be auto-wired
            // In any other assembly, AppSettings is read-only, so it must be populated in this assembly
            // SSAppHost is hosted within a WebServerHost. Location of all configuration files for SSAppHost will be relative and under the ContentRoot of the WebServerHost

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
            // Location of the text file is relative to the current working directory at the point in time when this method executes.
            // If this file does not exist, it is not considered an error, but if it does exist, not having read permission is an error
            // ToDo: if a configuration settings text file is specified as a constant string in the app, ensure it exists and the ensure we have permission to read it
            // ToDo: Security: There is something called a Time-To-Check / Time-To-Use vulnerability, ensure the way we check then access the text file does not open the program to this vulnerability
            // ToDo: if it exists, append AddTextFile for configuration settings in a text file specified as a constant string in the app to the AppSettingsBuilder
            // ToDo: Investigate SS's MapHostAbsolutePath
            //  new TextFileSettings("~/appsettings.txt".MapHostAbsolutePath(),
            if (!HostingEnvironment.IsProduction()) {
                var settingsTextFileName = agentSettingsTextFileNameString+'.'+envName+settingsTextFileNameSuffix;
                // ToDo: ensure it exists and the ensure we have permission to read it
                // ToDo: Security: There is something called a Time-To-Check / Time-To-Use vulnerability, ensure the way we check then access the text file does not open the program to this vulnerability
                multiAppSettingsBuilder.AddTextFile(settingsTextFileName);
            }
            // Add the production Agent.BaseServices settings text file if it exists
            // ToDo: ensure it exists and the ensure we have permission to read it
            // ToDo: Security: There is something called a Time-To-Check / Time-To-Use vulnerability, ensure the way we check then access the text file does not open the program to this vulnerability
            multiAppSettingsBuilder.AddTextFile(agentSettingsTextFileNameString+settingsTextFileNameSuffix);
            // Next in priority are the SSAppHost settings text files. Environment-specific settings text files have a higher priority than the default (production) settings text files
            if (!HostingEnvironment.IsProduction()) {
                var settingsTextFileName = sSAppHostSettingsTextFileNameString+'.'+envName+settingsTextFileNameSuffix;
                // ToDo: ensure it exists and the ensure we have permission to read it
                // ToDo: Security: There is something called a Time-To-Check / Time-To-Use vulnerability, ensure the way we check then access the text file does not open the program to this vulnerability
                multiAppSettingsBuilder.AddTextFile(settingsTextFileName);
            }
            // Add the production SSAppHost settings text file if it exists
            // ToDo: ensure it exists and the ensure we have permission to read it
            // ToDo: Security: There is something called a Time-To-Check / Time-To-Use vulnerability, ensure the way we check then access the text file does not open the program to this vulnerability
            multiAppSettingsBuilder.AddTextFile(sSAppHostSettingsTextFileNameString+settingsTextFileNameSuffix);
            // ToDo: Investigate the web.config file and see when it makes sense to include it in the genericHost ConfigurationRoot
            // ToDo: Investigate the .exe.config file and see when it makes sense to include it in the genericHost ConfigurationRoot
            // multiAppSettingsBuilder.AddAppSettings()
            // Next in priority are Built-in (compiled in) Production configuration settings for Agent.BaseServices
            multiAppSettingsBuilder.AddDictionarySettings(Ace.Agent.BaseServices.DefaultConfiguration.Production);
            // Next in priority are Built-in (compiled in) Production configuration settings for SSAppHost
            multiAppSettingsBuilder.AddDictionarySettings(SSAppHostDefaultConfiguration.Production);
            // Last in priority are the AppSettings inherited from genericHost
            //multiAppSettingsBuilder.AddNetCore(Configuration);

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
               new RealEstateServicesPlugin(),
               new MinerServicesPlugin(),
               new DiskAnalysisServicesPlugin(),
               new GUIServicesPlugin()
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

            Log.Debug("Leaving SSAppHost.Configure");
        }

        /* this was part of teh now obiolete "winForms" way of doing a notify icon
         * // ToDo: Findout how to display a Notify Icon, and respond to an 'exit' selection 
        // Stop the entire program
        void menuItem1_Click(object Sender, EventArgs e) {
            Log.Debug("AceCommander NotifyIcon menuItem1_Click event handler started");
            this.Stop();
            Log.Debug("AceCommander NotifyIcon menuItem1_Click event handler started");
        }
        */

        /* in ASP.Net Core, there must be some other method or event handler used to shutdown middleware
         *  // ToDo: Findout how to shutdown SS gracefully ad dispose of it's stuff
        /// <summary>
        /// Shut down the SS Middleware
        /// </summary>
        public override void Stop() {
            Log.Debug("Entering SSAppHost Stop Method");
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
            Log.Debug("Leaving SSAppHost Stop Method");
        }

        */

        // Get the latest known current configuration, and use that information to populate the data structures
        //ToDo: computerInventory = new ComputerInventory(AppSettings.GetAll());

        // validate that the configuration settings match the real computer inventory

        // if the current computer inventory specifies that there are sensors that
        // can and should be monitored, attach the event handlers that will respond to changes in the monitored data structures

        // setup and enable the mechanisms that monitors each monitored sensor, and start them running
        // ToDo: container.Register<ComputerInventory>(c => computerInventory);
        // if the current computer inventory specifies that there are sensors that are being monitored, dispose of the resources that are doing the monitoring
        //ComputerInventory computerInventory = container.TryResolve(typeof(ComputerInventory)) as ComputerInventory;
    }
}
