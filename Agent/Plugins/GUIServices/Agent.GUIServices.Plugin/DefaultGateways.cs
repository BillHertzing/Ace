using System;
using System.Collections.Generic;
using ATAP.Utilities.Http;

namespace Ace.Agent.GUIServices{
  static class DefaultGateways
  {
    public static Dictionary<string, IGateway> Configuration()
    {
      return new Dictionary<string, IGateway>() {
                { "GoogleMapsGeoCoding", new GatewayBuilder().Build() }
                ,{ "Bing", new GatewayBuilder().Build() }
                ,{ "Yahoo", new GatewayBuilder().Build() }
                ,{ "GutHub", new GatewayBuilder().Build() }
                ,{ "AceCommander", new GatewayBuilder().Build() }
            };
    }
  }
}
