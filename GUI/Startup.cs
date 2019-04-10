using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

// Both are required for the logger/logging
using Microsoft.Extensions.Logging;
//using Blazor.Extensions.Logging;


namespace GUI
{

  //This file and this class for standalone blazor appeared between V0.4.0 and V0.6.0
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      // Add Blazor.Extensions.Logging.BrowserConsoleLogger; taken from the Blazor.Extensions.Logging NuGet package home page https://www.nuget.org/packages/Blazor.Extensions.Logging/# on 6/12/2018
      //services.AddLogging(builder => builder
      //    .AddBrowserConsole() // Register the logger with the ILoggerBuilder
      //    .SetMinimumLevel(LogLevel.Debug) // Set the minimum log level to Information
      //);
    }

    public void Configure(IComponentsApplicationBuilder app)
    {
      app.AddComponent<App>("app");
    }
  }
}

