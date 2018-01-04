using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace AceAgent
{
    class AppConfiguration
    {
        public string ListeningOn { get; set; }
        public WindowConfiguration MainWindow { get; set; }
        public class WindowConfiguration
        {
            public int Height { get; set; }
            public int Width { get; set; }
            public int Left { get; set; }
            public int Top { get; set; }
        }
    }

    partial class Program
    {
        static public IConfiguration Configuration { get; set; }
        public static void GetConfiguration(string[]  args)            {
            IReadOnlyDictionary<string, string> DefaultConfigurationStrings = new Dictionary<string, string>()   {
                //["Profile:UserName"] = Environment.UserName,
              [$"AppConfiguration:ListeningOn"] = "http://localhost:21100/",
              [$"AppConfiguration:MainWindow:Height"] = "400",
              [$"AppConfiguration:MainWindow:Width"] = "600",
              [$"AppConfiguration:MainWindow:Top"] = "0",
              [$"AppConfiguration:MainWindow:Left"] = "0",
            };
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            // Add defaultConfigurationStrings
            configurationBuilder.AddInMemoryCollection(DefaultConfigurationStrings)
                .AddJsonFile("AceServiceConfig.json", true) // Bool indicates file is optional
                .AddEnvironmentVariables("AceServiceConfiguration")
                .AddCommandLine(args, GetSwitchMappings(DefaultConfigurationStrings))                   ;
            Program.Configuration= configurationBuilder.Build();

            }

        static public Dictionary<string, string> GetSwitchMappings(
  IReadOnlyDictionary<string, string> configurationStrings)
        {
            return configurationStrings.Select(item =>
              new KeyValuePair<string, string>(
                "-" + item.Key.Substring(item.Key.LastIndexOf(':') + 1),
                item.Key))
                .ToDictionary(
                  item => item.Key, item => item.Value);
        }
    }
}
