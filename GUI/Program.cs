//using Microsoft.AspNetCore.Blazor.Browser.Rendering;
//using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
// Both are required for the logger/logging
using Microsoft.Extensions.Logging;
using Blazor.Extensions.Logging;
using GUI;

namespace GUI
{
  
    // Code for Blazor 0.6.0
     public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
            BlazorWebAssemblyHost.CreateDefaultBuilder()
                .UseBlazorStartup<Startup>();
    }
   

  /*
   * // Code for Blazor V0.4.0
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(services =>
            {
              // Add Blazor.Extensions.Logging.BrowserConsoleLogger; taken from the Blazor.Extensions.Logging NuGet package home page https://www.nuget.org/packages/Blazor.Extensions.Logging/# on 6/12/2018
              services.AddLogging(builder => builder
                  .AddBrowserConsole() // Register the logger with the ILoggerBuilder
                  .SetMinimumLevel(LogLevel.Debug) // Set the minimum log level to Information
              );
                // Add any additional custom services here
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
    */
}
