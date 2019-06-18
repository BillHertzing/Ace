using System;

namespace Agent.GUIServices.Shared {
    #region ConfigurationData
    public class ConfigurationData {
        //ToDo: 
        public ConfigurationData() : this(string.Empty,string.Empty) { }

        public ConfigurationData(string identifier, string version) {
            Identifier=identifier;
            Version=version;
        }

        public string Identifier { get; set; }
        public string Version { get; set; }
    }
    #endregion

    #region UserData
    public class UserData {
        public UserData() : this(string.Empty) { }
        public UserData(string placeholder) {
            Placeholder=placeholder;
        }

        public string Placeholder { get; set; }
    }
    #endregion UserData

}
