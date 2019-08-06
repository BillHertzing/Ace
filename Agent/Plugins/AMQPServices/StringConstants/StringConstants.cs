
namespace Ace.PlugIn.AMQPServices {

    public static class StringConstants {
        // ToDo: Localize the string constants
        #region Configuration Key strings
        public const string AMQPConnectionStringConfigKey = "AMQPConnectionString";
        #endregion 

        #region Exception Messages (string constants)
        public const string AMQPConnectionStringNotFoundExceptionMessage = "AMQPConnectionString ConfigKey not found in Plugin's Configuration setting, or the key is present but set to String.Empty. Add the AMQPConnectionStringConfigKey and Value to the Application Configuration, and retry.";
        #endregion

        #region File Name string constants
        public const string PluginSettingsTextFileName = "PlugIn.AMQPServices.Settings";
        public const string PluginSettingsTextFileSuffix = ".txt";
        #endregion
    }
}
