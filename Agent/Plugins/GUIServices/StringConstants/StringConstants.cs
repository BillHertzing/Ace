
namespace Ace.Agent.GUIServices {
    public static class StringConstants {
        // ToDo: Localize the string constants
        #region Configuration Key strings
        public const string GUIMapsConfigKey = "GUIMaps";
        public const string DefaultGUIVirtualRootPathConfigKey = "DefaultGUIVirtualRootPath";
        #endregion 

        #region Exception Messages (string constants)
        public const string DefaultGUIVirtualRootPathKeyOrValueNotFoundExceptionMessage = "DefaultGUIVirtualRootPath Key not found in Plugin's Configuration setting, or the key is present but set to String.Empty. Add the DefaultGUIVirtualRootPath Key and Value to the Plugin's Configuration, and retry.";
        public const string DefaultGUIVirtualRootPathDoesNotMatchExactlyOneGUIVirtualRootPathExceptionMessage = "The DefaultGUIVirtualRootPath found in Plugin's Configuration setting does exactly one VirtualRootPath across the collection of GUIs. Ensure the DefaultGUIVirtualRootPath in AppSettings matches at least one VirtualRootPath and retry.";
        public const string GUISKeyOrValueNotFoundExceptionMessage = "GUIS Key not found in Plugin's Configuration setting, or the key is present but set to String.Empty. Add the GUIS Key and Value to the Plugin's Configuration, and retry.";
        public const string RelativeToContentRootPathKeyOrValueNotFoundExceptionMessage = "RelativeToContentRootPath Key not found in Plugin's Configuration setting, or the key is present but set to String.Empty. Add the ReleaseRelativeRootPath Key and Value to the Plugin's Configuration, and retry.";
        public const string VirtualRootPathKeyOrValueNotFoundExceptionMessage = "VirtualRootPath Key not found in Plugin's Configuration setting. Add the VirtualRootPath Key and Value (null value is OK) to the Plugin's Configuration, and retry.";
        public const string RelativeRootPathValueContainsIlegalCharacterExceptionMessage = "relativeRootPathValue contains one or more characters that are illegal in a path. Ensure that the DebugRelativeRootPathKey's value and the ReleaseRelativeRootPathKey's value does not contain any characters that are illegal in a path, and retry.";
        #endregion

        #region File Name string constants
        public const string PluginSettingsTextFileName = "PlugIn.GUIServices.Settings";
        public const string PluginSettingsTextFileSuffix = ".txt";
        #endregion

        #region Templates
        public const string DefaultRedirectPathTemplate = "/{0}/index.html";
        #endregion
    }
}
