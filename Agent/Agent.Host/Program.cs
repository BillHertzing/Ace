using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

using Serilog;
//Required for Serilog.SelfLog
using Serilog.Debugging;

using ServiceStack;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using ATAP.Utilities.ETW;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.Runtime.Enumerations;
using ATAP.Utilities.WebHostBuilders.Enumerations;

namespace Ace.Agent.Host {

    partial class Program {
        // Log Program Startup to ETW (as of 06/2019, ILWeaving this assembly results in a thrown invalid CLI Program Exception
        // ATAP.Utilities.ETW.ATAPUtilitiesETWProvider.Log.MethodBoundry("<");

        // Extend the CommandLine Configuration Provider with these switch mappings
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.0#switch-mappings
        public static readonly Dictionary<string, string> switchMappings =
            new Dictionary<string, string>
            {
                { "-Console", StringConstants.ConsoleAppConfigRootKey },
                { "-C", StringConstants.ConsoleAppConfigRootKey },
            };

        public const string userSecretsID = "E5D6C5E5-6E30-49EF-BE15-E1B7C377D2A7";

        public static async Task Main(string[] args) {

            // Setup Serilog's static logger with an initial configuration sufficient to log statup errors
            Serilog.Core.Logger genericHostStartupNaubLogger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                //.Enrich.WithHttpRequestId()
                //.Enrich.WithUserName()
                //.WithExceptionDetails()
                .WriteTo.Seq(serverUrl: "http://localhost:5341")
                .WriteTo.Debug()
                //.WriteTo.File(path: "Logs/Demo.Serilog.{Date}.log", fileSizeLimitBytes: 1024, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, retainedFileCountLimit: 31)
                .CreateLogger();

            // When running as a service, the initial working dir is usually %WinDir%\System32, but the program (and configuration files) is probably installed to a different directory
            // Change the working directory to the location where the Exe and configuration files are installed to.
            var loadedFromDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Log.Debug("in Program.Main: loadedFromDir is {loadedFromDir}", loadedFromDir);
            Directory.SetCurrentDirectory(loadedFromDir);

            // Create the initialConfigurationBuilder for this genericHost. This creates an ordered chain of configuration providers. The first providers in the chain have the lowest priority, the last providers in the chain have a higher priority.
            //  Initial configuration does not take Environment into account. 
            var initialGenericHostConfigurationBuilder = new ConfigurationBuilder()
                // Start with a "compiled-in defaults" for anything that is REQUIRED to be provided in configuration for Production
                .AddInMemoryCollection(GenericHostDefaultConfiguration.Production)
                // SetBasePath creates a Physical File provider, which will be used by the following methods
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(StringConstants.genericHostSettingsFileName+StringConstants.hostSettingsFileNameSuffix, optional: true)
                .AddEnvironmentVariables(prefix: StringConstants.ASPNETCOREEnvironmentVariablePrefix)
                .AddEnvironmentVariables(prefix: StringConstants.CustomEnvironmentVariablePrefix)
                // Add commandline switch provider and map -console to --console:false
                .AddCommandLine(args, switchMappings);

            // Create this program's initial ConfigurationRoot
            var initialGenericHostConfigurationRoot = initialGenericHostConfigurationBuilder.Build();

            // Determine the environment (Debug, TestingUnit, TestingX, QA, QA1, QA2, ..., Staging, Production) to use from the initialGenericHostConfigurationRoot
            var initialEnvName = initialGenericHostConfigurationRoot.GetValue<string>(StringConstants.EnvironmentConfigRootKey, StringConstants.EnvironmentDefault);
            Log.Debug("in Program.Main: initialEnvName from initialGenericHostConfigurationRoot = {initialEnvName}", initialEnvName);

            // declare the final ConfigurationRoot for this genericHost, and set it to the initialGenericHostConfigurationRoot
            IConfigurationRoot genericHostConfigurationRoot = initialGenericHostConfigurationRoot;

            // If the initialGenericHostConfigurationRoot specifies the Environment is production, then the final genericHostConfigurationRoot is correect 
            //   but if not, build a 2nd genericHostConfigurationBuilder and .Build it to create the genericHostConfigurationRoot

            // Validate the environment provided is one this progam understands how to use, and create the final genericHostConfigurationRoot
            // The first switch statement in the following block also provides validation the the initialEnvName is one that this program understands and knows how to use
            if (initialEnvName!=StringConstants.EnvironmentProduction) {
                // Recreate the ConfigurationBuilder for this genericHost, this time including environment-specific configuration providers.
                IConfigurationBuilder genericHostConfigurationBuilder = new ConfigurationBuilder()
                // Start with a "compiled-in defaults" for anything that is REQUIRED to be provided in configuration for Production
                .AddInMemoryCollection(GenericHostDefaultConfiguration.Production);
                // Only Production is compiled in by default, all non-production uses are configured with settings. there are no environment-specific "compiled-in defaults"
                switch (initialEnvName) {
                    case StringConstants.EnvironmentDevelopment:
                        //genericHostConfigurationBuilder.AddInMemoryCollection(Ace.Agent.CommonHost.GenericHostDefaultConfiguration.Development);
                        break;
                    case StringConstants.EnvironmentProduction:
                        throw new InvalidOperationException(String.Format(StringConstants.InvalidCircularEnvironmentExceptionMessage, initialEnvName));
                    default:
                        throw new NotImplementedException(String.Format(StringConstants.InvalidSupportedEnvironmentExceptionMessage, initialEnvName));
                }
                // SetBasePath creates a Physical File provider, which will be used by the following methods that read files
                genericHostConfigurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(StringConstants.genericHostSettingsFileName+StringConstants.hostSettingsFileNameSuffix, optional: true);
                // Add environment-specific settings file
                switch (initialEnvName) {
                    case StringConstants.EnvironmentDevelopment:
                        genericHostConfigurationBuilder.AddJsonFile(StringConstants.genericHostSettingsFileName+"."+initialEnvName+StringConstants.hostSettingsFileNameSuffix, optional: true);
                        break;
                    case StringConstants.EnvironmentProduction:
                        throw new InvalidOperationException(String.Format(StringConstants.InvalidCircularEnvironmentExceptionMessage, initialEnvName));
                    default:
                        throw new NotImplementedException(String.Format(StringConstants.InvalidSupportedEnvironmentExceptionMessage, initialEnvName));
                }
                genericHostConfigurationBuilder
                    .AddEnvironmentVariables(prefix: StringConstants.ASPNETCOREEnvironmentVariablePrefix)
                    .AddEnvironmentVariables(prefix: StringConstants.CustomEnvironmentVariablePrefix)
                    .AddCommandLine(args);
                // Set the final genericHostConfigurationRoot to the .Build() results
                genericHostConfigurationRoot=genericHostConfigurationBuilder.Build();
            }

            // Validate that the current Environment matches the Environment from the initialConfigurationRoot
            var envName = genericHostConfigurationRoot.GetValue<string>(StringConstants.EnvironmentConfigRootKey, StringConstants.EnvironmentDefault);
            Log.Debug("in Program.Main: envName from genericHostConfigurationRoot = {envName}", envName);
            if (initialEnvName!=envName) {
                throw new InvalidOperationException(String.Format(StringConstants.InvalidRedeclarationOfEnvironmentExceptionMessage, initialEnvName, envName));
            }

            // Setup the Microsoft.Logging.Extensions Logging
            // One of what seems to me to be a limitation, is, the configuration needs to exist before logging can be read from it, so, 
            //    the whole process of getting the environment above, has to be done without the loggers. That seems... wrong?

            // Serilog is the logging provider I picked to provide a logging solution more robust than NLog/
            //  MLE is anacroynm for Microsoft.Logging.Extensions
            //  Serilog.ILogger MLELog;

            // Enable Serilog's internal debug logging. Note that internal logging will not write to any user-defined sinks
            //  https://github.com/serilog/serilog-sinks-file/blob/dev/example/Sample/Program.cs
            SelfLog.Enable(Console.Out);
            // Another example is at https://stackify.com/serilog-tutorial-net-logging/
            //  This brings in the System.Diagnostics.Debug namespace and writes the SelfLog there
            // Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            // Create a Serilog logger based on the ConfigurationRoot
            //Serilog.Core.Logger x = new LoggerConfiguration().ReadFrom.Configuration(genericHostConfigurationRoot).CreateLogger();
            // Example of setting up Serilogger in code, instead of configuration
            //Serilog.Core.Logger y = new LoggerConfiguration()
            //    .MinimumLevel.Verbose()
            //    .Enrich.FromLogContext()
            //    .Enrich.WithThreadId()
            //    //.Enrich.WithHttpRequestId()
            //    //.Enrich.WithUserName()
            //    //.WithExceptionDetails()
            //    .WriteTo.Seq(serverUrl: "http://localhost:5341")
            //    //.WriteTo.Udp(remoteAddress:IPAddress.Loopback, remotePort:9999, formatter:default) // I could not get it to write to Sentinel
            //    .WriteTo.Console(theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code)
            //    .WriteTo.Debug()
            //    .WriteTo.File(path: "Logs/Demo.Serilog.{Date}.log", fileSizeLimitBytes: 1024, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, retainedFileCountLimit: 31)
            //    .CreateLogger();

            // The Serilog.Log is a static entry to the Serilog logging provider
            // Create a Serilog logger based on the ConfigurationRoot and assign it to the static Serilog.Log object
            // Configure logging based on the information in ConfigurationRoot
            Log.Logger=new LoggerConfiguration().ReadFrom.Configuration(genericHostConfigurationRoot).CreateLogger();
            Log.Debug("in Program.Main: Environment = {envName}", envName);

            // Validate the value of WebHostBuilderToBuild from the genericHostConfigurationRoot is one that is supported by this program
            var webHostBuilderName = genericHostConfigurationRoot.GetValue<string>(StringConstants.WebHostBuilderToBuildConfigRootKey, StringConstants.WebHostBuilderStringDefault);
            SupportedWebHostBuilders webHostBuilderToBuild;
            if (!Enum.TryParse<SupportedWebHostBuilders>(webHostBuilderName, out webHostBuilderToBuild)) {
                throw new InvalidDataException(String.Format(StringConstants.InvalidWebHostBuilderStringExceptionMessage, webHostBuilderName));
            }
            Log.Debug("in Program.Main: webHostBuilderToBuild = {webHostBuilderToBuild}", webHostBuilderToBuild);

            // temporary testing 
            var tID = new Id<LongRunningTaskInfo>(new Guid());
            //Log.Debug("tID.ToString(): {temp_tidToString}", tID.ToString());
            //Log.Debug("tID.Dump(): {temp_tidDump}", tID.Dump());
            // Log.Debug("tID(object): {temp_tidObject}", tID);
            // End temporary testing 


            // During Development, the genericHost runs as a ConsoleHost. In production it runs as a service (Windows) or daemon (Linux)
            // Before creating the genericHostBuilder, we need to know if the program should be running as a console host or as a service
            // There are two conditions for this: 
            //  if a debugger is attached at this point in the program's execution
            //  if command line args contains --console or -console or -c
            //  previously we added a switchMappings to the CommandlineArgs configuration provider, so, we can just get the value of console from the ConfigurationRoot
            bool isConsoleHost = Debugger.IsAttached||genericHostConfigurationRoot.GetValue(StringConstants.ConsoleAppConfigRootKey, false);
            //  A class that helps determine if running under Windows or Linux
            RuntimeKind runtimeKind = new RuntimeKind(isConsoleHost);
            Log.Debug("in Program.Main: runtimeKind = {@runtimeKind}", runtimeKind);

            // Introduce a Cancellation token source. This is a all-method cancellation token source, that can be used to signal the genericHost regardless of having it configured with a ConsoleApplication lifetime or a Service lifetime
            CancellationTokenSource genericHostCancellationTokenSource = new CancellationTokenSource();
            // and its token
            CancellationToken genericHostCancellationToken = genericHostCancellationTokenSource.Token;

            // Build and run the generic host, either as a ConsoleApp, or as a service/daemon
            // Attribution to https://www.stevejgordon.co.uk/running-net-core-generic-host-applications-as-a-windows-service
            // Attribution to https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-3.0

            // Create the GenericHostBuilder instance based on the ConfigurationRoot
            Log.Debug("in Program.Main: create genericHostBuilder by calling static method CreateSpecificHostBuilder");
            IHostBuilder genericHostBuilder = CreateSpecificHostBuilder(args, genericHostConfigurationRoot);
            //Log.Debug("in Program.Main: genericHostBuilder = {@genericHostBuilder}", genericHostBuilder);

            Log.Debug("in Program.Main: IsConsoleApplication = {IsConsoleApplication}", runtimeKind.IsConsoleApplication);
            if (!runtimeKind.IsConsoleApplication) {
                Log.Debug("in Program.Main: extend genericHostBuilder by calling extension method .ConfigureServices and adding to the DI Container a singleton instance of AceAsServiceLifetimeKingpin of type IHostLifetime");
                genericHostBuilder.ConfigureServices((hostContext, services) => {
                    services.AddSingleton<IHostLifetime, GenericHostAsServiceLifetimeLynchpin>();
                    // add Host options here if needed
                    // Extend the generic host timeout to thecvalue specified in the configuration, in seconds, to give all running process time to do a graceful shutdown
                    // Ensure that the string value specified in the ConfigurationRoot can be converted to a double
                    String shutDownTimeoutInSecondsString = genericHostConfigurationRoot.GetValue<string>(StringConstants.MaxTimeInSecondsToWaitForGenericHostShutdownConfigKey, StringConstants.MaxTimeInSecondsToWaitForGenericHostShutdownStringDefault);
                    Double shutDownTimeoutInSecondsDouble;
                    if (!Double.TryParse(shutDownTimeoutInSecondsString, out shutDownTimeoutInSecondsDouble)) {
                        throw new InvalidCastException(String.Format(StringConstants.CannotConvertShutDownTimeoutInSecondsToDoubleExceptionMessage, shutDownTimeoutInSecondsString));
                    }
                    services.AddOptions<HostOptions>().Configure(opts => opts.ShutdownTimeout=TimeSpan.FromSeconds(shutDownTimeoutInSecondsDouble));
                });
            } else {
                Log.Debug("in Program.Main: extend genericHostBuilder by calling the extension method UseConsoleLifetime");
                genericHostBuilder.UseConsoleLifetime();
            }

            // Create the generic host genericHost
            Log.Debug("in Program.Main: create genericHost by calling .Build() on the genericHostBuilder");
            var genericHost = genericHostBuilder.Build();
            //Log.Debug("in Program.Main: genericHost.Dump() = {@genericHost}", genericHost);

            Log.Debug("in Program.Main: genericHost created, starting RunAsync, listening on {urls} and awaiting it", genericHostConfigurationRoot.GetValue<string>(StringConstants.URLSConfigRootKey));
            // Start the generic host
            Log.Debug("in Program.Main: webHost created, starting RunAsync and awaiting it");

            // Attribution to https://stackoverflow.com/questions/52915015/how-to-apply-hostoptions-shutdowntimeout-when-configuring-net-core-generic-host for OperationCanceledException notes
            // ToDo: further investigation to ensure OperationCanceledException should be ignored in all cases (service or console host, kestrel or IntegratedIISInProcessWebHost)
            try {
                // Run it Async
                await genericHost.RunAsync(genericHostCancellationToken);
            }
            catch (OperationCanceledException e) {
                ; // Just ignore it to suppress it
                  // The Exception should be shown in the ETW trace
                  //ToDo: Add Error level or category to ATAPUtilitiesETWProvider
                  //ATAPUtilitiesETWProvider.Log.Information($"Exception in Program.Main: {e.Exception.GetType()}: {e.Exception.Message}");
            }
            // The IHostLifetime instance methods take over now
            // Log Program finishing to ETW if it happens to resume executionhere for some reason(as of 06/2019, ILWeaving this assembly results in a thrown invalid CLI Program Exception
            // ATAP.Utilities.ETW.ATAPUtilitiesETWProvider.Log("> Program.Main");

        }
    }

