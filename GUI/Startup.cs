
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

// Both are required for the logger/logging
using Microsoft.Extensions.Logging;
using Blazor.Extensions.Logging;

// Local Storage provided by Blazored.LocalStorage
using Blazored.LocalStorage;
using System;
using ATAP.Utilities.IJSON;
using ATAP.Utilities.IJSON.STJ;

namespace GUI {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            // Add an implementation of the IJSON interface
            services.AddSingleton<ATAP.Utilities.IJSON.IJSON>(new ATAP.Utilities.IJSON.STJ.IJSONSTJ());
            // Add Blazor.Extensions.Logging.BrowserConsoleLogger; taken from the Blazor.Extensions.Logging NuGet package home page https://www.nuget.org/packages/Blazor.Extensions.Logging/# on 6/12/2018
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
            // ToDo: this is experimental Add a scoped instance of a UriBuilder
            services.AddScoped<UriBuilder, LocalUriBuilder>();

        }

        public void Configure(IComponentsApplicationBuilder app) {
            app.AddComponent<App>("app");
        }
    }

    // Create a 
    public class LocalUriBuilder : UriBuilder {
        public LocalUriBuilder() :this("http://localhost:21200/"){}

        public LocalUriBuilder(string uri) : base(uri) {
         }

        public LocalUriBuilder(Uri uri) : base(uri) {
        }

        public LocalUriBuilder(string schemeName, string hostName) : base(schemeName, hostName) {
        }

        public LocalUriBuilder(string scheme, string host, int portNumber) : base(scheme, host, portNumber) {
        }

        public LocalUriBuilder(string scheme, string host, int port, string pathValue) : base(scheme, host, port, pathValue) {
        }

        public LocalUriBuilder(string scheme, string host, int port, string path, string extraValue) : base(scheme, host, port, path, extraValue) {
        }
    }
}

