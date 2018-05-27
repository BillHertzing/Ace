using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Web;

namespace Ace.AceService.GUIServices.Models
{

   [Route("/StartGUI")]
  [Route("/StartGUI/{Kind};{Version}")]
  public class StartGUIRequest : IReturn<StartGUIResponse>
    {
        public string Kind { get; set; }
        public string Version { get; set; }
    }
    public class StartGUIResponse
    {
        public string Result { get; set; }
    }

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
