using ATAP.Utilities.CryptoCoin;
using ATAP.Utilities.ComputerInventory;
using ATAP.Utilities.Http;
using ATAP.Utilities.Tcp;
using ServiceStack;
using Swordfish.NET.Collections;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using ServiceStack.Logging;
using OpenHardwareMonitor;
using System.Linq;
using Funq;
using System.Reflection;
using OpenHardwareMonitor.Hardware;

namespace Ace.AceService.MinerServicePlugin
{


    public class MinerServicePluginData
    {
        ConcurrentObservableDictionary<(MinerSWE minerSWE, string version, Coin[] coins), MinerSW> minerSWs;
        ConcurrentObservableDictionary<int, MinerGPU> minerGPUs ;

        public MinerServicePluginData() : this(new ConcurrentObservableDictionary<(MinerSWE minerSWE, string version, Coin[] coins), MinerSW>(), new ConcurrentObservableDictionary<int, MinerGPU>()) { }

        public MinerServicePluginData(ConcurrentObservableDictionary<(MinerSWE minerSWE, string version, Coin[] coins), MinerSW> minerSWs, ConcurrentObservableDictionary<int, MinerGPU> minerGPUs)
        {
            this.minerSWs = minerSWs;
            this.minerGPUs = minerGPUs;
        }

        //ToDo: constructors with event handlers

        public ConcurrentObservableDictionary<(MinerSWE minerSWE, string version, Coin[] coins), MinerSW> MinerSWs { get => minerSWs;}
        public ConcurrentObservableDictionary<int, MinerGPU> MinerGPUs { get => minerGPUs; }
    }

    public class MinerServicePlugin : IPlugin
    {
        /// <summary>
        /// Register this plugin with the appHost
        /// setup any necessary objects and intercepts
        /// </summary>
        /// <param name="appHost">The hosting provider</param>
        public void Register(IAppHost appHost)
        {
            if (null == appHost)
                throw new ArgumentNullException("appHost");

            appHost.RegisterService<MinerServices>();

            var container = appHost.GetContainer();

            // Create the plugIn's observable data structures based on the configuration settings, and the 
            // current computer inventory
            // Get the latest known current configuration, and use that information to populate the data structures
            // appHost.AppSettings.Get<string>("Ace.AceService.MinerPlugin.InstalledMinerSW");
            // appHost.AppSettings.Get<string>("Ace.AceService.MinerPlugin.InstalledGPUs");

            var mspd = new MinerServicePluginData();

            // if the current mining rig configuration specifies that the mining rig has mining-specific data structures that
            //  can and should be monitored, attach the event handlers that will respond to changes in the monitored data structures

            // setup and enable the mechanisms that monitors each mining-specific data sensor, and start them running


            // pass the plugIn's observable data structures and event handlers to the container so they will be available to every other module and services
            container.Register<MinerServicePluginData>(d => mspd);

            //ComputerInventory computerInventory = container.TryResolve(typeof(ComputerInventory)) as ComputerInventory;
            //var x = computerInventory.ComputerHardware.Computer.Hardware.Length;
            // get this computers current power consumption from the sensors package
            //PowerConsumption pc = new PowerConsumption() { Period = new TimeSpan(0, 1, 0), Watts = 1000.0 };
            // get this computers CPU temp and fan structure from the sensors

           // TempAndFan tf = new TempAndFan { Temp = 50, FanPct = 95.5 };
           // rigConfig = RigConfigBuilder.CreateNew()
            //    .AddMinerSWs(mspd.MinerSWs)
            //    .AddMinerGPUs(mspd.MinerGPUs)
            //    .AddPowerConsumption(pc)
            //    .AddTempAndFan(tf)
           //     .Build();
        }
    //    RigConfig rigConfig;
    //    public RigConfig RigConfig { get => rigConfig; }

    }


    [Route("/StartMiner")]
    [Route("/StartMiner/{ID}")]
    public class StartMinerRequest : IReturn<StartMinerResponse>
    {
        public string ID { get; set; }
    }
    public class StartMinerResponse
    {
        public string Result { get; set; }
    }
    [Route("/StopMiner")]
    [Route("/StopMiner/{ID}")]
    public class StopMinerRequest : IReturn<StopMinerResponse>
    {
        public string ProcessName { get; set; }
        public string ID { get; set; }
    }
    public class StopMinerResponse
    {
        public string Result { get; set; }
    }
    [Route("/ListMiner")]
    [Route("/ListMiner/{Kind};{Version}")]
    public class ListMinerRequest : IReturn<ListMinersResponse>
    {
        public string ID { get; set; }
    }
    public class ListMinersResponse
    {
        public string Result { get; set; }
    }

    [Route("/StatusMiners")]
    [Route("/StatusMiner/{ID}")]
    public class StatusMinerRequest : IReturn<StatusMinersResponse>
    {
        public static ILog Log = LogManager.GetLogger(typeof(StatusMinerRequest));

        public string ID { get; set; }
    }
    public class StatusMinersResponse
    {
        public static ILog Log = LogManager.GetLogger(typeof(StatusMinersResponse));

        public MinerStatus Result { get; set; }
    }

    [Route("/TuneMinerGPU")]
    [Route("/TuneMinerGPU/{ID}")]
    public class TuneMinerGPURequest : IReturn<TuneMinerGPUResponse>
    {
        public static ILog Log = LogManager.GetLogger(typeof(TuneMinerGPURequest));

        public string ID { get; set; }
    }
    public class TuneMinerGPUResponse
    {
        public static ILog Log = LogManager.GetLogger(typeof(TuneMinerGPUResponse));

        public TuneMinerGPUsResult[] Result { get; set; }
    }



    public class MinerServices : Service
    {
        public static ILog Log = LogManager.GetLogger(typeof(MinerServices));

        public object Any(StartMinerRequest request)
        {
            Log.Debug("starting Any StartMiner request");
            var minerID = request.ID;
            //ToDo Get miner process details
            return new StartMinerResponse { Result = minerID.ToString() };
        }
        public object Any(StopMinerRequest request)
        {
            Log.Debug("starting Any StopMiner request");
            var processName = request.ProcessName;
            Process[] localByName = Process.GetProcessesByName(processName);
            // ToDo get an index from the Request, and/or loop over all if the index is not provided

            int i = 0;
            Process p = localByName[i];
            var pName = p.ProcessName;
            //ToDo - more validation to ensure this is a miner process, and it is in a state that will stop cleanly, etc.
            //ToDo return an error if this process was not started under the control of Ace, and create another route that will support arbitrary process kills (with lots of attack surface checks)
            p.Kill();
            return new StopMinerResponse { Result = $"process {pName} stopped" };
        }
        public object Any(ListMinerRequest request)
        {
            Log.Debug("starting Any ListMiners request");
            //ToDo
            return new ListMinersResponse { Result = "RigConfig" };
        }

        //ToDo need a cancellable method here, too
        public object Any(StatusMinerRequest request)
        {
            Log.Debug("starting Any StatusMiners request");
            //ToDo
            return new StatusMinersResponse { Result = new ClaymoreMinerStatus("ToDo:create a real miner status using a Formatter?")};
        }

        public object Any(TuneMinerGPURequest request)
        {
            TuneMinerGPUsResult[] tuneMinerGPUsResult;
            Log.Debug("starting Any TuneGPUs request");
            //ToDo asking for all GPUs, or a set of specific GPUs?
            //ToDo asking for all MinerSWs, or a set of specific MinerSWs?
            //ToDo asking for fine or coarse tuning adjustments
            tuneMinerGPUsResult = default;
            return tuneMinerGPUsResult;
        }

    }



}
