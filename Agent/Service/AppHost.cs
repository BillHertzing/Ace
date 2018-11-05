using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Ace.AceCommon.Plugin.RealEstateSearchServices;
using Ace.Agent.GUIServices;
using Ace.AceService.MinerServices.Plugin;
using Funq;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Redis;
using Ace.AceAgent.BaseServices;

namespace Ace.AceService {
    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptyWindowService
    public class AppHost : AppSelfHostBase {
        static readonly ILog Log = LogManager.GetLogger(typeof(AppHost));
    #region File Name string constants
    public const string agentSettingsTextFileNameString = "Agent.settings.txt";
    #endregion File Name string constants
    List<Task> longRunningTaskList;
        Dictionary<string, System.Timers.Timer> timers;
        public AppHost() : base("AceService", typeof(BaseServices.Interfaces.BaseServices).Assembly) {
            Log.Debug("Entering AppHost Ctor");
            Log.Debug("Leaving AppHost Ctor");
        }

        void LongRunningTasksCheckTimer_Elapsed(object sender, ElapsedEventArgs e) {
            //Log.Debug("Entering the appHost.LongRunningTasksCheckTimer_Elapsed Method");
            var container = base.Container;
            Dictionary<string, System.Timers.Timer> timers = container.TryResolve(typeof(Dictionary<string, System.Timers.Timer>)) as Dictionary<string, System.Timers.Timer>;
            timers["longRunningTasksCheckTimer"].Stop();
            //Log.Debug("checking for existence of any longRunningTasks");
            List<Task> longRunningTaskList = container.TryResolve(typeof(List<Task>)) as List<Task>;
            //Log.Debug($"There are {longRunningTaskList.Count} tasks in the longRunningTaskList");
            timers["longRunningTasksCheckTimer"].Start();
            //Log.Debug("Leaving the appHost.LongRunningTasksCheckTimer_Elapsed Method");
        }

        // Stop the entire program
        void menuItem1_Click(object Sender, EventArgs e) {
            Log.Debug("AceCommander NotifyIcon menuItem1_Click event handdler started");
            this.Stop();
            Log.Debug("AceCommander NotifyIcon menuItem1_Click event handdler started");
        }

