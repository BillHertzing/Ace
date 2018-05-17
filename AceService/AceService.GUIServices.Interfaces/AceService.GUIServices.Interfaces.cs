

using System;
using System.IO;
using System.Runtime.Serialization;
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

    public HttpResult Get(FallbackForUnmatchedGUIRoutes request)
    {
      //Make sure we have the configuration for this station
      //SourceConfig sourceConf = LoadSourceConfig("gui");

      //Make sure we have the right path structure.
      if (request.PathInfo.StartsWith("/") == false)
      {
        request.PathInfo = $"/{request.PathInfo.Replace('/', Path.DirectorySeparatorChar)}";
      }

      FileInfo streamingFile = new FileInfo(request.PathInfo);
      HttpResult httpresult = new HttpResult(streamingFile);
      return httpresult;

      //FileInfo streamingFile = new FileInfo($"{sourceConf.contentPath}{request.PathInfo}");
      //String contentMimeType = StreamingContext.ResolveContentType(streamingFile.Extension);
      //return new HttpResult(streamingFile, contentMimeType);
    }
  }
}
