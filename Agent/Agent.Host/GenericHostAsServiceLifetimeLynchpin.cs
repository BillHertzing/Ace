using ATAP.Utilities.ETW;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;


namespace Ace.Agent.Host {
 
    // Attribution to https://dejanstojanovic.net/aspnet/2018/june/clean-service-stop-on-linux-with-net-core-21/
    // Attribution to https://www.stevejgordon.co.uk/running-net-core-generic-host-applications-as-a-windows-service
    // Attribution to https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0
    // Attribution to  https://github.com/aspnet/Hosting/blob/2a98db6a73512b8e36f55a1e6678461c34f4cc4d/samples/GenericHostSample/ServiceBaseLifetime.cs
    // Attribution to  https://stackoverflow.com/questions/52915015/how-to-apply-hostoptions-shutdowntimeout-when-configuring-net-core-generic-host
    // This class implements the service injected into the genericHost.  
    // This class implements the methods needed for IHostLifetime (WaitForStartAsync and StopAsync) to be a ConsoleLifetime (IHostLifetime derives from ConsoleLifetime)
    // This class derives from ServiceBase and implements the necessary adapters so the GenericHost can provide the implementation of the necessary delegates needed for Microsoft.Extensions.Hosting. ServiceLifetime IHostedService (StartAsync and StopAsync) 
    [ATAP.Utilities.ETW.ETWLogAttribute]
    public class GenericHostAsServiceLifetimeLynchpin : ServiceBase, IHostLifetime, IHostedService {
        IHostApplicationLifetime HostApplicationLifetime;
        ILogger<GenericHostAsServiceLifetimeLynchpin> logger;
        IHostEnvironment HostEnvironment;
        IConfiguration HostConfiguration;
        CancellationToken CancellationToken;

        // the _delayStart TaskCompletionSource exposes the task, its state and its results, accepts a cancellation request, and supports exception handling 
        private readonly TaskCompletionSource<object> _delayStart = new TaskCompletionSource<object>();

        // This creates an instance of a ServiceBase with the methods needed to control the GenericHost's Lifetime events
        //   the .Ctor loads the instance's properties with corresponding instances from the GenericHost when GenericHostAsServiceLifetimeLynchpin is created by the GenericHost
        //  This class will be registered as a Service in the GenericHosts Services collection
        public GenericHostAsServiceLifetimeLynchpin(
            IConfiguration hostConfiguration,
            IHostEnvironment hostEnvironment,
            ILogger<GenericHostAsServiceLifetimeLynchpin> logger,
            IHostApplicationLifetime hostApplicationLifetime) {
            this.logger=logger??throw new ArgumentNullException(nameof(logger));
            this.logger.LogInformation("Injected logger starting GenericHostAsServiceLifetimeLynchpin .ctor");

            HostApplicationLifetime =hostApplicationLifetime??throw new ArgumentNullException(nameof(hostApplicationLifetime));
            HostEnvironment=hostEnvironment??throw new ArgumentNullException(nameof(hostEnvironment));
            HostConfiguration=hostConfiguration??throw new ArgumentNullException(nameof(hostConfiguration));
            this.logger.LogInformation("leaving GenericHostAsServiceLifetimeLynchpin .ctor");
        }

        #region IHostLifetime and IHostedService Interfaces implementation
        // Used in IHostLifetime interface
        // Not called in ConsoleWindow mode
        // Hosting.Abstractions' StartAsync method calls this method at the start of StartAsync(CancellationToken)
        public Task WaitForStartAsync(CancellationToken cancellationToken) {
            this.logger.LogInformation("GenericHostAsServiceLifetimeLynchpin.WaitForStartAsync method called.");
            // Store away the CancellationToken passed as an argument
            CancellationToken=cancellationToken;
            // Register on that cancellationToken an Action that will call TrySetCanceled method on the _delayStart task.
            // This lets the cancellationToken passed into this method  signal to the genericHost an overall request for cancellation 
            CancellationToken.Register(() => _delayStart.TrySetCanceled());

            new Thread(Run).Start(); // Otherwise this would block and prevent IHost.StartAsync from finishing.
            this.logger.LogDebug("Leaving Program.GenericHostAsServiceLifetimeLynchpin.WaitForStartAsync method ");
            return _delayStart.Task;
        }

        // Used in IHostedService interface
        // in ConsoleWindow (debug) mode, this is called after Program.HostedWebServerStartup.Configure completes.
        public Task StartAsync(CancellationToken cancellationToken) {
            this.logger.LogInformation("GenericHostAsServiceLifetimeLynchpin.StartAsync method called.");
            // Store away the CancellationToken passed as an argument
            CancellationToken=cancellationToken;
            // Register on that cancellationToken an Action that will call TrySetCanceled method on the _delayStart task.
            // This lets the cancellationToken passed into this method  signal to the genericHost an overall request for cancellation 
            CancellationToken.Register(() => _delayStart.TrySetCanceled());
            // Register the methods defined in this class with the three CancellationToken properties found on the IHostApplicationLifetime instance passed to this class in it's .ctor
            HostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            HostApplicationLifetime.ApplicationStopping.Register(OnStopping);
            HostApplicationLifetime.ApplicationStopped.Register(OnStopped);
            this.logger.LogInformation("Leaving Program.GenericHostAsServiceLifetimeLynchpin.StartAsync method ");
            return Task.CompletedTask;
        }

