using Serilog;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.VirtualPath;

using System;
using System.Linq;
using ATAP.Utilities.ServiceStack;
using System.IO;

namespace Ace.Agent.GUIServices {
    public class GUIServicesPlugin : IPlugin, IPreInitPlugin {

        // Surface the configKeyPrefix for this namespace
        public static string myNamespace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;

        public void Configure(IAppHost appHost) {
            Log.Debug("<GUIServicesPlugin.Configure");
            Log.Debug("in GUIServicesPlugin.Configure, appHost = {appHost}", appHost);

            // Populate this PlugIn's AppSettings Configuration Settings and place it in the appSettingsDictionary
            // Location of the files are at the ContentRoot. ToDo: figure out how to place them / resolve them relative to the location of the PlugIn assembly
            // 
            // HAck!
            string envName = "Development";

            var pluginAppSettingsBuilder = new MultiAppSettingsBuilder();
            // ToDo: command line settings
            // Environment variables have 2nd highest priority
            // ToDo: specific prefix
            pluginAppSettingsBuilder.AddEnvironmentalVariables();
            // Location of the files are at the ContentRoot. ToDo: figure out how to place them / resolve them relative to the location of the PlugIn assembly
            // Non-Production Configuration settings in a text file
            if (true) { // Hack!
                //if (!appHost.IsProduction()) {
                    var settingsTextFileName = Ace.Agent.GUIServices.StringConstants.PluginSettingsTextFileNameString+'.'+ envName + StringConstants.PluginSettingsTextFileNameSuffix;
                // ToDo: ensure it exists and the ensure we have permission to read it
                // ToDo: Security: There is something called a Time-To-Check / Time-To-Use vulnerability, ensure the way we check then access the text file does not open the program to this vulnerability
                pluginAppSettingsBuilder.AddTextFile(settingsTextFileName);
            }
            // Production Configuration settings in a text file
            pluginAppSettingsBuilder.AddTextFile(StringConstants.PluginSettingsTextFileNameString + StringConstants.PluginSettingsTextFileNameSuffix);
            // BuiltIn (compiled in) have the lowest priority
            pluginAppSettingsBuilder.AddDictionarySettings(DefaultConfiguration.Production);

            // Create the appSettings from the builder
            var pluginAppSettings = pluginAppSettingsBuilder.Build();
            // Register them so other PlugIns can see them
            AppSettingsDictionary appSettingsDictionary = appHost.TryResolve<AppSettingsDictionary>();
            if (appSettingsDictionary == null) {
                // ToDo: Log:Error
                throw new InvalidOperationException("ToDo: exception message");
            }
            // ToDo: extend appSettingsDictionary to allow for adding a plugIn's AppSettings instance; needs to account for both readonly and changenotify
            //appSettingsDictionary.

            // ContentRoot from SS, and WebHostRoot (null?)
			var contentRoot = Directory.GetCurrentDirectory();  // Hack!
			//ToDo: For each GUI in GUIs. Create a linq query, define a static action, loop and map inputs to outputs
            // GUI(s) relative paths from Configuration; Production defaults for at least the first are in the DefaultConfiguration.Production dictionary
            if (!pluginAppSettings.Exists(StringConstants.RelativeToContentRootPathConfigKey)
                ||(pluginAppSettings.GetString(StringConstants.RelativeToContentRootPathConfigKey)==string.Empty)) {
                throw new Exception(StringConstants.RelativeToContentRootPathKeyOrValueNotFoundExceptionMessage);
            }
            var relativeRootPathValue=pluginAppSettings.GetString(StringConstants.RelativeToContentRootPathConfigKey);
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

            // Blazor requires CORS support, enable the ServiceStack CORS feature
            appHost.Plugins.Add(new CorsFeature(
               allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
               allowedOrigins: "*",
               allowCredentials: true,
               allowedHeaders: "content-type, Authorization, Accept"));

            // ToDo: if the GUI configuration specifies that the GUI has GUI-specific data sensor that can and should be monitored, attach the event handlers that will respond to changes in the monitored data structures
            // ToDo: setup the mechanisms that monitors the GUI 

            // create the plugIn's data object and pass it to the container so it will be available to every other module and services
            appHost.GetContainer()
                .Register<GUIServicesData>(d => new GUIServicesData(pluginAppSettings));

            // ToDo: enable the mechanisms that monitors each GUI-specific data sensor, and start them running
        }

