
using System;
using System.Reflection;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.VirtualPath;

namespace Ace.Agent.GUIServices {
    public class GUIServicesPluginData {
    #region string constants
    #region Configuration Key strings
    #endregion Configuration Key strings
    #region Exception Messages (string constants)
    #endregion Exception Messages (string constants)
    #endregion string constants

    public GUIServicesPluginData() : this(new MultiAppSettingsBuilder().Build()) {
        }

        public GUIServicesPluginData(IAppSettings pluginAppSettings) {
      PluginAppSettings = pluginAppSettings;
        }

        //ToDo: constructors with event handlers


    public IAppSettings PluginAppSettings { get; }
    }
}
