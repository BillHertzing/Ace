using ServiceStack;


namespace Ace.Agent.GUIServices
{

  // This route will ensure that serviceStack has the GUI PlugIn loaded
  [Route("/VerifyGUI")]
  [Route("/VerifyGUI/{Kind};{Version}")]
  public class VerifyGUIRequest : IReturn<VerifyGUIResponse>
    {
        public string Kind { get; set; }
        public string Version { get; set; }
    }
    public class VerifyGUIResponse
    {
        public string Result { get; set; }
    }
	
	// This route will list all of the GUIS and their versions that are loaded via a plugin
    //[Route("/gui/ListGUI")]
    //[Route("/gui/ListGUI/{Kind};{Version}")]
    //public class ListGUIRequest : IReturn<ListGUIsResponse>
    //{
    //    public string Kind { get; set; }
    //    public string Version { get; set; }
    //}
    //public class ListGUIsResponse
    //{
    //    public string Kind { get; set; }
    //    public string Version { get; set; }
    //    public string App_Base { get; set; }
    //}
}
