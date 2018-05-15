
using ATAP.Utilities.Http;
using ATAP.Utilities.Tcp;
using ServiceStack;
using Swordfish.NET.Collections;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using ServiceStack.Logging;
using System.Linq;
using Funq;
using System.Reflection;
using Ace.AceService.MinerServices.Models;
using ATAP.Utilities.CryptoMiner.Models;

namespace Ace.AceService.MinerServices.Interfaces
{
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
            return new ListMinersResponse { ProcessID = 101010 };
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