    [ATAP.Utilities.ETW.ETWLogAttribute]
    public class Startup {
        public IConfiguration Configuration { get; }

        public IHostEnvironment HostEnvironment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment) {
            Configuration=configuration;
            HostEnvironment=hostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment hostEnvironment) {

            app.UseServiceStack(new SSAppHost(hostEnvironment) {
                AppSettings=new NetCoreAppSettings(Configuration)
            });

            // The supplied lambda becomes the final handler in the HTTP pipeline
            app.Run(async (context) => {
                Log.Debug("Last HTTP Pipeline handler");
                context.Response.StatusCode=404;
                await Task.FromResult(0);
            });
            // Temporary testing
            // TestIDTypeSerialization();
            // TestComplexTypeSerialization();
            // end temporary testing
        }
        // Temporary testing
        private void TestComplexTypeSerialization() {
            var cData = new Ace.Agent.BaseServices.ConfigurationData();
            Log.Debug($"cData = {cData.Dump()}");
            var cDataStr = JsonSerializer.SerializeToString(cData);
            Log.Debug($"cDataStr from ServiceStack.Text.JsonSerializer.SerializeToString(Id)= {cDataStr}");
            // ToDo: ask SS to Fix SS Deserializer in SS V5.5.1+ when running under Mono-WASM runtime. fails under Preview5 when run in a client-side Blazor app. passes server-side
            //var roundTripcData = ServiceStack.Text.JsonSerializer.DeserializeFromString<Ace.Agent.BaseServices.ConfigurationData>(cDataStr);
            //Log.LogDebug($"roundTripcData from ServiceStack.Text.JsonSerializer.DeserializeFromString<Ace.Agent.BaseServices.ConfigurationData>(cDataStr) = {roundTripcData.Dump()}");
            Log.Debug($"End Temporary Testing");
        }
        private void TestIDTypeSerialization() {

            // Testing for serialization of ID<T> instances
            // Temporary testing
            Log.Debug($"Temporary Testing");
            var Id = new Id<LongRunningTaskInfo>(Guid.NewGuid());
            Log.Debug($"Id = {Id.Dump()}");
            var IdStr = JsonSerializer.SerializeToString(Id);
            Log.Debug($"IdStr from ServiceStack.Text.JsonSerializer.SerializeToString(Id)= {IdStr.Dump()}");
            var roundTripId = JsonSerializer.DeserializeFromString<Id<LongRunningTaskInfo>>(IdStr);
            Log.Debug($"roundTripId from ServiceStack.Text.JsonSerializer.DeserializeFromString<Id<LongRunningTaskInfo>>(IdStr) = {roundTripId.Dump()}");
            // use the static instance of the System.Text.Json.Serialization.JsonSerializer (from .Net Core) found in the runtime this programa is running on
            IdStr=System.Text.Json.Serialization.JsonSerializer.ToString(Id);
            Log.Debug($"IdStr from System.Text.Json.Serialization.JsonSerializer.ToString(Id) = {IdStr.Dump()}");
            roundTripId=System.Text.Json.Serialization.JsonSerializer.Parse<Id<LongRunningTaskInfo>>(IdStr);
            Log.Debug($"roundTripId from Newtonsoft.Json.JsonConvert.DeserializeObject<Id<LongRunningTaskInfo>> = {roundTripId.Dump()}");
            // use the static instance of the Newtonsoft.Json.JsonConvert.SerializeObject (from a library package) found in the runtime or included in the distribution this programa is running on
            IdStr=Newtonsoft.Json.JsonConvert.SerializeObject(Id);
            Log.Debug($"IdStr from Newtonsoft.Json.JsonConvert.SerializeObject(Id) = {IdStr.Dump()}");
            roundTripId=Newtonsoft.Json.JsonConvert.DeserializeObject<Id<LongRunningTaskInfo>>(IdStr);
            Log.Debug($"roundTripId from Newtonsoft.Json.JsonConvert.DeserializeObject<Id<LongRunningTaskInfo>> = {roundTripId.Dump()}");
        }
    }
}
