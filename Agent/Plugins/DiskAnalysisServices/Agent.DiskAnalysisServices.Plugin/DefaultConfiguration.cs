using System;
using System.Collections.Generic;

namespace Ace.Plugin.DiskAnalysisServices {
    static class DefaultConfiguration {
        public static Dictionary<string, string> Production => new Dictionary<string, string>() {
            { "Ace.Plugin.DiskAnalysisServices.Config.Placeholder", "placeholder" }
        };
    }
}
