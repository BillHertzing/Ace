
using System;
using System.Reflection;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.VirtualPath;

namespace Ace.AceService.GUIServices.Plugin {
    public class GUIServicesPluginData {
        string rootPath;

        public GUIServicesPluginData() : this(string.Empty) {
        }

        public GUIServicesPluginData(string rootPath) {
      this.rootPath = rootPath;
        }

        //ToDo: constructors with event handlers

        public string RootPath => rootPath;
    }

    public class GUIServicesPlugin : IPlugin, IPreInitPlugin {
        const string builtinGUIVirtualRootPath = "";
        const string gUIServicesPlugInGUIVirtualRootPathKey = "Ace.GUIServices.Plugin.GUIVirtualRoot";

    public static ILog Log = LogManager.GetLogger(typeof(GUIServicesPlugin));

    public void Configure(IAppHost appHost) {
      Log.Debug("starting GUIServicesPlugin.Configure");
      // ToDo: Load a plugin-specific app settings file, if one is present

      // tell ServiceStack where to find the base of the GUI static files
      // In RELEASE mode, the physical path of the root of the GUI should be located in a subdirectory relative to the location of the assembly that holds Program
      // In DEBUG mode, the physical path of the root of the GUI will be located somewhere "above and to the side" of the ServiceStack Plugin project
      string physicalRootPath;
#if DEBUG
      // use a Debug appSetting for the physicalRootPath if one is present, otherwise use the builtin Debug value
            physicalRootPath = appHost.AppSettings.Get(gUIServicesPlugInDebugRootPathKey,
                                                       appHost.MapProjectPath(builtinDebugRootPath));
#else
      // use a Release appSetting for the physicalRootPath if one is present, otherwise use the builtin release value relative object the location where the program assembly is found
      physicalRootPath = appHost.AppSettings.Get(gUIServicesPlugInReleaseAbsoluteRootPathKey,Assembly.GetEntryAssembly().Location + appHost.AppSettings.Get(gUIServicesPlugInReleaseRelativeRootPathKey,builtinReleaseRelativeRootPath));
#endif
      // use an appSetting for the VirtualRootPath if one is present, otherwise use the builtin value
      string virtualRootPath;
            virtualRootPath = appHost.AppSettings.Get(gUIServicesPlugInGUIVirtualRootPathKey, builtinGUIVirtualRootPath);
            // Map the Virtual root Dir to the physical path of the root of the GUI
            // Wrap in a try catch block in case the root Path does not exist
            try {
                appHost.AddVirtualFileSources.Add(new FileSystemMapping(virtualRootPath, physicalRootPath));
            } catch {
                throw;
                // ToDo: figure out how to log this and fallback to something useful
            }

            // change the default redirect path so that a request to an unexpected path will redirect to index.html
            appHost.Config.DefaultRedirectPath = "/index.html";

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

        appHost.RegisterService<Interfaces.GUIServices>();
    }

#if DEBUG
    const string builtinDebugRootPath = @"~/../../../AceGUI/bin/Debug/netstandard2.0/Publish/AceGUI/dist";
        const string gUIServicesPlugInDebugRootPathKey = "Ace.GUIServices.Plugin.Debug.RootPath";
#else
    const string builtinReleaseRelativeRootPath = "/blazor";
    const string gUIServicesPlugInReleaseAbsoluteRootPathKey = "Ace.GUIServices.Plugin.Release.AbsoluteRootPath";
    const string gUIServicesPlugInReleaseRelativeRootPathKey = "Ace.GUIServices.Plugin.Release.RelativeRootPath";
#endif
    }
}
