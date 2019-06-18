using System;
using System.Collections.Generic;

namespace Ace.Agent.GUIServices {
    static class DefaultConfiguration {
        public static Dictionary<string, string> Production = new Dictionary<string, string>() {
            { "GUIKind", "Blazor" },
            { "GUIVersion", "3.0P5" },
            { "RelativeToContentRootPath", "./GUI/GUI/dist" },
            { "VirtualRootPath", string.Empty },
        };
    }
}

