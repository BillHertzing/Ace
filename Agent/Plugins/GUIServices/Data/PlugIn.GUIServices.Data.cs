
using System;
using System.Reflection;
using Agent.GUIServices.Shared;
using ServiceStack;
using ServiceStack.Configuration;

using ServiceStack.VirtualPath;

namespace Ace.Agent.GUIServices {
    public class GUIServicesData {
        public GUIServicesData(IAppSettings pluginAppSettings, ConfigurationData configurationData) {
            PlugInAppSettings=pluginAppSettings;
            ConfigurationData=configurationData;
        }

        public IAppSettings PlugInAppSettings { get; set; }
        public ConfigurationData ConfigurationData { get; set; }

    }
}
