using System;

namespace Ace.PlugIn.AMQPServices {
    #region ConfigurationData
    public class ConfigurationData
    {
        public string AMQPConnectionString { get; set; }

        public ConfigurationData(string aMQPConnectionString, string version)
        {
            AMQPConnectionString = aMQPConnectionString;
        }

    }
    #endregion

    #region UserData
    public class UserData
    {
        public UserData(string placeholder)
        {
            Placeholder = placeholder;
        }

        public string Placeholder { get; set; }
    }
    #endregion UserData
}