using System;
using System.Collections.Generic;

namespace Ace.Agent.GUIServices{
  static class DefaultConfiguration
  {
    public static Dictionary<string, string> Configuration()
    {
      return new Dictionary<string, string>() {
                { "DebugRelativeRootPath", "../../../../../GUI/bin/Debug/netstandard2.0/Publish/GUI/dist" },
                { "ReleaseRelativeRootPath", "./GUI" },
                { "VirtualRootPath", string.Empty },
                { "", "" }
            };
    }
  }
}
