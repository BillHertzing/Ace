using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Agent.BaseServices
{

  public class ConfigurationData
  {
    public ConfigurationData() : this(string.Empty, string.Empty) { }
    public ConfigurationData(string redisCacheConnectionString, string mySqlConnectionString)
    {
      RedisCacheConnectionString = redisCacheConnectionString;
      MySqlConnectionString = mySqlConnectionString;
    }
    public string RedisCacheConnectionString { get; set; }
    public string MySqlConnectionString { get; set; }
  }

}
