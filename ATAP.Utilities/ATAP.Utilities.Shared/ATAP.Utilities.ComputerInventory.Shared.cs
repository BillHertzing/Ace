using ATAP.Utilities.ComputerInventory.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using Itenso.TimePeriod;
using System.Threading.Tasks;
using System.Threading;
using ATAP.Utilities.ComputerInventory.Enumerations;
using ATAP.Utilities.DiskDrive;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.ConcurrentObservableCollections;

namespace ATAP.Utilities.ComputerInventory {


    // ToDo: remove this stub and integrate a ComputerSoftware type for the GUI (non-observable)
    [Serializable]
    public class ComputerSoftware {
        // ToDo implement OS when the bug in dot net core is fixed. this type cannot be serialized by newtonSoft in dot net core v2



        public string Placeholder;

        public ComputerSoftware() : this("NotImplemented") {
        }

        public ComputerSoftware(string placeholder) {
            Placeholder=placeholder??throw new ArgumentNullException(nameof(placeholder));
        }
        //public OperatingSystem OS { get; set; }

        //public List<ComputerSoftwareDriver> ComputerSoftwareDrivers { get; set; }

        //public List<ComputerSoftwareProgram> ComputerSoftwarePrograms { get; set; }
    }
    // ToDo: remove this stub and integrate a ComputerSoftware type for the GUI (non-observable)

    public class ComputerProcesses {
        // ToDo implement OS when the bug in dot net core is fixed. this type cannot be serialized by newtonSoft in dot net core v2



        public string Placeholder;

        public ComputerProcesses() : this("NotImplemented") {
        }

        public ComputerProcesses(string placeholder) {
            Placeholder=placeholder??throw new ArgumentNullException(nameof(placeholder));
        }

    }

    [Serializable]
    //ToDo: figure out how to conditionally compile in the dependency of ComputerHardware on a class library that only supports .Net Framework Full
#if NETFUL
  public class ComputerHardware : OpenHardwareMonitor.Hardware.Computer {
#else
    public class ComputerHardware : IDiskDriveInfoExs, IEquatable<ComputerHardware> {
#endif  
        // A very generic list of computer hardware with no disk drives
        public ComputerHardware() : this(new MainBoard(MainBoardMaker.Generic, CPUSocket.Generic),
            new List<CPUMaker>() { CPUMaker.Generic },
            new ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, DiskDriveInfoEx>() { { new Id<DiskDriveInfoEx>(), new DiskDriveInfoEx()} },
            new TimeBlock(DateTime.UtcNow, true)) { }
    
        public ComputerHardware(MainBoard mainboard, IList<CPUMaker> cPUs, IDiskDriveInfoExs diskDriveInfoExs) : this(mainboard, cPUs, diskDriveInfoExs.DiskDriveInfoExCOD, new TimeBlock(DateTime.UtcNow, true)) { }

        public ComputerHardware(MainBoard mainBoard, IList<CPUMaker> cPUs, ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, DiskDriveInfoEx> diskDriveInfoExCOD, TimeBlock moment) {
            MainBoard=mainBoard??throw new ArgumentNullException(nameof(mainBoard));
            CPUs=cPUs??throw new ArgumentNullException(nameof(cPUs));
            DiskDriveInfoExCOD=diskDriveInfoExCOD??throw new ArgumentNullException(nameof(diskDriveInfoExCOD));
            Moment=moment??throw new ArgumentNullException(nameof(moment));
        }


        /*
        // ToDo: Investigate the following, it is from old code that uses a .NetFull HW library to query info about a physical computer 
        public ComputerHardware(CPU[] cPUs, MainBoard mainBoard, VideoCard[] videoCards, TimeBlock moment) {
                isMainboardEnabled=true;
                isCPUsEnabled=true;
                isVideoCardsEnabled=true;
                isFanControllerEnabled=true;
                this.cPUs=cPUs;
                this.mainBoard=mainBoard;
                this.videoCards=videoCards;
                this.moment=moment;
        #if NETFUL
                this.OpenComputer();
        #endif
            }
            
            public ComputerHardware(CPU[] cPUs, MainBoard mainBoard, VideoCard[] videoCards) {
                isMainboardEnabled=true;
                isCPUsEnabled=true;
                isVideoCardsEnabled=true;
                isFanControllerEnabled=true;
                this.cPUs=cPUs;
                this.mainBoard=mainBoard;
                this.videoCards=videoCards;
                this.moment=new TimeBlock(DateTime.UtcNow, true);
        #if NETFUL
                this.computer = new Computer
                {
                    MainboardEnabled = isMainboardEnabled,
                    CPUEnabled = isCPUsEnabled,
                    FanControllerEnabled = isFanControllerEnabled,
                    GPUEnabled = isVideoCardsEnabled
                };
                // ToDo: Get teh HardwareMonitorLib to work, right now, it throws an exception it can't find system.management dll
                // computer.Open();
        #endif
            }
        */