        /// <summary>
        /// Register this plugin with the appHost
        /// setup any necessary objects and intercepts
        /// </summary>
        /// <param name="appHost">The hosting provider</param>
        public void Register(IAppHost appHost) {
            Log.Debug("starting GUIServicesPlugin.Register");
            if (null==appHost) {
                throw new ArgumentNullException("appHost");
            }
            appHost.RegisterService<GUIServices>();
        }

        /*
        #region string constants
        #region Configuration Key strings
        const string debugRelativeRootPathKey = "DebugRelativeRootPath";
        const string releaseRelativeRootPathKey = "ReleaseRelativeRootPath";
        const string virtualRootPathKey = "VirtualRootPath";
        #endregion 

        #region Exception Messages (string constants)
        const string debugRelativeRootPathKeyOrValueNotFoundExceptionMessage = "DebugRelativeRootPath Key not found in Plugin's Production setting, or the key is present but set to String.Empty. Add the DebugRelativeRootPath Key and Value to the Application Production, and retry.";
        const string releaseRelativeRootPathKeyOrValueNotFoundExceptionMessage = "ReleaseRelativeRootPath Key not found in Plugin's Production setting, or the key is present but set to String.Empty. Add the ReleaseRelativeRootPath Key and Value to the Application Production, and retry.";
        const string virtualRootPathKeyOrValueNotFoundExceptionMessage = "VirtualRootPath Key not found in Plugin's Production setting. Add the VirtualRootPath Key and Value (null value is OK) to the Application Production, and retry.";
        const string relativeRootPathValueContainsIlegalCharacterExceptionMessage = "relativeRootPathValue contains one or more characters that are illegal in a path. Ensure that the DebugRelativeRootPathKey's value and the ReleaseRelativeRootPathKey's value does not contain any characters that are illegal in a path, and retry.";
        #endregion Exception Messages (string constants)


        #region File Name string constants
        const string pluginSettingsTextFileNameString = "Agent.GUIServices.settings.txt";
        #endregion File Name string constants
        #endregion string constants
        */
    }

	/*public static class StringConstants {
        // ToDo: Localize the string constants

        #region Configuration Key strings
        public const string RelativeToContentRootPathConfigKey = "RelativeToContentRootPath";
        public const string VirtualRootPathConfigKey = "VirtualRootPath";
        #endregion 

        #region Exception Messages (string constants)
        public const string RelativeToContentRootPathKeyOrValueNotFoundExceptionMessage = "RelativeToContentRootPath Key not found in Plugin's Configuration setting, or the key is present but set to String.Empty. Add the ReleaseRelativeRootPath Key and Value to the Application Configuration, and retry.";
        public const string VirtualRootPathKeyOrValueNotFoundExceptionMessage = "VirtualRootPath Key not found in Plugin's Configuration setting. Add the VirtualRootPath Key and Value (null value is OK) to the Application Configuration, and retry.";
        public const string RelativeRootPathValueContainsIlegalCharacterExceptionMessage = "relativeRootPathValue contains one or more characters that are illegal in a path. Ensure that the DebugRelativeRootPathKey's value and the ReleaseRelativeRootPathKey's value does not contain any characters that are illegal in a path, and retry.";
        #endregion


        #region File Name string constants
        public const string PluginSettingsTextFileNameString = "Agent.GUIServicesSettings";
        public const string PluginSettingsTextFileNameSuffix = ".txt";
        #endregion
    } */
}
