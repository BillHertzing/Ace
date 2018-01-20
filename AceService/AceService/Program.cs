using System;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;
using ServiceStack.Configuration;
using ServiceStack.Logging;

namespace Ace.AceService
{
    partial class  Program
    {
        public static ILog Log = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
            //To ensure every ServiceStack service uses the same Global Logger, set it before you initialize ServiceStack's AppHost,
            LogManager.LogFactory = new DebugLogFactory();
            Log.Debug("starting Program.Main");
            // setup shutdown handlers
            // process machine resources
            // confirm parameters
            // setup event handlers
            // start main processing thread
            var appHost = new AppHost();
            // Run the service in a console window when built in Debug configuration, to allow for debugging during development
#if DEBUG
            Console.WriteLine("Running WinServiceAppHost in Console mode");

            try
            {
                appHost.Init();
                appHost.Start(appHost.AppSettings.Get<string>("Ace.AceService:ListeningOn"));
                Process.Start(appHost.AppSettings.Get<string>("Ace.AceService:ListeningOn"));
                Console.WriteLine("Press <CTRL>+C to stop.");
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}: {1}", ex.GetType().Name, ex.Message);
                throw;
            }
            finally
            {
                appHost.Stop();
            }

            Console.WriteLine("WinServiceAppHost has finished");

#else
            //When in RELEASE mode it will run as a Windows Service with the code below

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new WinService(appHost, Configuration[$"AppConfiguration:ListeningOn"]) //appHost.AppSettings.Get<string>("Ace.AceService:ListeningOn")?
            };
            ServiceBase.Run(ServicesToRun);
#endif

            Console.ReadLine();
        }
    }
}