        #region Properties
        public MainBoard MainBoard { get; set; }
        public IList<CPUMaker> CPUs { get; set; }
        public ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, DiskDriveInfoEx> DiskDriveInfoExCOD { get; set; }
        public TimeBlock Moment { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as ComputerHardware);
        }

        public bool Equals(ComputerHardware other) {
            return other!=null&&
                   EqualityComparer<MainBoard>.Default.Equals(MainBoard, other.MainBoard)&&
                   EqualityComparer<IList<CPUMaker>>.Default.Equals(CPUs, other.CPUs)&&
                   EqualityComparer<ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, DiskDriveInfoEx>>.Default.Equals(DiskDriveInfoExCOD, other.DiskDriveInfoExCOD)&&
                   EqualityComparer<TimeBlock>.Default.Equals(Moment, other.Moment);
        }


        public override int GetHashCode() {
            var hashCode = 1924731101;
            hashCode=hashCode*-1521134295+EqualityComparer<MainBoard>.Default.GetHashCode(MainBoard);
            hashCode=hashCode*-1521134295+EqualityComparer<IList<CPUMaker>>.Default.GetHashCode(CPUs);
            hashCode=hashCode*-1521134295+EqualityComparer<ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, DiskDriveInfoEx>>.Default.GetHashCode(DiskDriveInfoExCOD);
            hashCode=hashCode*-1521134295+EqualityComparer<TimeBlock>.Default.GetHashCode(Moment);
            return hashCode;
        }

        public static bool operator ==(ComputerHardware left, ComputerHardware right) {
            return EqualityComparer<ComputerHardware>.Default.Equals(left, right);
        }

        public static bool operator !=(ComputerHardware left, ComputerHardware right) {
            return !(left==right);
        }

        /*
        #if NETFUL
                readonly OpenHardwareMonitor.Hardware.Computer computer;
        #endif
        #if NETFULL
            //public Computer Computer => computer;
        #endif
                public bool IsCPUsEnabled { get; set; }
                readonly bool isCPUsEnabled;
                readonly bool isFanControllerEnabled;
                readonly bool isMainboardEnabled;
                readonly bool isVideoCardsEnabled;
                readonly MainBoard mainBoard;
                readonly VideoCard[] videoCards;

                // ToDo: Add field and property for MainBoardMemory
                // ToDo: Add field and property for Disks
                // ToDo: Add field and property for PowerSupply
                // ToDo: Add field and property for USBPorts
                public CPU[] CPUs => cPUs;

                public bool IsCPUsEnabled => isCPUsEnabled;

                public bool IsFanControllerEnabled => isFanControllerEnabled;

                public bool IsMainboardEnabled => isMainboardEnabled;

                public bool IsVideoCardsEnabled => isVideoCardsEnabled;

                public MainBoard MainBoard => mainBoard;

                public TimeBlock Moment {
                    get => moment; set => moment=value;
                }

                public VideoCard[] VideoCards => videoCards;
        */
        #endregion
    }

    public class ComputerInventory {

        public ComputerInventory() : this(new ComputerHardware(), new ComputerSoftware(), new ComputerProcesses(), "generic") { }

        public ComputerInventory(ComputerHardware computerHardware, ComputerSoftware computerSoftware, ComputerProcesses computerProcesses, string computerName) {
            ComputerHardware=computerHardware??throw new ArgumentNullException(nameof(computerHardware));
            ComputerSoftware=computerSoftware??throw new ArgumentNullException(nameof(computerSoftware));
            ComputerProcesses=computerProcesses??throw new ArgumentNullException(nameof(computerProcesses));
            ComputerName=computerName??throw new ArgumentNullException(nameof(computerName));
        }


        #region Properties

        public ComputerHardware ComputerHardware { get; set; }

        public ComputerSoftware ComputerSoftware { get; set; }

        public ComputerProcesses ComputerProcesses { get; set; }

        public string ComputerName { get; set; }

        #endregion

    }

}
