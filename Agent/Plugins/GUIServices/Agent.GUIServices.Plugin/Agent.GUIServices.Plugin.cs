
using System;
using System.Reflection;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.VirtualPath;

namespace Ace.Agent.GUIServices {


    public class GUIServicesPlugin : IPlugin, IPreInitPlugin {
    #region string constants
    #region Configuration Key strings
    const string debugRelativeRootPathKey = "DebugRelativeRootPath";
    const string releaseRelativeRootPathKey = "ReleaseRelativeRootPath";
    const string virtualRootPathKey = "VirtualRootPath";
    #endregion Configuration Key strings

    #region Exception Messages (string constants)
    const string debugRelativeRootPathKeyOrValueNotFoundExceptionMessage = "DebugRelativeRootPath Key not found in Plugin's Configuration setting, or the key is present but set to String.Empty. Add the DebugRelativeRootPath Key and Value to the Application Configuration, and retry.";
    const string releaseRelativeRootPathKeyOrValueNotFoundExceptionMessage = "ReleaseRelativeRootPath Key not found in Plugin's Configuration setting, or the key is present but set to String.Empty. Add the ReleaseRelativeRootPath Key and Value to the Application Configuration, and retry.";
    const string virtualRootPathKeyOrValueNotFoundExceptionMessage  = "VirtualRootPath Key not found in Plugin's Configuration setting. Add the VirtualRootPath Key and Value (null value is OK) to the Application Configuration, and retry.";
    #endregion Exception Messages (string constants)

    const string defaultRedirectPath = "/index.html";

    #region File Name string constants
    const string pluginSettingsTextFileNameString = "Agent.GUIServices.settings.txt";
    #endregion File Name string constants
    #endregion string constants

    // Create a logger for this class
    public static ILog Log = LogManager.GetLogger(typeof(GUIServicesPlugin));

    // Surface the configKeyPrefix for this namespace
    public static string myNamespace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;

    // AppSettings will be auto-wired by ServiceStack to the IAppSettings in the IOC
    public IAppSettings AppSettings { get; set; }

    public Funq.Container Container { get; set; }
    public IAppHost appHost { get; set; }

    public void Configure(IAppHost appHost) {
      Log.Debug("starting GUIServicesPlugin.Configure");
      // ToDo: Load a plugin-specific app settings file, if one is present
	    // Populate this Plugin's Application Configuration Settings
      // Location of the files will depend on running as LifeCycle Production/QA/Dev as well as Debug and Release settings
      var pluginAppSettings =new MultiAppSettingsBuilder()
    // Environment varialbes have higest priority
    //.AddEnvironmentalVariables()
    // Configuration settings in a text file relativve to the current working directory at the point in time when this method executes.
    .AddTextFile(pluginSettingsTextFileNameString)
    // Builtin (compiled in) have the lowest priority
    .AddDictionarySettings(DefaultConfiguration.Configuration())
    .Build();

      // tell ServiceStack where to find the physical base of the GUI static files
      // In RELEASE mode, the physical path of the root of the GUI should be located in a subdirectory relative to the location of the assembly that holds Program
      // In DEBUG mode, the physical path of the root of the GUI will be located somewhere "above and to the side" of the ServiceStack Plugin project
      string physicalRootPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
      Log.Debug($"in GUIServicesPlugin.Configure, initial physicalRootPath (GetExecutingAssembly().Location) = {physicalRootPath}");
#if DEBUG
      // set the physicalRootPath of the GUI code to the appSetting for the development RelativeRootPathKey relative to the location of this plugin's assembly
      // verify key exists, else throw exception
      if (!pluginAppSettings.Exists(debugRelativeRootPathKey) || (pluginAppSettings.GetString(debugRelativeRootPathKey) == ""))
      {
        throw new Exception(debugRelativeRootPathKeyOrValueNotFoundExceptionMessage);
      }
      // ToDo: Add check for invalid characters in the relative location
      Log.Debug($"in GUIServicesPlugin.Configure, debugRelativeRootPath = {pluginAppSettings.GetString(debugRelativeRootPathKey)}");
      physicalRootPath = PathUtils.CombinePaths(physicalRootPath, pluginAppSettings.GetString(debugRelativeRootPathKey));
#else
      // set the physicalRootPath of the GUI code to the appSetting for the production RelativeRootPathKey relative to the location of this plugin's assembly
      // verify key exists, else throw exception
      if (!pluginAppSettings.Exists(productionRelativeRootPathKey) || (pluginAppSettings.GetString(productionRelativeRootPathKey) == ""))
      {
        throw new Exception(productionRelativeRootPathKeyOrValueNotFoundExceptionMessage);
      }
      Log.Debug($"in GUIServicesPlugin.Configure, productionRelativeRootPathKey = {pluginAppSettings.GetString(productionRelativeRootPathKey)}");
       physicalRootPath =PathUtils.CombinePaths(physicalRootPath, pluginAppSettings.GetString(productionRelativeRootPathKey));
#endif
      Log.Debug($"in GUIServicesPlugin.Configure, physicalRootPath = {physicalRootPath}");

      // use an appSetting for the VirtualRootPath 
      string virtualRootPath;
      // verify key exists, else throw exception. String.Empty is allowed
      if (!pluginAppSettings.Exists(virtualRootPathKey))
      {
        throw new Exception(virtualRootPathKeyOrValueNotFoundExceptionMessage);
      }
      virtualRootPath = pluginAppSettings.GetString(virtualRootPathKey);
            // Map the Virtual root Dir to the physical path of the root of the GUI
            // Wrap in a try catch block in case the root Path does not exist
            try {
                appHost.AddVirtualFileSources.Add(new FileSystemMapping(virtualRootPath, physicalRootPath));
            } catch {
                throw;
                // ToDo: figure out how to log this and fallback to something useful
            }

      Log.Debug($"in GUIServicesPlugin.Configure, virtualRootPath = {virtualRootPath}");

      // change the default redirect path so that a request to an unexpected path will redirect to index.html
      appHost.Config.DefaultRedirectPath = defaultRedirectPath;

            // ToDo: if the GUI configuration specifies that the GUI has GUI-specific data sensor that can and should be monitored, attach the event handlers that will respond to changes in the monitored data structures
      // ToDo: setup the mechanisms that monitors the GUI 

            // create the plugIn's data object and pass it to the container so it will be available to every other module and services
            var gspd = new GUIServicesPluginData(physicalRootPath);
            appHost.GetContainer()
            .Register<GUIServicesPluginData>(d => gspd);

            // ToDo: enable the mechanisms that monitors each GUI-specific data sensor, and start them running
        }

        /// <summary>
    /// Register this plugin with the appHost
    /// setup any necessary objects and intercepts
    /// </summary>
    /// <param name="appHost">The hosting provider</param>
    public void Register(IAppHost appHost) {
      Log.Debug("starting GUIServicesPlugin.Register");
      if (null == appHost) {
            throw new ArgumentNullException("appHost");
        }

        appHost.RegisterService<GUIServices>();
    }


    }
}
