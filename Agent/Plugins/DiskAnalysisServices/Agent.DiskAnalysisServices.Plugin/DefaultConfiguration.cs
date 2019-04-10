using System;
using System.Collections.Generic;

namespace Ace.Agent.DiskAnalysisServices
{
  static class DefaultConfiguration
  {
    public static Dictionary<string, string> Configuration()
    {
      return new Dictionary<string, string>() {
                { "Placeholder", "placeholder" }
            };
    }
  }
}
