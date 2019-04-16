using ATAP.Utilities.ComputerInventory.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using Itenso.TimePeriod;
using System.Threading.Tasks;
// ToDo: figure out logging for the ATAP libraires, this is only temporary
using ServiceStack.Logging;
using System.Threading;
using ATAP.Utilities.Database.Enumerations;
using ServiceStack.Text;

namespace ATAP.Utilities.ComputerInventory {


    public class DiskInfoEx {
        public DiskInfoEx() : this(-1, -1, Guid.Empty, DiskDriveMaker.Generic, DiskDriveType.Generic, string.Empty, new List<PartitionInfoEx>(), new List<Exception>()) { }
        public DiskInfoEx(int driveNumber, long diskDriveDBIdentityId, Guid diskDriveGuid, DiskDriveMaker diskDriveMaker, DiskDriveType diskDriveType, string serialNumber, IEnumerable<PartitionInfoEx> partitionInfoExs, IEnumerable<Exception> exceptions) {
            DriveNumber=driveNumber;
            DiskDriveDBIdentityId=diskDriveDBIdentityId;
            DiskDriveGuid=diskDriveGuid;
            DiskDriveMaker=diskDriveMaker;
            DiskDriveType=diskDriveType;
            SerialNumber=serialNumber;
            Exceptions=exceptions;
            PartitionInfoExs=partitionInfoExs;
        }
        public int DriveNumber;
        public long DiskDriveDBIdentityId;
        public Guid DiskDriveGuid;
        public DiskDriveMaker DiskDriveMaker { get; set; }
        public DiskDriveType DiskDriveType { get; set; }
        public string SerialNumber { get; set; }
        public IEnumerable<PartitionInfoEx> PartitionInfoExs;
        public IEnumerable<Exception> Exceptions;
    }

    public class PartitionInfoEx {
        public PartitionInfoEx() : this(-1, Guid.Empty, new List<string>(), new List<Exception>()) { }
        public PartitionInfoEx(long partitionIdentityId, Guid partitionGuid, IEnumerable<string> driveLetters, IEnumerable<Exception> exceptions) {
            PartitionIdentityId=partitionIdentityId;
            PartitionGuid=partitionGuid;
            DriveLetters=driveLetters;
            Exceptions=exceptions;
        }
        public long PartitionIdentityId;
        public Guid PartitionGuid;
        public IEnumerable<string> DriveLetters { get; set; }
        public IEnumerable<Exception> Exceptions;
    }

   
    public class DiskDrivePartitionDriveLetterIdentifier {
        public DiskDrivePartitionDriveLetterIdentifier() : this(new Dictionary<Guid, IDictionary<Guid, string>>()) { }
        public DiskDrivePartitionDriveLetterIdentifier(IDictionary<Guid, IDictionary<Guid, string>> diskDrivePartitionInfoGuidsDriveLetterStrings) { DiskDrivePartitionInfoGuidsDriveLetterStrings=diskDrivePartitionInfoGuidsDriveLetterStrings; }
        public IDictionary<Guid, IDictionary<Guid, string>> DiskDrivePartitionInfoGuidsDriveLetterStrings { get; set; }
    }



    [Serializable]
    public class MainBoard : ISerializable {
        //readonly MainBoardMaker maker;

        //readonly string socket;

        MainBoard(SerializationInfo info, StreamingContext ctxt) {
            ///ToDo: figure out why the static extension method found in utilities.Enumerations doesn't work
            //this.maker = MainBoardMaker.ToEnum<MainBoardMaker>(info.GetString("Maker"));
            Maker=(MainBoardMaker)Enum.Parse(typeof(MainBoardMaker), info.GetString("Maker"), true);
            Socket=info.GetString("Socket");
        }

        public MainBoard(MainBoardMaker maker, string socket) {
            Maker=maker;
            Socket=socket;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("Maker", this.Maker.ToString());
            info.AddValue("Socket", this.Socket);
        }

        public MainBoardMaker Maker { get; set; }

        public string Socket { get; set; }

        public override bool Equals(Object obj) {
            if (obj==null)
                return false;

            MainBoard id = obj as MainBoard;
            if (id==null)
                return false;

            return (Maker==id.Maker);
        }

        public override int GetHashCode() {
            return Maker.GetHashCode();
        }
    }

    [Serializable]
    public class CPU {
        CPUMaker maker;
        //public CPU() : this(CPUMaker.Generic) {}
        public CPU(CPUMaker maker) {
            this.maker=maker;
        }
        public CPUMaker Maker => maker;

        public override bool Equals(Object obj) {
            if (obj==null)
                return false;

            CPU id = obj as CPU;
            if (id==null)
                return false;

            return (maker==id.Maker);
        }

        public override int GetHashCode() {
            return maker.GetHashCode();
        }
    }

