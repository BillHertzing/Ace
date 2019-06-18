using System;
using System.Collections.Generic;

namespace Ace.Agent.RealEstateServices
{
  static class DefaultConfiguration
  {
    public static Dictionary<string, string> Production => new Dictionary<string, string>() {
        { "HomeAway_API_URI", "https://homeaway.com/api/" },
        { "GoogleAPIKeyEncrypted", string.Empty },
        { "HomeAwayAPIKeyEncrypted", string.Empty },
        { "Google_API_URI", "https://google.com/api/" }
    };
  }
}
