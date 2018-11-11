using System;
using System.Collections.Generic;
using System.Linq;

namespace Ace.Agent.BaseServices {
    public static class DefaultConfiguration {
        public static Dictionary<string, string> Configuration()
        {
            return new Dictionary<string, string>() {
                { "Ace.Agent.BaseServices.Config.ListenOn", "http://localhost:21100/" },
                { "Ace.Agent:ListeningOn", "http://localhost:21100/" },
                {
                    "Ace.Agent.BaseServices.Config.MySqlConnectionString",
                    "Server=localhost;Port=3306;Database=acecommander;Uid=whertzing;Pwd=devDBAdminPwd!"
                },
                {
                    "Ace.Agent.BaseServices.Config.RedisConnectionString",
                    "localhost:6379"
                },
                {
                    "Ace.Agent:UserName",
                    Environment.UserName
                },
                {
                    "Ace.Agent.Plugin.HWPlugin.PathToHWConfigFile",
                    $"{Environment.GetEnvironmentVariable("ProgramData")}+/ACE/AceService.HWPlugin.Config.txt"
                },
                {
                    "Ace.Agent.Plugin.MinerPlugin.PathToMinerConfigFile",
                    $"{Environment.GetEnvironmentVariable("ProgramData")}+/ACE/AceService.MinerPlugin.Config.txt"
                },
                { "Ace.Agent.Plugin.MinerPlugin.MPort", "21200" },
                { "Ace.Agent.Plugin.MinerPlugin.ProcessName", "EthDcrMiner64" },
                {
                    "Ace.Agent.Plugin.MinerPlugin.PathToEXE",
                    @"C:\ProgramData\CryptoCurrency\Ethereum\Claymore's Dual Ethereum+Decred_Siacoin_Lbry_Pascal AMD+NVIDIA GPU Miner v10.2\EthDcrMiner64.exe"
                },
                {
                    "Ace.Agent.Plugin.MinerPlugin.PathToEPools",
                    @"C:\ProgramData\CryptoCurrency\Ethereum\Claymore's Dual Ethereum+Decred_Siacoin_Lbry_Pascal AMD+NVIDIA GPU Miner v10.2\epools.txt"
                },
                {
                    "Ace.Agent.Plugin.MinerPlugin.PathToDPools",
                    @"C:\ProgramData\CryptoCurrency\Ethereum\Claymore's Dual Ethereum+Decred_Siacoin_Lbry_Pascal AMD+NVIDIA GPU Miner v10.2\dpools.txt"
                },
                { "Ace.Agent.Plugin.MinerPlugin.TargetTemperature", "70" },
                { "Ace.Agent.ProcessesToStartOnAceServiceStartup", "placeholder" },
                { "Ace.AceGUI::MainWindow:Height", "400" },
                { "Ace.AceGUI::MainWindow:Width", "600" },
                { "Ace.AceGUI::MainWindow:Top", "0" },
                { "Ace.AceGUI::MainWindow:Left", "0" }
            };
        }
    }
}

/*
 Abstract:
 ComputerInventory
 Moment or span?
    Motherboard
    CPU
    Memory
    Disks
    PowerSupply
    USBPorts
    VideoCards
    Software
        Drivers
        Mining Programs (includes both name and version)
        AceAgent
  
Concrete:
"FactoryReset", AKA AllDummy (moment, 1/1/1980)
"CurrentActual" (span, from start of program (or last change) to now())
"Profile or hypothetical" (span or moment, can include planned time spans)

ConcurrentObservableDictionary<TimePeriod, ComputerInventory> changeHistoryComputerInventory (each has non-overlapping periods, should be in the aggregate a contiguous span).
 
*****
Current actual Inventory
    is there a changeHistoryComputerInventory in the configuration settings, or a ChangeHistoryComputerInventoryFile (or DB)?
        yes - create and load a change history object, make currentActual = latest history
    is there a currentComputerInventory in the configuration settings, or a CurrentComputerInventoryFile (or DB)?
        yes - compare currentActual
    if there is nothing, currentComputerInventory = FactoryReset, and changeHistory = currentComputerInventory
    create foundComputerInventory ( run the take inventory method)
    if foundComputerInventory == currentComputerInventory, done, else currentComputerInventory = foundComputerInventory and add currentComputerInventory to changeHistoryComputerInventory
at this point, the currentComputerInventory object is up to date

 * */
