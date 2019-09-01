

using System;
using Agent.GUIServices.Shared;
using ServiceStack;


namespace Ace.Agent.GUIServices {
    public class GUIServices : Service {

        public object Any(GetGUIServicesConfigurationDataReqDTO request) {
            // Get the Plugin's data structure from the SS container
            Ace.Agent.GUIServices.GUIServicesData gUIServicesData = HostContext.Resolve<GUIServicesData>();
            // Get this PlugIn's ConfigurationData
            ConfigurationData configurationData = gUIServicesData.ConfigurationData;
            return new GetGUIServicesConfigurationDataRspDTO { ConfigurationData=configurationData };
        }
    }
}
