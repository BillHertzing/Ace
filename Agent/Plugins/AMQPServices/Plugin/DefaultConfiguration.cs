using System;
using System.Collections.Generic;

namespace Ace.PlugIn.AMQPServices {

    static class DefaultConfiguration {
        public static Dictionary<string, string> Production = new Dictionary<string, string>() {
            { "AMQPConnectionString", "amqp://guest:guest@localhost:5672/" },
        };
    }
}

