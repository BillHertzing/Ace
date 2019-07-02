

using System;
using ServiceStack;


namespace Ace.Agent.GUIServices {
    public class GUIServices : Service {

		public object Any(VerifyGUIRequest request) {
			var kind = request.Kind;
			var version = request.Version;
			// ToDo: add the code that returns True/False for the route that includes the kind/version
			return new VerifyGUIResponse { Result = "Blazor" };
		}
    }
}
