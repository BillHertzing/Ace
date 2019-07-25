
using System;
using System.Reflection;
using ServiceStack;
using ServiceStack.Configuration;

using ServiceStack.VirtualPath;

namespace Ace.Agent.GUIServices {
    public class GUIServicesData {

    public GUIServicesData() : this(new MultiAppSettingsBuilder().Build()) {
        }

        public GUIServicesData(IAppSettings pluginAppSettings) {
      PluginAppSettings = pluginAppSettings;
        }

        //ToDo: constructors with event handlers


    public IAppSettings PluginAppSettings { get; }
    }
}