        // StopAsync issued in both IHostedService and IHostLifetime interfaces
        // This IS called when the user closes the ConsoleWindow with the windows top right pane "x (close)" icon
        // This IS called when the user hits ctrl-C in the console window
        //  After Ctrl-C and after this method exits, the debugger
        //    shows an unhandled exception: System.OperationCanceledException: 'The operation was canceled.'
        // See also discussion of Stop async in the following attributions.
        // Attribution to  https://stackoverflow.com/questions/51044781/graceful-shutdown-with-generic-host-in-net-core-2-1
        // Attribution to https://stackoverflow.com/questions/52915015/how-to-apply-hostoptions-shutdowntimeout-when-configuring-net-core-generic-host for OperationCanceledException notes

        public Task StopAsync(CancellationToken cancellationToken) {
            this.logger.LogInformation("<GenericHostAsServiceLifetimeLynchpin.StopAsync");
            Log.Debug("in Program.GenericHostAsServiceLifetimeLynchpin.StopAsync: Calling the ServiceBase.Stop() method ");
            Stop();
            this.logger.LogDebug(">{@ Method}", "Program.GenericHostAsServiceLifetimeLynchpin.StopAsync");
            return Task.CompletedTask;
        }
        #endregion

        #region ServiceBase object overriden methods
        // Called by base.Run when the service is ready to start.
        //  base is the ServiceBase this ServerAsSevice descends from.
        //  only 
        protected override void OnStart(string[] args) {
            this.logger.LogInformation("GenericHostAsServiceLifetimeLynchpin.OnStart method called.");
            _delayStart.TrySetResult(null);
            base.OnStart(args);
        }

        // Called by base.Stop. This may be called multiple times by service Stop, ApplicationStopping, and StopAsync.
        // That's OK because StopApplication uses a CancellationTokenSource and prevents any recursion.
        // This IS called when user hits ctrl-C in ConsoleWindow
        //  This IS NOT called when user closes the startup auto browser
        // ToDo:investigate BrowserLink, and multiple browsers effect on this call
        protected override void OnStop() {
            this.logger.LogInformation("GenericHostAsServiceLifetimeLynchpin.OnStop method called.");
            HostApplicationLifetime.StopApplication();
            base.OnStop();
        }

        // All the other ServiceBase's virtual methods could be overridden here as well, to log them
        #endregion

        #region Event Handlers registered with the HostApplicationLifetime events
        // Registered as a handler with the HostApplicationLifetime.ApplicationStarted event
        // HostApplicationLifetime instance is passed to the GenericHostAsServiceLifetimeLynchpin ctor
        private void OnStarted() {
            this.logger.LogInformation("OnStarted event handler called.");
            // Post-startup code goes here  
        }

        // Registered as a handler with the HostApplicationLifetime.Application event
        // HostApplicationLifetime instance is passed to the GenericHostAsServiceLifetimeLynchpin ctor
        // This is NOT called if the ConsoleWindows ends when the connected browser (browser opened by LaunchSettings when starting with debugger)
        //  is closed
        // This IS called if the user hits ctrl-C in the ConsoleWindow
        private void OnStopping() {
            this.logger.LogInformation("OnStopping method called.");
            // On-stopping code goes here  
        }

        // Registered as a handler with the HostApplicationLifetime.ApplicationStarted event
        // HostApplicationLifetime instance is passed to the GenericHostAsServiceLifetimeLynchpin ctor
        private void OnStopped() {
            this.logger.LogInformation("OnStopped method called.");
            // Post-stopped code goes here  
        }
        #endregion

        // Run method with no arguments
        private void Run() {
            this.logger.LogInformation("Injected logger Run method called.");
            Log.Debug("In Program.GenericHostAsServiceLifetimeLynchpin.Run");
            try {
                Log.Debug("in Program.GenericHostAsServiceLifetimeLynchpin.Run, calling Run(this) on the ServiceBase class, expecting it to block until service stoppped");
                Run(this); // This blocks until the service is stopped.
                Log.Debug("in Program.GenericHostAsServiceLifetimeLynchpin.Run, returned from Run(this), something has unblocked it");
                _delayStart.TrySetException(new InvalidOperationException("Stopped without starting"));
            }
            catch (Exception ex) {
                Log.Debug(ex, "in Program.GenericHostAsServiceLifetimeLynchpin.Run, Run(this) threw exception = {Message}", ex.Message);
                _delayStart.TrySetException(ex);
            }
        }
    }

}