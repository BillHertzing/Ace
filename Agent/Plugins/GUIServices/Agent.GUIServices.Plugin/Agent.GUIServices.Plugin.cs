
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Funq;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.VirtualPath;

namespace Ace.Agent.GUIServices {
    public class GUIServicesPlugin : IPlugin, IPreInitPlugin {
        // Create a logger for this class
        public static ILog Log = LogManager.GetLogger(typeof(GUIServicesPlugin));

        // Surface the configKeyPrefix for this namespace
        public static string myNamespace =
            MethodBase
            .GetCurrentMethod()
            .DeclaringType
            .Namespace;

        public void Configure(IAppHost appHost) {
            Log.Debug("starting GUIServicesPlugin.Configure");

            // Populate this Plugin's Application Configuration Settings
      // Location of the files will depend on running as LifeCycle Production/QA/Dev as well as Debug and Release settings
      var pluginAppSettings = new MultiAppSettingsBuilder()
    // Environment variables have higest priority
    //.AddEnvironmentalVariables()
    // Configuration settings in a text file relative to the current working directory at the point in time when this method executes.
    .AddTextFile(pluginSettingsTextFileNameString)
          // Builtin (compiled in) have the lowest priority
          .AddDictionarySettings(DefaultConfiguration.Configuration())
          .Build();

            // tell ServiceStack where to find the physical base of the GUI static files
      // In RELEASE mode, the physical path of the root of the GUI should be located in a subdirectory relative to the location of the assembly that holds Program
      // In DEBUG mode, the physical path of the root of the GUI will be located somewhere "above and to the side" of the ServiceStack Plugin project
      string physicalRootPath =
          Path
          .GetDirectoryName(
          Assembly
          .GetExecutingAssembly()
          .Location);
            Log.Debug($"in GUIServicesPlugin.Configure, initial physicalRootPath (GetExecutingAssembly().Location) = {physicalRootPath}");
            string relativeRootPathValue;
#if DEBUG
      // set the physicalRootPath of the GUI code to the appSetting for the development RelativeRootPathKey relative to the location of this plugin's assembly
      // verify key exists, else throw exception
      if(!pluginAppSettings.Exists(debugRelativeRootPathKey)
          || (pluginAppSettings.GetString(debugRelativeRootPathKey) == string.Empty)) {
          throw new Exception(debugRelativeRootPathKeyOrValueNotFoundExceptionMessage);
      }
            relativeRootPathValue = pluginAppSettings.GetString(debugRelativeRootPathKey);
#else
      // set the physicalRootPath of the GUI code to the appSetting for the production RelativeRootPathKey relative to the location of this plugin's assembly
      // verify key exists, else throw exception
      if (!pluginAppSettings.Exists(productionRelativeRootPathKey) || (pluginAppSettings.GetString(productionRelativeRootPathKey) == ""))
      {
        throw new Exception(productionRelativeRootPathKeyOrValueNotFoundExceptionMessage);
      }
       relativeRootPathValue = pluginAppSettings.GetString(productionRelativeRootPathKey);
#endif
            Log.Debug($"in GUIServicesPlugin.Configure, relativeRootPathValue = {relativeRootPathValue}");
            // Use these for checking for invalid characters in the appSettings RelativeRootPath values
            char[] invalidChars = Path.GetInvalidPathChars();
            bool valid = !relativeRootPathValue.Any(x => invalidChars.Contains(x));

            if(!valid) {
                throw new Exception(relativeRootPathValueContainsIlegalCharacterExceptionMessage);
            }
            Log.Debug($"in GUIServicesPlugin.Configure, physicalRootPath = {physicalRootPath}, relativeRootPathValue = {relativeRootPathValue}");
            physicalRootPath = PathUtils.CombinePaths(physicalRootPath, relativeRootPathValue);
            Log.Debug($"in GUIServicesPlugin.Configure, ");

            // use an appSetting for the VirtualRootPath 
            string virtualRootPath;
            // verify key exists, else throw exception. String.Empty is allowed
            if(!pluginAppSettings.Exists(virtualRootPathKey)) {
                throw new Exception(virtualRootPathKeyOrValueNotFoundExceptionMessage);
            }
            virtualRootPath = pluginAppSettings.GetString(virtualRootPathKey);
            // Map the Virtual root Dir to the physical path of the root of the GUI
            // Wrap in a try catch block in case the root Path does not exist
            try {
                appHost.AddVirtualFileSources
                    .Add(new FileSystemMapping(virtualRootPath, physicalRootPath));
            } catch {
                throw;
                // ToDo: figure out how to log this and fallback to something useful
            }

            Log.Debug($"in GUIServicesPlugin.Configure, virtualRootPath = {virtualRootPath}");

            // per conversation with Myth at SS, the default behaviour, for URi "/" is to return the content of wwwroot/index.html

            // ToDo: if the GUI configuration specifies that the GUI has GUI-specific data sensor that can and should be monitored, attach the event handlers that will respond to changes in the monitored data structures
      // ToDo: setup the mechanisms that monitors the GUI 

            // create the plugIn's data object and pass it to the container so it will be available to every other module and services
            var gspd = new GUIServicesData(pluginAppSettings);
            appHost.GetContainer()
                .Register<GUIServicesData>(d => gspd);

            // ToDo: enable the mechanisms that monitors each GUI-specific data sensor, and start them running
        }

        /// <summary>
    /// Register this plugin with the appHost
    /// setup any necessary objects and intercepts
    /// </summary>
    /// <param name="appHost">The hosting provider</param>
    public void Register(IAppHost appHost) {
        Log.Debug("starting GUIServicesPlugin.Register");
        if(null == appHost) {
            throw new ArgumentNullException("appHost");
        }

        appHost.RegisterService<GUIServices>();
    }

        public IAppHost appHost { get; set; }

        // AppSettings will be auto-wired by ServiceStack to the IAppSettings in the IOC
        public IAppSettings AppSettings { get; set; }

        public Container Container { get; set; }

    #region string constants
    #region Configuration Key strings
    const string debugRelativeRootPathKey = "DebugRelativeRootPath";
        const string releaseRelativeRootPathKey = "ReleaseRelativeRootPath";
        const string virtualRootPathKey = "VirtualRootPath";
    #endregion Configuration Key strings

    #region Exception Messages (string constants)
    const string debugRelativeRootPathKeyOrValueNotFoundExceptionMessage = "DebugRelativeRootPath Key not found in Plugin's Configuration setting, or the key is present but set to String.Empty. Add the DebugRelativeRootPath Key and Value to the Application Configuration, and retry.";
        const string releaseRelativeRootPathKeyOrValueNotFoundExceptionMessage = "ReleaseRelativeRootPath Key not found in Plugin's Configuration setting, or the key is present but set to String.Empty. Add the ReleaseRelativeRootPath Key and Value to the Application Configuration, and retry.";
        const string virtualRootPathKeyOrValueNotFoundExceptionMessage = "VirtualRootPath Key not found in Plugin's Configuration setting. Add the VirtualRootPath Key and Value (null value is OK) to the Application Configuration, and retry.";
        const string relativeRootPathValueContainsIlegalCharacterExceptionMessage = "relativeRootPathValue contains one or more characters that are illegal in a path. Ensure that the DebugRelativeRootPathKey's value and the ReleaseRelativeRootPathKey's value does not contain any characters that are illegal in a path, and retry.";
    #endregion Exception Messages (string constants)


    #region File Name string constants
        const string pluginSettingsTextFileNameString = "Agent.GUIServices.settings.txt";
    #endregion File Name string constants
    #endregion string constants
  }
}
