using ATAP.Utilities.WebHostBuilders.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ace.Agent.Host {
    static public class GenericHostDefaultConfiguration {

        // Create the minimal set of Configuration settings that the Generic Host needs to startup and run in production
        public static Dictionary<string, string> Production =
            new Dictionary<string, string> {
                {"WebHostBuilderToBuild", SupportedWebHostBuilders.KestrelAloneWebHostBuilder.ToString()},
                {"Environment", StringConstants.EnvironmentProduction},
            };
    }
    static public class WebHostDefaultConfiguration {

        // Create the minimal set of Configuration settings that the webhost selected by the generichost needs to startup and run in production
        public static Dictionary<string, string> Production =
			new Dictionary<string, string> { 
				{"urls", "http://localhost:22000/"}
			};

    }
}
