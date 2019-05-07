using Microsoft.AspNetCore;
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
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

namespace Ace.AceService {
    partial class Program {
        public const string ServiceNameBase = "AceCommander";
        public const string ServiceDisplayNameBase = "AceCommander";
        public const string ServiceDescriptionBase = "AceCommander";
        public const string LifeCycleSuffix =
#if Debug
            "Dev";
#else
            "";
#endif

        public static ILog Log;

        public static async Task Main(string[] args) {
            //To ensure every ServiceStack service uses the same Global Logger, set it before you initialize ServiceStack's SSAppHost,
            LogManager.LogFactory=new NLogFactory();
            Log=LogManager.GetLogger(typeof(Program));
            Log.Debug("Entering Program.Main");

            // When running as a service, the initial working dir is usually %WinDir%\System32, but the program (and configuration files) is probably installed to a different directory
            // Change the working dir to the location where the Exe and configuration files are installed to.
            var loadedFromDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Log.Debug($"in Program.Main: loadedFromDir is {loadedFromDir}");
            Directory.SetCurrentDirectory(loadedFromDir);

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

            app.UseServiceStack(new SSAppHost() {
                AppSettings=new NetCoreAppSettings(Configuration) // Use **appsettings.json** and config sources
            });

            // The supplied lambda becomes the final handler in the HTTP pipeline
            app.Run(async (context) => {
                Log.Debug("Last HTTP Pipeline handler");
                context.Response.StatusCode=404;
                await Task.FromResult(0);
            });

            Log.Debug("Leaving Program.Startup.Configure");

        }
    }
}
