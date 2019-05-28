
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

// Both are required to configure the logger/logging
using Microsoft.Extensions.Logging;
using Blazor.Extensions.Logging;

// Local Storage provided by Blazored.LocalStorage
using Blazored.LocalStorage;
using System;

namespace GUI {

    //This file and this class for standalone blazor appeared between V0.4.0 and V0.6.0
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            // Add Blazor.Extensions.Logging.BrowserConsoleLogger
            // taken from the Blazor.Extensions.Logging NuGet package home page https://www.nuget.org/packages/Blazor.Extensions.Logging/# 
            // original on 6/12/2018, updated for V3.0 P4 on 5/5/19
            services.AddLogging(builder => builder
                .AddBrowserConsole() // Register the logger with the ILoggerBuilder
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

