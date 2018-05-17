
using ServiceStack;
using System;
using Ace.AceService.GUIServices.Interfaces;
using Ace.AceService.GUIServices.Models;
using ServiceStack.Configuration;


namespace Ace.AceService.GUIServices.Plugin
{


    public class GUIServicesPluginData
    {
    string webHostPhysicalPath;
        public GUIServicesPluginData() : this("") { }

        public GUIServicesPluginData(string webHostPhysicalPath)
        {
            this.webHostPhysicalPath = webHostPhysicalPath;
        }

    //ToDo: constructors with event handlers

    public string WebHostPhysicalPath => webHostPhysicalPath;
    }

    public class GUIServicesPlugin : IPlugin
    {
    /// <summary>
    /// Register this plugin with the appHost
    /// setup any necessary objects and intercepts
    /// </summary>
    /// <param name="appHost">The hosting provider</param>
    public void Register(IAppHost appHost)
    {
      if (null == appHost) {  throw new ArgumentNullException("appHost"); }

            appHost.RegisterService<Ace.AceService.GUIServices.Interfaces.GUIServices>();

            var container = appHost.GetContainer();

      // writeable access to the Parent appHost's Config
      var config = HostContext.AppHost.Config;

      // tell ServiceStack where to find the base of the GUI application, relative to the location from which ServiceStack started executing
      // In RELEASE mode, the GUI should be located in a subdirectory relative to the location of the assembly that holds Program
      // In DEBUG mode, the GUI will be located somewhere above and to the side of the location of the assembly that holds Program
      // ToDo: remove the HardCoded DEBUG path and use VS Build Tools to discover the location of the latest GUI files built in DEBUG mode
      string webHostPhysicalPath;
#if DEBUG
      webHostPhysicalPath = @"C:\Dropbox\whertzing\GitHub\Ace\AceGUI\obj\Debug\netstandard2.0\blazor";
      // ToDo: overwrite the default with a Debug appSetting if one is present
      // webHostPhysicalPath = appHost.AppSettings.Get<string>("Ace.AceService.GUIServices.Plugin.Debug.WebHostPhysicalPath")? ;
#else
      // Get the location of the Program assembly
      string startPath = "ToDo";
      appHost.Config.WebHostPhysicalPath = strartpath + "AceGUI/"
      //VirtualPathProvider = new FileSystemVirtualPathProvider(this, "../../../Project2")
      // ToDo: overwrite the default with a release appSetting if one is present
      // webHostPhysicalPath = appHost.AppSettings.Get<string>("Ace.AceService.GUIServices.Plugin.Release.WebHostPhysicalPath")? ;
#endif

      config.WebHostPhysicalPath = webHostPhysicalPath;


      // if the GUI configuration specifies that the GUI has GUI-specific data sensor that
      //  can and should be monitored, attach the event handlers that will respond to changes in the monitored data structures

      // setup the mechanisms that monitors the GUI 

      var gspd = new GUIServicesPluginData(webHostPhysicalPath);
      // pass the plugIn's observable data structures and event handlers to the container so they will be available to every other module and services
      container.Register<GUIServicesPluginData>(d => gspd);

      // enable the mechanisms that monitors each GUI-specific data sensor, and start them running
    }
  }
}
