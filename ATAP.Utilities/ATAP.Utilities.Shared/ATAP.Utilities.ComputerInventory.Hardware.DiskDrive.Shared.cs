

using ATAP.Utilities.ComputerHardware.Enumerations;
using ATAP.Utilities.ConcurrentObservableCollections;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using System;
using System.Collections.Generic;

namespace ATAP.Utilities.DiskDrive {


    public interface IDiskDriveInfoEx {
        bool Equals(object obj);
        int GetHashCode();

        Id<DiskDriveInfoEx> DiskDriveDbId { get; set; }
        Id<DiskDriveInfoEx> DiskDriveId { get; set; }
        DiskDriveMaker DiskDriveMaker { get; set; }
        DiskDriveType DiskDriveType { get; set; }
        int? DriveNumber { get; set; }
        IList<Exception> Exceptions { get; set; }
        PartitionInfoExs PartitionInfoExs { get; set; }
        string SerialNumber { get; set; }
    }
    public class DiskDriveInfoEx : IEquatable<DiskDriveInfoEx>, IDiskDriveInfoEx {

        public DiskDriveInfoEx() : this(new Id<DiskDriveInfoEx>(), new Id<DiskDriveInfoEx>(), DiskDriveMaker.Generic, string.Empty, DiskDriveType.Generic, null, new PartitionInfoExs(), new List<Exception>()) { }

        public DiskDriveInfoEx(Id<DiskDriveInfoEx> diskDriveId, Id<DiskDriveInfoEx> diskDriveDbId, DiskDriveMaker diskDriveMaker, string serialNumber, DiskDriveType diskDriveType, int? driveNumber, PartitionInfoExs partitionInfoExs, IList<Exception> exceptions) {

            DiskDriveId=diskDriveId;
            DiskDriveDbId=diskDriveDbId;
            DiskDriveMaker=diskDriveMaker;
            SerialNumber=serialNumber??throw new ArgumentNullException(nameof(serialNumber));
            DiskDriveType=diskDriveType;
            DriveNumber=driveNumber;
            PartitionInfoExs=partitionInfoExs??throw new ArgumentNullException(nameof(partitionInfoExs));
            Exceptions=exceptions??throw new ArgumentNullException(nameof(exceptions));
        }

        public Id<DiskDriveInfoEx> DiskDriveId { get; set; }
        public Id<DiskDriveInfoEx> DiskDriveDbId { get; set; }
        public DiskDriveMaker DiskDriveMaker { get; set; }
        public string SerialNumber { get; set; }
        public DiskDriveType DiskDriveType { get; set; }
        public int? DriveNumber { get; set; }
        public PartitionInfoExs PartitionInfoExs { get; set; }
        public IList<Exception> Exceptions { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as DiskDriveInfoEx);
        }

        public bool Equals(DiskDriveInfoEx other) {
            return other!=null&&
                   DiskDriveId.Equals(other.DiskDriveId)&&
                   DiskDriveDbId.Equals(other.DiskDriveDbId)&&
                   DiskDriveMaker==other.DiskDriveMaker&&
                   SerialNumber==other.SerialNumber&&
                   DiskDriveType==other.DiskDriveType&&
                   EqualityComparer<int?>.Default.Equals(DriveNumber, other.DriveNumber)&&
                   EqualityComparer<PartitionInfoExs>.Default.Equals(PartitionInfoExs, other.PartitionInfoExs)&&
                   EqualityComparer<IList<Exception>>.Default.Equals(Exceptions, other.Exceptions);
        }

        public override int GetHashCode() {
            var hashCode = 1057818730;
            hashCode=hashCode*-1521134295+EqualityComparer<Id<DiskDriveInfoEx>>.Default.GetHashCode(DiskDriveId);
            hashCode=hashCode*-1521134295+EqualityComparer<Id<DiskDriveInfoEx>>.Default.GetHashCode(DiskDriveDbId);
            hashCode=hashCode*-1521134295+DiskDriveMaker.GetHashCode();
            hashCode=hashCode*-1521134295+EqualityComparer<string>.Default.GetHashCode(SerialNumber);
            hashCode=hashCode*-1521134295+DiskDriveType.GetHashCode();
            hashCode=hashCode*-1521134295+EqualityComparer<int?>.Default.GetHashCode(DriveNumber);
            hashCode=hashCode*-1521134295+EqualityComparer<PartitionInfoExs>.Default.GetHashCode(PartitionInfoExs);
            hashCode=hashCode*-1521134295+EqualityComparer<IList<Exception>>.Default.GetHashCode(Exceptions);
            return hashCode;
        }

