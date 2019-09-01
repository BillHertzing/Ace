using Agent.GUIServices.Shared;
using ServiceStack;


namespace Ace.Agent.GUIServices
{

    // This route will ensure that the GUI PlugIn is loaded
    // This route will return this PlugIn's ConfigurationData
    [Route("/GetGUIServicesConfigurationData")]
    public class GetGUIServicesConfigurationDataReqDTO : IReturn<GetGUIServicesConfigurationDataRspDTO> { }
    public class GetGUIServicesConfigurationDataRspDTO {
        public ConfigurationData ConfigurationData { get; set; }
    }
}
