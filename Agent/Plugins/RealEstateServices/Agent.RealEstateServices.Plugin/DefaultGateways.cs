using System;
using System.Collections.Generic;
using ATAP.Utilities.Http;

namespace Ace.Plugin.RealEstateServices
{
  static class DefaultGateways
  {
    public static Dictionary<string, IGateway> Configuration()
    {
      return new Dictionary<string, IGateway>() {
                { "HomeAway", new GatewayBuilder().Build() }
                ,{ "Zillow",  new GatewayBuilder().Build() }
                ,{ "Realtor",  new GatewayBuilder().Build() }
                ,{ "RetSy",  new GatewayBuilder().Build() }
            };
    }
  }
}
