using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using ServiceStack.Text;

namespace Ace.AceService {
    partial class Program {

        // ToDo: Localize the string constants
        public const string hostEnvironmentNotRecoginedExceptionMessage = "The value {env} is not a recognized environment string";
        public const string ServiceNameBase = "AceCommander";
        public const string ServiceDisplayNameBase = "AceCommander";
        public const string ServiceDescriptionBase = "AceCommander";
        // Do Not localize below this point
        public const string EnvironmentVariablePrefix = "AceCommander";
        public const string EnvironmentVariableEnvironment = "Environment";
        public const string EnvironmentDefault = "Production";

        // Helper method to properly combine the prefix with the suffix
        static string EnvironmentVariableFullName(string name) { return EnvironmentVariablePrefix+"_"+name; }
        // ServiceStack Logging
        public static ILog Log;

        public static async Task Main(string[] args) {
            // To ensure every class uses the same Global Logger, set the LogManager's LogFactory before initializing the hosting environment
            //  set the LogFactory to ServiceStack's NLogFactory
            LogManager.LogFactory=new NLogFactory();
            Log=LogManager.GetLogger(typeof(Program));
            Log.Debug("Entering Program.Main");

            // When running as a service, the initial working dir is usually %WinDir%\System32, but the program (and configuration files) is probably installed to a different directory
            // Change the working dir to the location where the Exe and configuration files are installed to.
            var loadedFromDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Log.Debug($"in Program.Main: loadedFromDir is {loadedFromDir}");
            Directory.SetCurrentDirectory(loadedFromDir);

            // Determine the environment from an EnvironmentVariable
            var env = Environment.GetEnvironmentVariable(EnvironmentVariableFullName(EnvironmentVariableEnvironment))??EnvironmentDefault;

            // Create the genericHost hosting Kestrel WITHOUT IISIntegration, and Kestrel hosting ServiceStack
            // Create a self-hosted host with just Kestrel
            Log.Debug("in Program.Main: create webHostBuilderSelfHostedKestrel");
            /*
// Use the static contructor CreateGenericHostBuilder
IHostBuilder genericHostBuilder = CreateGenericHostBuilder(args);
// ToDo: Treat errors differently based on environment (Debug, or Production) - move to static builder constructor
/*
Log.Debug("in Program.Main: modify genericHostBuilder based on the environment in which the Net Core Host is executing ");
genericHostBuilder.ConfigureWebHostDefaults(webBuilder {
    .CaptureStartupErrors(true)
    .UseSetting("detailedErrors", "true")
    })
;

// Create the generic host
Log.Debug("in Program.Main: create genericHost by calling .Build() on the genericHostBuilder");
var genericHost= genericHostBuilder.Build();
// setup shutdown handlers
// process machine resources
// confirm parameters
// setup event handlers

// Start the webHost
Log.Debug("in Program.Main: genericHost created, starting RunAsync and awaiting it");
// ToDo: add a CancellationToken so that the web server and all middleware can listen and get notified when its time to stop
await genericHost.StartAsync();
*/
            // From P4 demo 07
            var webHostBuilderSelfHostedKestrel = new WebHostBuilder()
                .UseKestrel()
                // In V30P4, all SS interfaces return an error that "synchronous writes are disallowed", see following issue
                //  https://github.com/aspnet/AspNetCore/issues/8302
                // Woraround is to configure the default web server to AllowSynchronousIO=true
                // ToDo: see if this is fixed in a release after V30P4
                .ConfigureKestrel((context, options) => {
                    options.AllowSynchronousIO=true;
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")??"http://localhost:21200/")
                ;

            var webHostBuilder = webHostBuilderSelfHostedKestrel;

            // ToDo: Treat errors differently based on environment (Debug, or Production)
            Log.Debug("in Program.Main: modify webHostBuilder based on the environment in whihc the Net Core Host is executing ");
            webHostBuilder=webHostBuilder
                .CaptureStartupErrors(true)
                .UseSetting("detailedErrors", "true")
           ;
                
            // Create the web server host
            Log.Debug("in Program.Main: create webHost by calling .Build() on the webHostBuilder");
            var webHost = webHostBuilder.Build();

            // Start the webHost
            Log.Debug("in Program.Main: webHost created, starting RunAsync and awaiting it");

            await webHost.RunAsync();


            Log.Debug($"in Program.Main: webHost.RunAsync called at {DateTime.Now}, listening on {"ToDo: get the list of listening proto:host:port from the webHost"}");

            Console.WriteLine("press any key to close the hosting environment that is running as a ConsoleApp");
            Console.ReadKey();
            Log.Debug("Leaving Program.Main");
        }

        /*

        // This (older) post has great info and examples on setting up the Kestrel options
        //https://github.com/aspnet/KestrelHttpServer/issues/1334
        public static IHostBuilder CreateGenericHostBuilder(string[] args) {
            // CreateDefaultBuilder includes IISIntegration which is NOT desired, so
            // The Kestral Web Server must be manually configured into the Generic Host
            // Configure Kestral
            return new HostBuilder().ConfigureServices((hcontext,hservices) => {
                hservices.ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseKestrel()
                    // In V30P4, all SS interfaces return an error that "synchronous writes are disallowed", see following issue
                    //  https://github.com/aspnet/AspNetCore/issues/8302
                    // Woraround is to configure the default web server to AllowSynchronousIO=true
                    // ToDo: see if this is fixed in a release after V30P4
                     webBuilder.ConfigureKestrel((context, options) => {
                         options.AllowSynchronousIO=true;
                     })
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory())
                    webBuilder.UseStartup<Startup>()
                    webBuilder.UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")??"http://localhost:21200/");
                });
             });
        }
             */



    }

    public class Startup {
        static ILog Log { get; set; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration=configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // Create a logger instance for this class
            Log=LogManager.GetLogger(typeof(Startup));
            Log.Debug("Entering Program.Startup.ConfigureServices");
            Log.Debug("Leaving Program.Startup.ConfigureServices");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            // Create a logger instance for this class
            Log=LogManager.GetLogger(typeof(Startup));
            Log.Debug("Entering Program.Startup.Configure");
            //app.U UseBrowserLink();
            TestIDTypeSerialization();
            TestComplexTypeSerialization();


            app.UseServiceStack(new SSAppHost() {
                AppSettings=new NetCoreAppSettings(Configuration) // Use **appsettings.json** and config sources
            });

            // The supplied lambda becomes the final handler in the HTTP pipeline
            app.Run(async (context) => {
                Log.Debug("Last HTTP Pipeline handler");
                context.Response.StatusCode=404;
                await Task.FromResult(0);
            });
            // Temporary testing
            Log.Debug($"Temporary Testing");
            var Id = new Id<LongRunningTaskInfo>(Guid.NewGuid());
            Log.Debug($"Id = {Id.Dump()}");
            var IdStr = ServiceStack.Text.JsonSerializer.SerializeToString(Id);
            Log.Debug($"IdStr from ServiceStack.Text.JsonSerializer.SerializeToString(Id)= {IdStr}");
            var roundTripId = ServiceStack.Text.JsonSerializer.DeserializeFromString<Id<LongRunningTaskInfo>>(IdStr);
            Log.Debug($"roundTripId from ServiceStack.Text.JsonSerializer.DeserializeFromString<Id<LongRunningTaskInfo>>(IdStr) = {roundTripId.Dump()}");
            IdStr=Newtonsoft.Json.JsonConvert.SerializeObject(Id);
            Log.Debug($"IdStr from Newtonsoft.Json.JsonConvert.SerializeObject(Id) = {IdStr.Dump()}");
            roundTripId=Newtonsoft.Json.JsonConvert.DeserializeObject<Id<LongRunningTaskInfo>>(IdStr);
            Log.Debug($"roundTripId from Newtonsoft.Json.JsonConvert.DeserializeObject<Id<LongRunningTaskInfo>> = {roundTripId.Dump()}");
            // end temporary testing

            Log.Debug("Leaving Program.Startup.Configure");

        }
        // Temporary testing
        private void TestComplexTypeSerialization() {
            var cData = new Ace.Agent.BaseServices.ConfigurationData();
            Log.Debug($"cData = {cData.Dump()}");
            var cDataStr = ServiceStack.Text.JsonSerializer.SerializeToString(cData);
            Log.Debug($"cDataStr from ServiceStack.Text.JsonSerializer.SerializeToString(Id)= {cDataStr}");
            // ToDo: ask SS to Fix SS Deserializer in SS V5.5.1+
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
            var IdStr = ServiceStack.Text.JsonSerializer.SerializeToString(Id);
            Log.Debug($"IdStr from ServiceStack.Text.JsonSerializer.SerializeToString(Id)= {IdStr.Dump()}");
            var roundTripId = ServiceStack.Text.JsonSerializer.DeserializeFromString<Id<LongRunningTaskInfo>>(IdStr);
            Log.Debug($"roundTripId from ServiceStack.Text.JsonSerializer.DeserializeFromString<Id<LongRunningTaskInfo>>(IdStr) = {roundTripId.Dump()}");
            IdStr=Newtonsoft.Json.JsonConvert.SerializeObject(Id);
            Log.Debug($"IdStr from Newtonsoft.Json.JsonConvert.SerializeObject(Id) = {IdStr.Dump()}");
            roundTripId=Newtonsoft.Json.JsonConvert.DeserializeObject<Id<LongRunningTaskInfo>>(IdStr);
            Log.Debug($"roundTripId from Newtonsoft.Json.JsonConvert.DeserializeObject<Id<LongRunningTaskInfo>> = {roundTripId.Dump()}");
        }
    }
}
