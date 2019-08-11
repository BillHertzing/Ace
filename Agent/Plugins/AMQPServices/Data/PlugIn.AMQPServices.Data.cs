
using System;
using System.Reflection;
using ServiceStack;
using ServiceStack.Configuration;
using ATAP.Utilities.ETW;
using ATAP.Utilities.ConcurrentObservableCollections;
namespace Ace.PlugIn.AMQPServices {
#if TRACE
    [ETWLogAttribute]
#endif
    public class AMQPServicesData : IAMQPServicesData {

        public AMQPServicesData() : this(new MultiAppSettingsBuilder().Build()) {
        }

        public AMQPServicesData(IAppSettings pluginAppSettings) {
            PluginAppSettings = pluginAppSettings;
        }

        //ToDo: constructors with event handlers


        public IAppSettings PluginAppSettings { get; }

        public ConcurrentObservableDictionary<string, object> AMQPServicesDataCOD { get;set;}
    }
}
