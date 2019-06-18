using ATAP.Utilities.WebHostBuilders.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ace.Agent.Host {
    static public class SSAppHostDefaultConfiguration {

        // Create the minimal set of Configuration settings that the program needs to startup and run in production
        public static Dictionary<string, string> Production =
        new Dictionary<string, string>
        {
            { "dummy", "dummy" }
        };

    }
}
