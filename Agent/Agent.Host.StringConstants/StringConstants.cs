
namespace Ace.Agent.Host {
    public static class StringConstants {
        // ToDo: Localize the string constants

        #region string constants: File Names 
        public const string genericHostSettingsFileName = "genericHostSettings";
        public const string webHostSettingsFileName = "webHostSettings";
        public const string settingsTextFileSuffix = ".txt";
        public const string sSAppHostSettingsTextFileName = "SSAppHostSettings";
        public const string agentSettingsTextFileName = "Agent.BaseServicesSettings";
        public const string hostSettingsFileNameSuffix = ".json";
        //It would be nice if ServiceStack implemented the User Secrets pattern that ASP Core provides
        // Without that, the following string constant identifies an Environmental variable that can be populated with the name of a file
        public const string agentEnvironmentIndirectSettingsTextFileNameKey = "Agent.BaseServices.IndirectSettings.Path";
        #endregion

        #region string constants: Exception Messages
        public const string InvalidWebHostBuilderStringExceptionMessage = "The WebHostBuilder string {0} specified in the environment variable does not match any member of the SupportedWebHostBuilders enumeration.";
        public const string InvalidWebHostBuilderToBuildExceptionMessage = "The WebHostBuilder enumeration argument specified {0} is not supported.";
        public const string InvalidSupportedEnvironmentStringExceptionMessage = "The Environment string {0} specified in the environment variable does not match any member of the SupportedEnvironments enumeration.";
        public const string InvalidSupportedEnvironmentExceptionMessage = "The Environment enumeration argument specified {0} is not supported.";
        public const string InvalidCircularEnvironmentExceptionMessage = "The Environment \"Production\" should not be reached here";
        public const string InvalidRedeclarationOfEnvironmentExceptionMessage = "The initial Environment from the initialConfigurationRoot is {0}, and after reconfiguration, the Environment is {1}. this is a mis-match, and indicates a problem with the environment-specific configuration providers. Environment-specific configuration providers are not allowed to change the environment";
        public const string CannotConvertShutDownTimeoutInSecondsToDoubleExceptionMessage = "The value specified for a Shutdown Timeout ({0}) cannot be converted to a Double, please check the configuration settings";
        public const string cannotReadEnvironmentVariablesSecurityExceptionMessage = "Ace cannot read from the environment variables (Security)";
        public const string CouldNotCreateServiceStackVirtualFileMappingExceptionMessage = "Could not create ServiceStack Virtual File Mapping: ";
        public const string ConfigKeyGoogleMapsAPIKeyNotFoundExceptionMessage ="GoogleAPI Key does not exist in the AppSettings ";
        #endregion

        #region string constants: ConfigKeys and default values for string-based Configkeys
        public const string PhysicalRootPathConfigKey = "PhysicalRootPath";
        public const string PhysicalRootPathStringDefault = "./GUI/GUI";
        public const string WebHostBuilderToBuildConfigRootKey = "WebHostBuilderToBuild";
        public const string WebHostBuilderStringDefault = "KestrelAloneWebHostBuilder";
        public const string URLSConfigRootKey = "urls";
        public const string ConsoleAppConfigRootKey = "Console";
        public const string MaxTimeInSecondsToWaitForGenericHostShutdownConfigKey = "MaxTimeInSecondsToWaitForGenericHostShutdown";
        public const string MaxTimeInSecondsToWaitForGenericHostShutdownStringDefault = "10";
        public const string configKeyAceAgentListeningOnString = "Ace.Agent.ListeningOn";
        public const string configKeyRedisConnectionString = "RedisConnectionString";
        public const string configKeyMySqlConnectionString = "MySqlConnectionString";
        public const string configKeyGoogleMapsAPIKey = "GoogleMapsAPIKey";
        public const string PlugInsDirConfigKey = "PlugInsDir";
        public const string PlugInsDirStringDefault = "./PlugIns";


        #endregion

        #region string constants: EnvironmentVariablePrefixs
        public const string ASPNETCOREEnvironmentVariablePrefix = "ASPNETCORE_";
        public const string CustomEnvironmentVariablePrefix = "AceCommander_";
        #endregion

        // ToDo: replace with newest "best practices" that use IHostEnvironment
        #region string constants: Environments
        public const string EnvironmentProduction = "Production"; // Environments.Production
        public const string EnvironmentDevelopment = "Development";
        public const string EnvironmentDefault = EnvironmentProduction;
        public const string EnvironmentConfigRootKey = "Environment";
        #endregion

        #region string constants: For running as a Windows Service
        public const string ServiceNameBase = "AceCommander";
        public const string ServiceDisplayNameBase = "AceCommander";
        public const string ServiceDescriptionBase = "AceCommander";
        #endregion

    }
}
