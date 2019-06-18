
namespace Ace.Agent.Host {
    public static class StringConstants {
        // ToDo: Localize the string constants
        public const string ASPNETCOREEnvironmentVariablePrefix = "ASPNETCORE_";
        public const string CustomEnvironmentVariablePrefix = "AceCommander_";


        public const string WebHostBuilderToBuildConfigRootKey = "WebHostBuilderToBuild";
        public const string WebHostBuilderStringDefault = "KestrelAloneWebHostBuilder";

        public const string EnvironmentProduction = "Production"; // Environments.Production
        public const string EnvironmentDevelopment = "Development";
        public const string EnvironmentDefault = EnvironmentProduction;
        public const string EnvironmentConfigRootKey = "Environment";

        public const string genericHostSettingsFileName = "genericHostSettings";
        public const string webHostSettingsFileName = "webHostSettings";
        public const string hostSettingsFileNameSuffix = ".json";


        public const string URLSConfigRootKey = "urls";

        public const string InvalidWebHostBuilderStringExceptionMessage = "The WebHostBuilder string {0} specified in the environment variable does not match any member of the SupportedWebHostBuilders enumeration.";
        public const string InvalidWebHostBuilderToBuildExceptionMessage = "The WebHostBuilder enumeration argument specified {0} is not supported.";
        public const string InvalidSupportedEnvironmentStringExceptionMessage = "The Environment string {0} specified in the environment variable does not match any member of the SupportedEnvironments enumeration.";
        public const string InvalidSupportedEnvironmentExceptionMessage = "The Environment enumeration argument specified {0} is not supported.";
        public const string InvalidCircularEnvironmentExceptionMessage = "The Environment \"Production\" should not be reached here";
        public const string InvalidRedeclarationOfEnvironmentExceptionMessage = "The initial Environment from the initialConfigurationRoot is {0}, and after reconfiguration, the Environment is {1}. this is a mis-match, and indicates a problem with the environment-specific configuration providers. Environment-specific configuration providers are not allowed to change the environment";

        public const string ConsoleAppConfigRootKey = "Console";

        public const string ServiceNameBase = "AceCommander";
        public const string ServiceDisplayNameBase = "AceCommander";
        public const string ServiceDescriptionBase = "AceCommander";
    }
}
