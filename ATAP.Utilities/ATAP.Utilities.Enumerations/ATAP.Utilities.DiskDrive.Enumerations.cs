using System.ComponentModel;

namespace ATAP.Utilities.DiskDrive.Enumerations {
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


