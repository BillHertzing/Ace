
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

// Both are required for the logger/logging
using Microsoft.Extensions.Logging;
using Blazor.Extensions.Logging;

// Local Storage provided by Blazored.LocalStorage
using Blazored.LocalStorage;

namespace GUI {
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            // Add Blazor.Extensions.Logging.BrowserConsoleLogger; taken from the Blazor.Extensions.Logging NuGet package home page https://www.nuget.org/packages/Blazor.Extensions.Logging/# on 6/12/2018
            // So that Fody and the MethodBoundryAspect ILWeaver can use the logging provider, register both the ILogger and the ILoggerFactory
            builder.Services.AddLogging(builder => builder
                // Register the Blazor.Extensions.Logging logger with the Microsoft.Extensions.Logging.ILoggerBuilder
                .AddBrowserConsole()
                // Setting minimum LogLevel to Trace enables detailed tracing
                // Setting minimum LogLevel to Debug disables detailed tracing, but will show LogDebug logging calls
                .SetMinimumLevel(LogLevel.Debug) // Set the minimum log level to Debug
            );
            // Add a library that enables local storage on the browser
            //  https://github.com/Blazored/LocalStorage
            builder.Services.AddBlazoredLocalStorage();

            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();
        }
    }
}
