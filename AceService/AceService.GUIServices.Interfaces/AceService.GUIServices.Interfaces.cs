

using System;
using Ace.AceService.GUIServices.Models;
using ServiceStack;
using ServiceStack.Logging;

namespace Ace.AceService.GUIServices.Interfaces {
    public class GUIServices : Service {
        public static ILog Log = LogManager.GetLogger(typeof(GUIServices));

    public object Any(StartGUIRequest request) {
        Log.Debug("starting Any StartGUI request");
      var kind = request.Kind;
      var version = request.Version;
      return new StartGUIResponse { Result = "OK?" };
    }
        //public object Any(ListGUIRequest request) {
        //    Log.Debug("starting Any ListGUIs request");
        //  //ToDo
        //  return new ListGUIsResponse { Kind = request.Kind, Version = request.Version, App_Base = "A" };
        //}
    }
}
