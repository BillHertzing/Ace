using ATAP.Utilities.WebHostBuilders.Enumerations;
using ATAP.Utilities.ETW;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

using System.IO;
using System.ComponentModel;
using System;

namespace Ace.Agent.Host {
    partial class Program {
        #region genericHostBuilder creation / configuration
        // This Builder pattern creates a GenericHostBuilder populated by a specific web host as specified by a paramter
        public static IHostBuilder CreateSpecificHostBuilder(string[] args, IConfigurationRoot genericHostConfigurationRoot) {
            var hb = new HostBuilder()
            // The Generic Host Configuration. 
            .ConfigureHostConfiguration(configHostBuilder => {
                // Start with a "compiled-in defaults" for anything that is required to be provided in configuration for Production
                configHostBuilder.AddInMemoryCollection(GenericHostDefaultConfiguration.Production);
                // SetBasePath creates a Physical File provider, which will be used by the two following methods
                configHostBuilder.SetBasePath(Directory.GetCurrentDirectory());
                configHostBuilder.AddJsonFile(StringConstants.genericHostSettingsFileName+StringConstants.hostSettingsFileNameSuffix, optional: true);
                configHostBuilder.AddEnvironmentVariables(prefix: StringConstants.ASPNETCOREEnvironmentVariablePrefix);
                configHostBuilder.AddEnvironmentVariables(prefix: StringConstants.CustomEnvironmentVariablePrefix);
                // ToDo: get all (resolved) commandline args from genericHostConfigurationRoot.  Note the following does not include the command line switchMappings
                if (args!=null) {
                    configHostBuilder.AddCommandLine(args);
                }
            })
            // The genericHost loggers
            .ConfigureLogging((genericHostBuilderContext, loggingBuilder) => {
                // clear default loggingBuilder providers
                loggingBuilder.ClearProviders();
                // Read the Logging section of the ConfigurationRoot
                loggingBuilder.AddConfiguration(genericHostBuilderContext.Configuration.GetSection("Logging"));
                // Always provide these loggers regardless of Environment or WebHostBuilderToBuild
                var env = genericHostBuilderContext.Configuration.GetValue<string>(StringConstants.EnvironmentConfigRootKey);
                // use different logging providers based on both Environment and WebHostBuilderToBuild
                switch (env) {
                    case StringConstants.EnvironmentDevelopment:
                        // This is where many developer conveniences are configured for Development environment
                        // In the Development environment, Add Console and Debug Log providers (both are .Net Core provided loggers) 
                        loggingBuilder.AddConsole();
                        loggingBuilder.AddDebug();
                        loggingBuilder.AddSerilog();
                        break;
                    case StringConstants.EnvironmentProduction:
                        loggingBuilder.AddSerilog();
                        break;
                    default:
                        throw new NotImplementedException(String.Format(StringConstants.InvalidSupportedEnvironmentExceptionMessage, env));
                }

                //loggingBuilder.AddEventLog();
                //loggingBuilder.AddEventSourceLogger();
                //loggingBuilder.AddTraceSource(sourceSwitchName);
            })

            // the WebHost configuration
            .ConfigureAppConfiguration((genericHostBuilderContext, configWebHostBuilder) => {
                // Start with a "compiled-in defaults" for anything that is required to be provided in configuration  to the WebHost (IIS or Kestrel)
                //configWebHostBuilder.AddInMemoryCollection(DefaultConfiguration.aceCommanderWebHostConfigurationCompileTimeProduction);
                // Add additional required configuration variables to be provided in configuration for other environments
                string env = genericHostBuilderContext.Configuration.GetValue<string>(StringConstants.EnvironmentConfigRootKey);
                switch (env) {
                    case StringConstants.EnvironmentDevelopment:
                        // This is where many developer conveniences are configured for Development environment
                        // In the Development environment, modify the WebHostBuilder's Configuration settings to use the UserSecrets file
                        // This adds the UserSecrets file's Key:value pairs to the WebHostBuilder's Configuration for the specified userSecretsID
                        // See the CommonHost .csproj file for the value of the userSecretsID
                        // attribution: https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.0&tabs=windows
                        // ToDo: How do we implement an exception handler for issues with reading the file?
                        configWebHostBuilder.AddUserSecrets(userSecretsID);
                        break;
                    case StringConstants.EnvironmentProduction:
                        break;
                    default:
                        throw new NotImplementedException(String.Format(StringConstants.InvalidSupportedEnvironmentExceptionMessage, env));
                }
                // webHost configuration can see the global configuration, and will default to using the Physical File provider present in the GenericWebHost'scofiguration
                configWebHostBuilder.AddJsonFile(StringConstants.webHostSettingsFileName, optional: true);
                configWebHostBuilder.AddJsonFile(
                // ToDo: validate `genericHostBuilderContext.HostingEnvironment.EnvironmentName` has the same value as `env.ToString()`, especially case sensitivity
                $"webHostSettingsFileName.{genericHostBuilderContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                // ToDo: Investigate if adding web.config here is needed to support IISInProcesss hosted
                configWebHostBuilder.AddEnvironmentVariables(prefix: StringConstants.ASPNETCOREEnvironmentVariablePrefix);
                configWebHostBuilder.AddEnvironmentVariables(prefix: StringConstants.CustomEnvironmentVariablePrefix);
                // ToDo: get all (resolved) commandline args from genericHostBuilderContext.Configuration
                configWebHostBuilder.AddCommandLine(args);
            });
            SupportedWebHostBuilders webHostBuilderToBuild = genericHostConfigurationRoot.GetValue<SupportedWebHostBuilders>(StringConstants.WebHostBuilderToBuildConfigRootKey);
            hb.ConfigureWebHostDefaults(webHostBuilder => {
                switch (webHostBuilderToBuild) {
                    case SupportedWebHostBuilders.KestrelAloneWebHostBuilder:
                        webHostBuilder.UseKestrel();
                        // This (older) post has great info and examples on setting up the Kestrel options
                        //https://github.com/aspnet/KestrelHttpServer/issues/1334
                        // In V30P5, all SS interfaces return an error that "synchronous writes are disallowed", see following issue
                        //  https://github.com/aspnet/AspNetCore/issues/8302
                        // Woraround is to configure the default web server to AllowSynchronousIO=true
                        // ToDo: see if this is fixed in a release after V30P5
                        // Configure Kestrel
                        webHostBuilder.ConfigureKestrel((context, options) => {
                            options.AllowSynchronousIO=true;
                        });
                        break;
                    case SupportedWebHostBuilders.IntegratedIISInProcessWebHostBuilder:
                        webHostBuilder.UseIISIntegration();
                        break;
                    default:
                        throw new InvalidEnumArgumentException(StringConstants.InvalidWebHostBuilderToBuildExceptionMessage);
                }
                string env = genericHostConfigurationRoot.GetValue<string>(StringConstants.EnvironmentConfigRootKey);
                switch (env) {
                    case StringConstants.EnvironmentDevelopment:
                        // This is where many developer conveniences are configured for Development environment
                        // In the Development environment, modify the WebHostBuilder's settings to capture startup errors, and use the detailed error pages, 
                        webHostBuilder.CaptureStartupErrors(true)
                           .UseSetting("detailedErrors", "true");
                        break;
                    case StringConstants.EnvironmentProduction:
                        break;
                    default:
                        throw new InvalidEnumArgumentException(String.Format(StringConstants.InvalidSupportedEnvironmentExceptionMessage, env));
                }
                // Configure WebHost Logging to use Serilog
                //  webHostBuilder.UseSerilog(); // hmmm UseSesrilog does not exisit for the WebHost, just higher, for the GenericHost
                // Specify the class to use when starting the WebHost
                webHostBuilder.UseStartup<Startup>();
                // The URLS to ListenTo are part of the ConfigurationRoot, either from CompiledInDefaults, or as modified by later providers
                //  In fact, there are a number configuration information items preconfigured and available of from the ASPNETCORE_ Environment Variable name patterns
                //  that the Environment Variables Configuration Provider will pickup by default, as documented 
                //   https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.0#environment-variables-configuration-provider
            });
            return hb;
        }
        #endregion

    }
}