        /// <summary>
    /// Application specific configuration
    /// This method should initialize any IoC resources utilized by your web service classes.
    /// </summary>
    public override void Configure(Container container) {
        // inject the concrete logging implementation
        Log.Debug($"Entering AppHost.Configure method, container is {container.ToString()}");
      // Read in the Application's base configuration settings. AppSettings is a first class object on the Container, so it will be auto-wired
      // Location of the files will depend on running as LifeCycle Production/QA/Dev as well as Debug and Release settings
      // Start with the AceService.BaseService builtin (compile-time) configuration settings
      // Add (Superseding any previous values) the App.config file (AKA AceAgent.exe.config at runtime)
      // Add (Superseding any previous values) the optional configuration file for BaseService configuration settings from a text file in the program directory (AKA AceService.settings.txt )
      // ToDo: Add (Superseding any previous values) environment values that match any keys
      // ToDo: Add (Superseding any previous values) command line variable values that match any keys

      AppSettings = new MultiAppSettingsBuilder()
// Start with the AceService.BaseService builtin (compile-time) configuration settings
.AddDictionarySettings(DefaultConfiguration.Configuration())
          // Add (Superseding any previous values) the App.config file (AKA AceAgent.exe.config at runtime)
          .AddAppSettings()
          // Add (Superseding any previous values) the optional configuration file for BaseService configuration settings from a text file in the program directory (AKA AceService.settings.txt )
          .AddTextFile(agentSettingsTextFileNameString)
          // ToDo: Add (Superseding any previous values) environment values that match any keys
          //.AddEnvironmentalVariables()
          // ToDo: Add (Superseding any previous values) command line variable values that match any keys
          //.Add??
          .Build();

      // Create the BaseServices data structure and register it in the container
      var baseServicesData = new BaseServicesData(this);
      container.Register<BaseServicesData>(c => baseServicesData);


        // ToDo: support AppSettings to control the enable/disable of Postman
      // Enable Postman integration
      Plugins.Add(new PostmanFeature());

        // ToDo: support AppSettings to control the enable/disable of CorsFeature
      // Enable CORS support
      Plugins.Add(new CorsFeature(allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
                                  allowedOrigins: "*",
                                  allowCredentials: true,
                                  allowedHeaders: "content-type, Authorization, Accept"));

        // ToDo: support AppSettings to control the enable/disable of Metadata Feature
        this.Config
            .EnableFeatures = Feature.All
            .Remove(Feature.Metadata);

        // Turn debug mode for the ACEAgent depending if running in debug mode or release mode
#if Debug
      this.Config.DebugMode = true;
#else
        this.Config
            .DebugMode = false;
#endif

        // Create the basic services observable data structures based on the configuration settings

        // Add a dictionary of timers and a list to hold "long-running tasks" to the IoC container
      #region create a dictionary of timers and register it in the IoC container
        timers = new Dictionary<string, System.Timers.Timer>();
        container.Register<Dictionary<string, System.Timers.Timer>>(c => timers);
      #endregion create a dictionary of timers and register it in the IoC container
      #region create a list of tasks that is intended to hold "long running" tasks and register the list in the IoC container
      longRunningTaskList = new List<Task>();
        container.Register<List<Task>>(c => longRunningTaskList);
      #endregion create a list of tasks that is intended to hold "long running" tasks and register the list in the IoC container

      // Add a timer to check the status of long running tasks, and attach a callback to the timer
      #region create longRunningTasksCheckTimer, connect callback, and store in container's timers
      Log.Debug("In AppHost.Configure method, creating longRunningTasksCheckTimer");
        var longRunningTasksCheckTimer = new System.Timers.Timer(1000);
        longRunningTasksCheckTimer.AutoReset = true;
        longRunningTasksCheckTimer.Elapsed += new ElapsedEventHandler(LongRunningTasksCheckTimer_Elapsed);
        timers.Add("longRunningTasksCheckTimer", longRunningTasksCheckTimer);
      #endregion create longRunningTasksCheckTimer, connect callback, and store in container's timers

        // ToDo: Get the list of plugins to install from the configuration settings, currently hardcoded to load just the GUIServices
      // Create the list of PlugIns to load
      var plugInList = new List<IPlugin>() {
          new RealEstateSearchServicesPlugin(),
          new MinerServicesPlugin(),
          new GUIServicesPlugin()
      };

        // Load each plugin here. Note that plugins may add AppSettings specific to its needs
        foreach(var pl in plugInList) {
            Plugins.Add(pl);
        }

        // ToDo: See Issue #8
      // ToDo place a static, deep-copy of the current application'instance of the configuration settings as the first object in the application's configuration settings history list.

        // start all the timers
        Log.Debug("In AppHost.Configure method, starting all timers");
        longRunningTasksCheckTimer.Start();

        // create a NotifyIcon
        Log.Debug("Create a NotifyIcon for AceCommander");
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        NotifyIcon notifyIcon1 = new NotifyIcon();
        ContextMenu contextMenu1 = new ContextMenu();
        MenuItem menuItem1 = new MenuItem();
        contextMenu1.MenuItems
            .AddRange(new MenuItem[] { menuItem1 });
        menuItem1.Index = 0;
        menuItem1.Text = "E&xit";
        menuItem1.Click += new EventHandler(menuItem1_Click);
        notifyIcon1.Icon = new Icon("atap.ico");
        notifyIcon1.Text = "AceCommander";
        notifyIcon1.ContextMenu = contextMenu1;
        notifyIcon1.Visible = true;

        //Log.Debug("Calling a Web Forms Application instance's static Run method");
      //Application.Run();
      //notifyIcon1.Visible = false;
      Log.Debug("NotifyIcon for AceCommander created");
        Log.Debug("Leaving AppHost.Configure");
    }
        /// <summary>
    /// Shut down the Web Service
    /// </summary>
    public override void Stop() {
        Log.Debug("Entering AppHost Stop Method");
        var container = base.Container;

        // It is possible that the Stop method is called during service startup, because the service could be failing because of a problem during startup,
      //  so need to check that each of container's disposable items actually exist before disposing of them

        // Stop and dispose of all timers
        Dictionary<string, System.Timers.Timer> timers;
        try {
            Log.Debug("In AppHost.Stop method, trying to resolve the timers dictionary");
            timers = container.TryResolve(typeof(Dictionary<string, System.Timers.Timer>)) as Dictionary<string, System.Timers.Timer>;
        } catch(Exception ex) {
            Log.Debug($"In AppHost.Stop method, resolving the timers dictionary threw exception message: {ex.Message}");
            throw;
        }

        foreach(var t in timers.Values) {
            t.Stop();
            t.Dispose();
        }
        // call the ServiceStack AppSelfHostBase Stop method
        Log.Debug("Entering the ServiceStack AppSelfHostBase Stop Method");
        base.Stop();
        Log.Debug("Leaving AppHost Stop Method");
    }
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
