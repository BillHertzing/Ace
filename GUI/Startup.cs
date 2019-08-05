
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

// Both are required for the logger/logging
using Microsoft.Extensions.Logging;
using Blazor.Extensions.Logging;

// Local Storage provided by Blazored.LocalStorage
using Blazored.LocalStorage;
using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GUI {
    public class Startup {
        public IConfiguration Configuration { get; }

        public IHostEnvironment HostEnvironment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services) {
            // Add Blazor.Extensions.Logging.BrowserConsoleLogger; taken from the Blazor.Extensions.Logging NuGet package home page https://www.nuget.org/packages/Blazor.Extensions.Logging/# on 6/12/2018
            // So that Fody and the MethodBoundryAspect ILWeaver can use the logging provider, register both the ILogger and the ILoggerFactory
            services.AddLogging(builder => builder
                // Register the Blazor.Extensions.Logging logger with the Microsoft.Extensions.Logging.ILoggerBuilder
                .AddBrowserConsole()
                // Setting minimum LogLevel to Trace enables detailed tracing
                // Setting minimum LogLevel to Debug disables detailed tracing, but will show LogDebug logging calls
                .SetMinimumLevel(LogLevel.Debug) // Set the minimum log level to Debug
            );
            // Add a library that enables local storage on the browser
            //  https://github.com/Blazored/LocalStorage
            services.AddBlazoredLocalStorage();
        }

        public void Configure(IComponentsApplicationBuilder app) {
            app.AddComponent<App>("app");
        }
    }

}

