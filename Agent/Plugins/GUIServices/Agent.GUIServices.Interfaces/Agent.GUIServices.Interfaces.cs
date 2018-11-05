

using System;
using ServiceStack;
using ServiceStack.Logging;

namespace Ace.Agent.GUIServices {
    public class GUIServices : Service {
        public static ILog Log = LogManager.GetLogger(typeof(GUIServices));

		public object Any(VerifyGUIRequest request) {
			Log.Debug("starting Any VerifyGUI request");
			var kind = request.Kind;
			var version = request.Version;
			// ToDo: add the code that returns True/False for the route that includes the kind/version
			return new VerifyGUIResponse { Result = "Blazor" };
		}
    }
}