        public static bool operator ==(DiskDriveInfoEx left, DiskDriveInfoEx right) {
            return EqualityComparer<DiskDriveInfoEx>.Default.Equals(left, right);
        }

        public static bool operator !=(DiskDriveInfoEx left, DiskDriveInfoEx right) {
            return !(left==right);
        }
    }

    public interface IPartitionInfoEx {
        Id<PartitionInfoEx> PartitionId { get; set; }


        bool Equals(object obj);
        int GetHashCode();

        IEnumerable<string> DriveLetters { get; set; }
        IList<Exception> Exceptions { get; set; }
        Id<PartitionInfoEx> PartitionDbId { get; set; }
        PartitionFileSystem PartitionFileSystem { get; set; }
        long? Size { get; set; }
    }
    public class PartitionInfoEx : IEquatable<PartitionInfoEx>, IPartitionInfoEx {
        public PartitionInfoEx() : this(new Id<PartitionInfoEx>(), new Id<PartitionInfoEx>(), new List<String>(), PartitionFileSystem.Generic, new List<Exception>(), null) {
        }

        public PartitionInfoEx(Id<PartitionInfoEx> partitionId, Id<PartitionInfoEx> partitionDbId, IEnumerable<string> driveLetters, PartitionFileSystem partitionFileSystem, IList<Exception> exceptions, long? size) {
            PartitionId=partitionId;
            PartitionDbId=partitionDbId;
            DriveLetters=driveLetters??throw new ArgumentNullException(nameof(driveLetters));
            PartitionFileSystem=partitionFileSystem;
            Exceptions=exceptions??throw new ArgumentNullException(nameof(exceptions));
            Size=size;
        }

        public Id<PartitionInfoEx> PartitionId { get; set; }
        public Id<PartitionInfoEx> PartitionDbId { get; set; }
        public IEnumerable<string> DriveLetters { get; set; }
        public PartitionFileSystem PartitionFileSystem { get; set; }
        public IList<Exception> Exceptions { get; set; }
        public long? Size { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as PartitionInfoEx);
        }

        public bool Equals(PartitionInfoEx other) {
            return other!=null&&
                   PartitionId.Equals(other.PartitionId)&&
                   PartitionDbId.Equals(other.PartitionDbId)&&
                   EqualityComparer<IEnumerable<string>>.Default.Equals(DriveLetters, other.DriveLetters)&&
                   PartitionFileSystem==other.PartitionFileSystem&&
                   EqualityComparer<IList<Exception>>.Default.Equals(Exceptions, other.Exceptions)&&
                   EqualityComparer<long?>.Default.Equals(Size, other.Size);
        }

        public override int GetHashCode() {
            var hashCode = 508343711;
            hashCode=hashCode*-1521134295+EqualityComparer<Id<PartitionInfoEx>>.Default.GetHashCode(PartitionId);
            hashCode=hashCode*-1521134295+EqualityComparer<Id<PartitionInfoEx>>.Default.GetHashCode(PartitionDbId);
            hashCode=hashCode*-1521134295+EqualityComparer<IEnumerable<string>>.Default.GetHashCode(DriveLetters);
            hashCode=hashCode*-1521134295+PartitionFileSystem.GetHashCode();
            hashCode=hashCode*-1521134295+EqualityComparer<IList<Exception>>.Default.GetHashCode(Exceptions);
            hashCode=hashCode*-1521134295+EqualityComparer<long?>.Default.GetHashCode(Size);
            return hashCode;
        }

        public static bool operator ==(PartitionInfoEx left, PartitionInfoEx right) {
            return EqualityComparer<PartitionInfoEx>.Default.Equals(left, right);
        }

        public static bool operator !=(PartitionInfoEx left, PartitionInfoEx right) {
            return !(left==right);
        }
    }

    public interface IPartitionInfoExs {
        ConcurrentObservableDictionary<Id<PartitionInfoEx>, PartitionInfoEx> PartitionInfoExCOD { get; set; }
    }

    public class PartitionInfoExs : IPartitionInfoExs {
        public PartitionInfoExs() : this (new ConcurrentObservableDictionary<Id<PartitionInfoEx>, PartitionInfoEx>()) {}

        public PartitionInfoExs(ConcurrentObservableDictionary<Id<PartitionInfoEx>, PartitionInfoEx> partitionInfoExCOD) {
            PartitionInfoExCOD=partitionInfoExCOD??throw new ArgumentNullException(nameof(partitionInfoExCOD));
        }

        public ConcurrentObservableDictionary<Id<PartitionInfoEx>, PartitionInfoEx> PartitionInfoExCOD { get; set; }
    }

    public interface IDiskDriveInfoExs {
        ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, DiskDriveInfoEx> DiskDriveInfoExCOD { get; set; }
    }

    public class DiskDriveInfoExs : IDiskDriveInfoExs {
        public DiskDriveInfoExs() :this(new ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, DiskDriveInfoEx>()) {}

        public DiskDriveInfoExs(ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, DiskDriveInfoEx> diskDriveInfoExCOD) {
            DiskDriveInfoExCOD=diskDriveInfoExCOD??throw new ArgumentNullException(nameof(diskDriveInfoExCOD));
        }

        public ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, DiskDriveInfoEx> DiskDriveInfoExCOD { get; set; }
    }

    

    public interface IDiskDrivePartitionIdentifier {
        ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, IPartitionInfoExs> DiskDriveInfoPartitionInfoCOD { get; set; }

        bool Equals(DiskDrivePartitionIdentifier other);
        bool Equals(object obj);
        int GetHashCode();
    }

    public class DiskDrivePartitionIdentifier : IEquatable<DiskDrivePartitionIdentifier>, IDiskDrivePartitionIdentifier {
        public DiskDrivePartitionIdentifier() :this (new ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, IPartitionInfoExs>()) {}

        public DiskDrivePartitionIdentifier(ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, IPartitionInfoExs> diskDriveInfoPartitionInfoCOD) {
            DiskDriveInfoPartitionInfoCOD=diskDriveInfoPartitionInfoCOD??throw new ArgumentNullException(nameof(diskDriveInfoPartitionInfoCOD));
        }

        public ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, IPartitionInfoExs> DiskDriveInfoPartitionInfoCOD { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as DiskDrivePartitionIdentifier);
        }

        public bool Equals(DiskDrivePartitionIdentifier other) {
            return other!=null&&
                   EqualityComparer<ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, IPartitionInfoExs>>.Default.Equals(DiskDriveInfoPartitionInfoCOD, other.DiskDriveInfoPartitionInfoCOD);
        }

        public override int GetHashCode() {
            return -1250799812+EqualityComparer<ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, IPartitionInfoExs>>.Default.GetHashCode(DiskDriveInfoPartitionInfoCOD);
        }

        public static bool operator ==(DiskDrivePartitionIdentifier left, DiskDrivePartitionIdentifier right) {
            return EqualityComparer<DiskDrivePartitionIdentifier>.Default.Equals(left, right);
        }

        public static bool operator !=(DiskDrivePartitionIdentifier left, DiskDrivePartitionIdentifier right) {
            return !(left==right);
        }
    }

    public interface IDiskDriveSpecifier {
        bool Equals(DiskDriveSpecifier other);
        bool Equals(object obj);
        int GetHashCode();
    }

    public class DiskDriveSpecifier : IEquatable<DiskDriveSpecifier>, IDiskDriveSpecifier {
        public DiskDriveSpecifier() :this (string.Empty, null) {}

        public DiskDriveSpecifier(string computerName, int? diskDriveNumber) {
            ComputerName=computerName??throw new ArgumentNullException(nameof(computerName));
            DiskDriveNumber=diskDriveNumber;
        }

        public string ComputerName { get; set; }
        public int? DiskDriveNumber { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as DiskDriveSpecifier);
        }

        public bool Equals(DiskDriveSpecifier other) {
            return other!=null&&
                   ComputerName==other.ComputerName&&
                   EqualityComparer<int?>.Default.Equals(DiskDriveNumber, other.DiskDriveNumber);
        }

        public override int GetHashCode() {
            var hashCode = 748249881;
            hashCode=hashCode*-1521134295+EqualityComparer<string>.Default.GetHashCode(ComputerName);
            hashCode=hashCode*-1521134295+EqualityComparer<int?>.Default.GetHashCode(DiskDriveNumber);
            return hashCode;
        }

        public static bool operator ==(DiskDriveSpecifier left, DiskDriveSpecifier right) {
            return EqualityComparer<DiskDriveSpecifier>.Default.Equals(left, right);
        }

        public static bool operator !=(DiskDriveSpecifier left, DiskDriveSpecifier right) {
            return !(left==right);
        }
    }
    

}



