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

namespace Ace.AceService.MinerServicePlugin
{
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

            // Create the plugIn's observable data structures, pass them in by IoC
            ConcurrentObservableDictionary<(MinerSWE minerSWE, string version, Coin[] coins), MinerSW> minerSWs = new ConcurrentObservableDictionary<(MinerSWE minerSWE, string version, Coin[] coins), MinerSW> ();

            ConcurrentObservableDictionary<int, MinerGPU> minerGPUs = new ConcurrentObservableDictionary<int, MinerGPU>();
            // Get the latest known current configuration, and use that information to populate the data structures
            // appHost.AppSettings.Get<string>("Ace.AceService.MinerPlugin.InstalledMinerSW");
            // appHost.AppSettings.Get<string>("Ace.AceService.MinerPlugin.InstalledGPUs");
            // get this computers current power consumption from the sensors package
            PowerConsumption pc = new PowerConsumption() { Period = new TimeSpan(0, 1, 0), Watts = 1000.0 };
            // get this computers CPU temp and fan structure from the sensors
            TempAndFan tf = new TempAndFan { Temp = 50, FanPct = 95.5 };
            rigConfig = RigConfigBuilder.CreateNew()
                .AddMinerSWs(minerSWs)
                .AddMinerGPUs(minerGPUs)
                .AddPowerConsumption(pc)
                .AddTempAndFan(tf)
                .Build();
        }
        RigConfig rigConfig;
        public RigConfig RigConfig { get => rigConfig; }

    }


    [Route("/StartMiner")]
    [Route("/StartMiner/{ID}")]
    public class StartMiner : IReturn<StartMinerResponse>
    {
        public string ID { get; set; }
    }
    public class StartMinerResponse
    {
        public string Result { get; set; }
    }
    [Route("/StopMiner")]
    [Route("/StopMiner/{ID}")]
    public class StopMiner : IReturn<StopMinerResponse>
    {
        public string ProcessName { get; set; }
        public string ID { get; set; }
    }
    public class StopMinerResponse
    {
        public string Result { get; set; }
    }
    [Route("/ListMiners")]
    [Route("/ListMiner/{Kind};{Version}")]
    public class ListMiners : IReturn<ListMinersResponse>
    {
        public string ID { get; set; }
    }
    public class ListMinersResponse
    {
        public string Result { get; set; }
    }

    [Route("/StatusMiners")]
    [Route("/StatusMiner/{ID}")]
    public class StatusMiners : IReturn<StatusMinersResponse>
    {
        public static ILog Log = LogManager.GetLogger(typeof(StatusMiners));

        public string ID { get; set; }
    }
    public class StatusMinersResponse
    {
        public static ILog Log = LogManager.GetLogger(typeof(StatusMinersResponse));

        public ClaymoreMinerStatus Result { get; set; }
    }

    [Route("/TuneMinerGPUs")]
    [Route("/TuneMinerGPU/{ID}")]
    public class TuneMinerGPUs : IReturn<TuneMinerGPUsResponse>
    {
        public static ILog Log = LogManager.GetLogger(typeof(TuneMinerGPUs));

        public string ID { get; set; }
    }
    public class TuneMinerGPUsResponse
    {
        public static ILog Log = LogManager.GetLogger(typeof(TuneMinerGPUsResponse));

        public TuneMinerGPUsResult[] Result { get; set; }
    }



    public class MinerServices : Service
    {
        public static ILog Log = LogManager.GetLogger(typeof(MinerServices));

        public object Any(StartMiner request)
        {
            Log.Debug("starting Any StartMiner request");
            var minerID = request.ID;
            //ToDo Get miner process details
            return new StartMinerResponse { Result = minerID.ToString() };
        }
        public object Any(StopMiner request)
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
        public object Any(ListMiners request)
        {
            Log.Debug("starting Any ListMiners request");
            //ToDo
            return new ListMinersResponse { Result = "RigConfig" };
        }

        //ToDo need a cancellable method here, too
        public object Any(StatusMiners request)
        {
            Log.Debug("starting Any StatusMiners request");
            var DUalStr = "{\"id\": 0, \"result\": [\"10.2 - ETH\", \"4258\", \"50033;1249;0\", \"24583;25450\", \"1501011;2571;0\", \"737502;763509\", \"68;100;81;100\", \"eth-us-east1.nanopool.org:9999;sia-us-east1.nanopool.org:7777\", \"0;2;0;2\"], \"error\": null}";
            var msorigianlZEC = "{\"id\": 0, \"error\": null, \"result\": [\"12.6 - ZEC\", \"1676\", \"352; 1300; 4\", \"175; 177\", \"0; 0; 0\", \"off; off\", \"81; 100\", \"zec - us - east1.nanopool.org:6633\", \"0; 2; 0; 0\"]}";
            var ms = "{\"id\": 0, \"error\": null, \"result\": [\"12.6 - ZEC\", \"1676\", \"352; 1300; 4\", \"175; 177\", \"0; 0; 0\", \"off; off\", \"81; 100\", \"zec - us - east1.nanopool.org:6633\", \"0; 2; 0; 0\"]}";
            var host = "ncat040";
            var port = 21200;
            var message = "{\"id\":0,\"jsonrpc\":\"2.0\",\"method\":\"miner_getstat1\"}";
            byte[] responsebuffer = new byte[Tcp.defaultMaxResponseBufferSize];
            // ToDo figure out what to do about exceptions and policies  let exceptions bubble up?
            // If there is no process listening on the port, there will be an exception
            //ToDo add a cancellation token
            responsebuffer = Tcp.FetchAsync(host, port, message).Result;
            string str = Encoding.ASCII.GetString(responsebuffer);
            return new StatusMinersResponse { Result = new ClaymoreMinerStatus(str) };
        }

        public object Any(TuneMinerGPUs request)
        {
            TuneMinerGPUsResult[] tuneMinerGPUsResult;
            Log.Debug("starting Any TuneGPUs request");
            //ToDo asking for all GPUs, or a set of specific GPUs?
            //ToDo asking for all MinerSWs, or a set of specific MinerSWs?
            //ToDo asking for fine or coarse tuning adjustments
            bool fine = true;
            //ToDo asking for highest HashRate or most efficient HashRate
            // create the collection of GPUs to tune
            MinerGPU[] minerGPUsToTune = new MinerGPU[1];
            // create the collection of MinerSWs to tune
            MinerSW[] minerSWsToTune = new MinerSW[1];
            foreach (var msw in minerSWsToTune)
            {
                foreach (var mg in minerGPUsToTune)
                {
                    // Select the tuning strategy for this MinerSW and this VideoCard
                    var vcdc = mg.VideoCardDiscriminatingCharacteristics;
                    var vctp = KnownVideoCards.TuningParameters[vcdc];
                    // Calculate the step for each parameter
                    int memoryClockStep = (vctp.MemoryClockMax - vctp.MemoryClockMin) / (fine ? 1 : 5);
                    int coreClockStep = (vctp.CoreClockMax - vctp.CoreClockMin) / (fine ? 1 : 5);
                    double voltageStep = (vctp.VoltageMax - vctp.VoltageMin) / (fine ? 0.01 : 0.05);
                    // memoryClock Min, max, step
                    // CoreClock Min, max, step
                    // memoryVoltage min, max, step
                    int memoryClockTune = vctp.MemoryClockMin;
                    int coreClockTune = vctp.CoreClockMin;
                    double voltageTune = vctp.VoltageMin;
                    // initialize the structures that monitor for miner SW stopping, or Rig rebooting
                    while (voltageTune <= vctp.VoltageMax)
                    {
                        while (coreClockTune <= vctp.CoreClockMax)
                        {
                            while (memoryClockTune <= vctp.MemoryClockMax)
                            {
                                // create the tuning configuration settings for this MinerSW and this VideoCard
                                //MinerGPUTuningconfig minerGPUTuningconfig;

                                // Stop the miner software
                                //msw.Stop();
                                // update the MinerSW configuration
                                //msw.SetConfig(minerGPUTuningconfig);
                                // write the MinerSW Configuration to the miner's configuration file
                                // msw.SaveConfig
                                // update the structures that monitor for miner SW stopping, or Rig rebooting
                                // Start the miner MinerSW
                                //msw.Start();
                                // Wait a Delay for the card to settle
                                // Get the current HashRate and power consumption
                                // Or Detect a minerSW stoppage or detect a rig reboot
                                // Record the results for this combination of msw,mvc,mClock,cClock,and mVoltge
                                memoryClockTune += memoryClockStep;
                                memoryClockTune = memoryClockTune > vctp.MemoryClockMax ? vctp.MemoryClockMax : memoryClockTune;
                            }
                            coreClockTune += coreClockStep;
                            coreClockTune = coreClockTune > vctp.CoreClockMax ? vctp.CoreClockMax : coreClockTune;
                        }
                        voltageTune += voltageStep;
                        voltageTune = voltageTune > vctp.VoltageMax ? vctp.VoltageMax : voltageTune;
                    }
                }
            }
            tuneMinerGPUsResult = default;
            return tuneMinerGPUsResult;
        }

    }



}
