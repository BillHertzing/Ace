using ATAP.Utilities.WebHostBuilders.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ace.Agent.Host {
    static public class WebHostDefaultConfiguration {
        // Create the minimal set of Configuration settings that the WebHost hosted by the GenericHost needs to startup and run in production
        public static Dictionary<string, string> Production =
            new Dictionary<string, string> { 
                {"urls", "http://localhost:22000/"}
            };
    }
}
