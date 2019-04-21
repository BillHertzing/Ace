using System.ComponentModel;

namespace ATAP.Utilities.ComputerHardware.Enumerations {

    public enum GPUMaker {
        //ToDo: Add [LocalizedDescription("Generic", typeof(Resource))]
        [Description("Generic")]
        Generic,
        [Description("AMD")]
        AMD,
        [Description("NVIDEA")]
        NVIDEA
    }

    public enum VideoCardMaker {
        //ToDo: Add [LocalizedDescription("Generic", typeof(Resource))]
        [Description("Generic")]
        Generic,
        [Description("ASUS")]
        ASUS,
        [Description("EVGA")]
        EVGA,
        [Description("MSI")]
        MSI,
        [Description("PowerColor")]
        PowerColor
    }

    public enum MainBoardMaker {
        //ToDo: Add [LocalizedDescription("Generic", typeof(Resource))]
        [Description("Generic")]
        Generic,
        [Description("ASUS")]
        ASUS,
        [Description("MSI")]
        MSI
    }

    public enum CPUSocket {
        //ToDo: Add [LocalizedDescription("Generic", typeof(Resource))]
        [Description("Generic")]
        Generic,
        [Description("LGA 1156")]
        LGA1156,
        [Description("LGA 1136")]
        LGA1136,
        [Description("LGA 1155")]
        LGA1155,
        [Description("LGA 775")]
        LGA775
    }

    public enum CPUMaker {
        //ToDo: Add [LocalizedDescription("Generic", typeof(Resource))]
        [Description("Generic")]
        Generic,
        [Description("Intel")]
        Intel,
        [Description("AMD")]
        AMD
    }

    public enum VideoCardMemoryMaker {
        //ToDo: Add [LocalizedDescription("Generic", typeof(Resource))]
        [Description("Generic")]
        Generic,
        [Description("Elpida")]
        Elpida,
        [Description("Hynix")]
        Hynix,
        [Description("Samsung")]
        Samsung
    }

    public enum DiskDriveMaker {
        //ToDo: Add [LocalizedDescription("Generic", typeof(Resource))]
        [Description("Generic")]
        Generic,
        [Description("Samsung")]
        Samsung,
        [Description("Seagate")]
        Seagate,
        [Description("WesternDigital")]
        WesternDigital,
        [Description("Maxtor")]
        Maxtor,
        [Description("Hitachi")]
        Hitachi
    }
    public enum DiskDriveType {
        //ToDo: Add [LocalizedDescription("Generic", typeof(Resource))]
        [Description("Generic")]
        Generic,
        [Description("SSD")]
        SSD,
        [Description("HDD")]
        HDD
    }

    public enum PartitionFileSystem {
        //ToDo: Add [LocalizedDescription("CRC32", typeof(Resource))]
        [Description("NTFS")]
        NTFS,
        [Description("FAT32")]
        FAT32
    }
}