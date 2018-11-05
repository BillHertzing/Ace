
using System;
using System.Reflection;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.VirtualPath;

namespace Ace.Agent.GUIServices {
    public class GUIServicesPluginData {

        public GUIServicesPluginData() : this(string.Empty) {
        }

        public GUIServicesPluginData(string rootPath) {
      RootPath = rootPath;
        }

        //ToDo: constructors with event handlers

        public string RootPath {get;set;}
    }
}
