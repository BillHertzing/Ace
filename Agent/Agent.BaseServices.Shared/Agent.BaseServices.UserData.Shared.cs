using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Agent.BaseServices
{
  public class UserData
  {
    public UserData() : this(string.Empty, string.Empty) { }
    public UserData(string gatewayNameString, string gatewayEntryAPIKeyString)
    {
      GatewayNameString = gatewayNameString;
      GatewayEntryAPIKeyString = gatewayEntryAPIKeyString;
    }
    public string GatewayNameString { get; set; }
    public string GatewayEntryAPIKeyString { get; set; }
  }
}
