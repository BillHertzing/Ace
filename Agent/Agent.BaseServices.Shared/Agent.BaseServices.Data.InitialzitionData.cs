using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Agent.BaseServices
{
  public class InitializationData
  {
    public InitializationData() : this(new ConfigurationData(), new UserData()) { }
    public InitializationData(ConfigurationData configurationData, UserData userData)
    {
      ConfigurationData = configurationData;
      UserData = userData;
    }
    public ConfigurationData ConfigurationData { get; set; }
    public UserData UserData { get; set; }
  }
}
