
using System;
using System.Reflection;
using ServiceStack;
using ServiceStack.Configuration;
using ATAP.Utilities.ETW;
namespace Ace.PlugIn.AMQPServices {
#if TRACE
    [ETWLogAttribute]
#endif
    public class AMQPServicesData {

    public AMQPServicesData() : this(new MultiAppSettingsBuilder().Build()) {
        }

        public AMQPServicesData(IAppSettings pluginAppSettings) {
      PluginAppSettings = pluginAppSettings;
        }

        //ToDo: constructors with event handlers


    public IAppSettings PluginAppSettings { get; }
    }
}
