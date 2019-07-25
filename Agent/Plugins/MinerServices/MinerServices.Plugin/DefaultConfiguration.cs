using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Plugin.MinerServices {
    static class DefaultConfiguration {
        public static Dictionary<string, string> Production => new Dictionary<string, string>() {
                {
                    "Ace.Agent.Plugin.HWPlugin.PathToHWConfigFile",
                    $"{Environment.GetEnvironmentVariable("ProgramData")}+/ACE/AceService.HWPlugin.Config.txt // replace with "
                },
                {
                    "Ace.Plugin.MinerServices.Config.PathToMinerConfigFile",
                    $"{Environment.GetEnvironmentVariable("ProgramData")}+/ACE/AceService.MinerPlugin.Config.txt"
                },
                { "Ace.Plugin.MinerServices.Config.MPort", "21200" },
                { "Ace.Plugin.MinerServices.Config.ProcessName", "EthDcrMiner64" },
                {
                    "Ace.Plugin.MinerServices.Config.PathToEXE",
                    @"C:\ProgramData\CryptoCurrency\Ethereum\Claymore's Dual Ethereum+Decred_Siacoin_Lbry_Pascal AMD+NVIDIA GPU Miner v10.2\EthDcrMiner64.exe"
                },
                {
                    "Ace.Plugin.MinerServices.Config.PathToEPools",
                    @"C:\ProgramData\CryptoCurrency\Ethereum\Claymore's Dual Ethereum+Decred_Siacoin_Lbry_Pascal AMD+NVIDIA GPU Miner v10.2\epools.txt"
                },
                {
                    "Ace.Plugin.MinerServices.Config.PathToDPools",
                    @"C:\ProgramData\CryptoCurrency\Ethereum\Claymore's Dual Ethereum+Decred_Siacoin_Lbry_Pascal AMD+NVIDIA GPU Miner v10.2\dpools.txt"
                },
                { "Ace.Plugin.MinerServices.Config.TargetTemperature", "70" },
    };
    }
}
