using ServiceStack;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Serilog;
using System.Linq;
using Funq;
using System.Reflection;
//using ATAP.Utilities.CryptoMiner.Models;

namespace Ace.Plugin.MinerServices
{
    public class MinerServices : Service
    {
        //public static ILog Log = LogManager.GetLogger(typeof(MinerServices));

        public object Any(StartMinerRequest request)
        {
            var minerID = request.ID;
            //ToDo Get miner process details
            return new StartMinerResponse { Result = minerID.ToString() };
        }
        public object Any(StopMinerRequest request)
        {
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
            //ToDo
            return new ListMinersResponse { ProcessID = 101010 };
        }
/*
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
            //ToDo asking for all GPUs, or a set of specific GPUs?
            //ToDo asking for all MinerSWs, or a set of specific MinerSWs?
            //ToDo asking for fine or coarse tuning adjustments
            tuneMinerGPUsResult = default;
            return tuneMinerGPUsResult;
        }
*/
    }



}
