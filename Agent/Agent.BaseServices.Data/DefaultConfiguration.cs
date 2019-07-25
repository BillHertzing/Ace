using System;
using System.Collections.Generic;
using System.Linq;

namespace Ace.Agent.BaseServices {
    public static class DefaultConfiguration {
        public static Dictionary<string, string> Production =
            new Dictionary<string, string>() {
                {
                    "Ace.Agent.BaseServices.Config.RedisConnectionString",
                    "localhost:6379"
                },
                {
                    "Ace.Agent.BaseServices.Config.MySqlConnectionString",
                    "Server=localhost;Port=3306;Database=acecommander;Uid=whertzing;Pwd=ReplaceablePasswordPattern"
                },
                {
                    "Ace.Agent.BaseServices.Config.MSSQLConnectionString",
                    "localhost:6339TBD"
                },
                {
                    "Ace.Agent.BaseServices.Config.RabbitMQConnectionString",
                    "localhost:TBD"
                },

                { "Ace.Agent.ProcessesToStartOnAceServiceStartup", "placeholder" },
                { "Ace.AceGUI::MainWindow:Height", "400" },
                { "Ace.AceGUI::MainWindow:Width", "600" },
                { "Ace.AceGUI::MainWindow:Top", "0" },
                { "Ace.AceGUI::MainWindow:Left", "0" }
            };
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
