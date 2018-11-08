
using System;
using System.Reflection;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.VirtualPath;

namespace Ace.Agent.GUIServices {
    public class GUIServicesData {
    #region string constants
    #region Configuration Key strings
    #endregion Configuration Key strings
    #region Exception Messages (string constants)
    #endregion Exception Messages (string constants)
    #endregion string constants

    public GUIServicesData() : this(new MultiAppSettingsBuilder().Build()) {
        }

        public GUIServicesData(IAppSettings pluginAppSettings) {
      PluginAppSettings = pluginAppSettings;
        }

        //ToDo: constructors with event handlers


    public IAppSettings PluginAppSettings { get; }
    }
}
