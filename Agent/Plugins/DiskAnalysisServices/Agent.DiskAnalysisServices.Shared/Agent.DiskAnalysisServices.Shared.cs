using System;
using System.Collections.Generic;


namespace Ace.Plugin.DiskAnalysisServices
{
  #region ConfigurationData
  public class ConfigurationData
  {
    public ConfigurationData() : this(4095) { }

    public ConfigurationData(int blockSize)
    {
            BlockSize=blockSize;
    }

    public int BlockSize { get; set; }
  }
  #endregion

  #region UserData
  public class UserData
  {
    public UserData() : this(string.Empty) { }
    public UserData(string placeholder)
    {
      Placeholder = placeholder;
    }

    public string Placeholder { get; set; }
  }
  #endregion UserData

  
}