    [Serializable]
    //ToDo: figure out how to conditionally compile in the dependency of ComputerHardware on a class library that only supports .Net Framework Full
#if NETFUL
  public class ComputerHardware : OpenHardwareMonitor.Hardware.Computer {
#else
    public class ComputerHardware
#endif
  {
        // A very generic list of computer hardware
        public ComputerHardware(ILog log = null) : this(new MainBoard(MainBoardMaker.Generic, "dummy"),
            new List<CPUMaker>() { CPUMaker.Generic },
            new List<DiskInfoEx>() { new DiskInfoEx() }, new TimeBlock(DateTime.UtcNow, true), log) { }

        // Created on demand to match a specific computerName
        public ComputerHardware(string computerName, ILog log = null) {
            if (!computerName.Trim().ToLowerInvariant().Equals("localhost"))
                throw new NotImplementedException("ComputerName other than localhost is not supported");
            // ToDo: Query WMI or Configuration data for real details
            // Temp: hardcode for laptop
            var diskInfoExs = new List<DiskInfoEx>() { new DiskInfoEx(), new DiskInfoEx(), new DiskInfoEx() };
            new ComputerHardware(new MainBoard(MainBoardMaker.Generic, "dummy"), new List<CPUMaker> { CPUMaker.Intel }, diskInfoExs, new TimeBlock(DateTime.UtcNow, true));
        }

        public ComputerHardware(MainBoard mainboard, IEnumerable<CPUMaker> cPUs, IEnumerable<DiskInfoEx> diskInfoExs, ILog log = null) : this(mainboard, cPUs, diskInfoExs, new TimeBlock(DateTime.UtcNow, true), log) { }

        public ComputerHardware(MainBoard mainboard, IEnumerable<CPUMaker> cPUs, IEnumerable<DiskInfoEx> diskInfoExs, TimeBlock moment, ILog log = null) {
            MainBoard=mainboard??throw new ArgumentNullException(nameof(mainboard));
            CPUs=cPUs??throw new ArgumentNullException(nameof(cPUs));
            DiskInfoExs=diskInfoExs??throw new ArgumentNullException(nameof(diskInfoExs));
            Moment=moment??throw new ArgumentNullException(nameof(moment));
            Log=log;
        }

        /*
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
        #region Properties:class logger
        public ILog Log;
        #endregion
        public MainBoard MainBoard { get; set; }
        public IEnumerable<CPUMaker> CPUs { get; set; }
        public IEnumerable<DiskInfoEx> DiskInfoExs { get; set; }
        public TimeBlock Moment { get; set; }

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

        /*
        public ComputerInventory(ComputerHardware computerHardware, ComputerSoftware computerSoftware, ComputerProcesses computerProcesses) {
            this.computerHardware=computerHardware??throw new ArgumentNullException(nameof(computerHardware));
            this.computerSoftware=computerSoftware??throw new ArgumentNullException(nameof(computerSoftware));
            this.computerProcesses=computerProcesses??throw new ArgumentNullException(nameof(computerProcesses));
        }
        */
        public ComputerInventory() : this(new ComputerHardware()) {
        }
        public ComputerInventory(string computerName, ILog log = null) {
            ComputerName=computerName??throw new ArgumentNullException(nameof(computerName));
            if (!ComputerName.Trim().ToLowerInvariant().Equals("localhost"))
                throw new NotImplementedException("ComputerName other than localhost is not supported");
            ComputerHardware = new ComputerHardware(computerName, log);
        }

        public ComputerInventory(ComputerHardware computerHardware, ILog log = null) {
            Log=log;
            ComputerHardware=computerHardware??throw new ArgumentNullException(nameof(computerHardware));
        }

        // populate a list of PartitionInfoEx with information about the actual partitions on a DiskDrive
        // pass in an Action that will populate a storage location with the information
        public async Task PopulatePartitionInfoExs(CrudType cRUD, DiskInfoEx diskInfoEx, Action<DiskInfoEx> storePartitionInfoExs) {
            Log.Debug($"starting PopulatePartitionInfoExs: cRUD = {cRUD.ToString()}, diskInfoEx = {diskInfoEx.Dump()}");

            // ToDo: Get the list of partitions from the Disk hardware
            await new Task(() => Thread.Sleep(500));
            // Until real partitions are available, mock up the current laptop configuration as of 4/15/19
            // No partitions on drives 0 and 1, and one partition on drive 2, one drive letter E
            var partitionInfoExs = new List<PartitionInfoEx>();
            switch (diskInfoEx.DriveNumber) {
                case 2: {
                    var partitionInfoEx = new PartitionInfoEx() {
                        PartitionIdentityId=0,
                        PartitionGuid=Guid.NewGuid(),
                        DriveLetters=new List<string>() { "E" }
                    };
                    partitionInfoExs.Add(partitionInfoEx);
                    break;
                }
                default:
                    break;
            }
            storePartitionInfoExs.Invoke(diskInfoEx);
            // ToDo: see if the disk already has partitions in the DB
            var dBPartitions = new List<PartitionInfoEx>();

            Log.Debug($"leaving PopulatePartitionInfoExs");
        }

        #region Properties
        #region Properties:class logger
        public ILog Log;
        #endregion
        public string ComputerName { get; set; }
        ComputerHardware ComputerHardware { get; set; }
        //ComputerSoftware ComputerSoftware { get; set; }
        //ComputerProcesses ComputerProcesses { get; set; }
        #endregion

    }

}
