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


namespace AceAgent
{
    partial class  Program
    {
        
        static void Main(string[] args)
        {

            
            var appHost = new AppHost();
            //Allow you to debug your Windows Service while you're developing it. 
#if DEBUG
            Console.WriteLine("Running WinServiceAppHost in Console mode");
            try
            {
                appHost.Init();
                //appHost.Start(Configuration[$"AppConfiguration:ListeningOn"]);
                //Process.Start(Configuration[$"AppConfiguration:ListeningOn"]);
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
                new WinService(appHost, Configuration[$"AppConfiguration:ListeningOn"])
            };
            ServiceBase.Run(ServicesToRun);
#endif

            Console.ReadLine();
        }
    }
}
