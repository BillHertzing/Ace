
using ATAP.Utilities.TypedGuids;

using ATAP.Utilities.ComputerHardware.Enumerations;


using System;
using System.Collections.Generic;


namespace ATAP.Utilities.DiskDrive {
    public interface IDiskInfoEx {
        Id<DiskInfoEx> DiskDriveDbId { get; set; }
        DiskDriveMaker DiskDriveMaker { get; set; }
        DiskDriveType DiskDriveType { get; set; }
        int DriveNumber { get; set; }
        IEnumerable<Exception> Exceptions { get; set; }
        IEnumerable<PartitionInfoEx> PartitionInfoExs { get; set; }
        string SerialNumber { get; set; }

        bool Equals(DiskInfoEx other);
        bool Equals(object obj);
        int GetHashCode();
    }

    public class DiskInfoEx : IEquatable<DiskInfoEx>, IDiskInfoEx {
        public DiskInfoEx() {
        }

        public DiskInfoEx(int driveNumber, Id<DiskInfoEx> diskDriveDbId, DiskDriveMaker diskDriveMaker, DiskDriveType diskDriveType, string serialNumber, IEnumerable<PartitionInfoEx> partitionInfoExs, IEnumerable<Exception> exceptions) {
            DriveNumber=driveNumber;
            DiskDriveDbId=diskDriveDbId;
            DiskDriveMaker=diskDriveMaker;
            DiskDriveType=diskDriveType;
            SerialNumber=serialNumber??throw new ArgumentNullException(nameof(serialNumber));
            PartitionInfoExs=partitionInfoExs??throw new ArgumentNullException(nameof(partitionInfoExs));
            Exceptions=exceptions??throw new ArgumentNullException(nameof(exceptions));
        }

        public int DriveNumber { get; set; }
        public Id<DiskInfoEx> DiskDriveDbId { get; set; }
        public DiskDriveMaker DiskDriveMaker { get; set; }
        public DiskDriveType DiskDriveType { get; set; }
        public string SerialNumber { get; set; }
        public IEnumerable<PartitionInfoEx> PartitionInfoExs { get; set; }
        public IEnumerable<Exception> Exceptions { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as DiskInfoEx);
        }

        public bool Equals(DiskInfoEx other) {
            return other!=null&&
                   DriveNumber==other.DriveNumber&&
                   DiskDriveDbId.Equals(other.DiskDriveDbId)&&
                   DiskDriveMaker==other.DiskDriveMaker&&
                   DiskDriveType==other.DiskDriveType&&
                   SerialNumber==other.SerialNumber&&
                   EqualityComparer<IEnumerable<PartitionInfoEx>>.Default.Equals(PartitionInfoExs, other.PartitionInfoExs)&&
                   EqualityComparer<IEnumerable<Exception>>.Default.Equals(Exceptions, other.Exceptions);
        }

        public override int GetHashCode() {
            var hashCode = -2144340839;
            hashCode=hashCode*-1521134295+DriveNumber.GetHashCode();
            hashCode=hashCode*-1521134295+EqualityComparer<Id<DiskInfoEx>>.Default.GetHashCode(DiskDriveDbId);
            hashCode=hashCode*-1521134295+DiskDriveMaker.GetHashCode();
            hashCode=hashCode*-1521134295+DiskDriveType.GetHashCode();
            hashCode=hashCode*-1521134295+EqualityComparer<string>.Default.GetHashCode(SerialNumber);
            hashCode=hashCode*-1521134295+EqualityComparer<IEnumerable<PartitionInfoEx>>.Default.GetHashCode(PartitionInfoExs);
            hashCode=hashCode*-1521134295+EqualityComparer<IEnumerable<Exception>>.Default.GetHashCode(Exceptions);
            return hashCode;
        }

        public static bool operator ==(DiskInfoEx left, DiskInfoEx right) {
            return EqualityComparer<DiskInfoEx>.Default.Equals(left, right);
        }

        public static bool operator !=(DiskInfoEx left, DiskInfoEx right) {
            return !(left==right);
        }
    }

    public class PartitionInfoEx : IEquatable<PartitionInfoEx> {
        public PartitionInfoEx() {
        }

        public PartitionInfoEx(Id<PartitionInfoEx> partitionDbId, IEnumerable<string> driveLetters, IEnumerable<Exception> exceptions, long size, PartitionFileSystem partitionFileSystem) {
            PartitionDbId=partitionDbId;
            DriveLetters=driveLetters??throw new ArgumentNullException(nameof(driveLetters));
            Exceptions=exceptions??throw new ArgumentNullException(nameof(exceptions));
            Size=size;
            PartitionFileSystem=partitionFileSystem;
        }

        public Id<PartitionInfoEx> PartitionDbId { get; set; }
        public IEnumerable<string> DriveLetters { get; set; }
        public IEnumerable<Exception> Exceptions { get; set; }
        public long Size { get; set; }
        public PartitionFileSystem PartitionFileSystem { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as PartitionInfoEx);
        }

        public bool Equals(PartitionInfoEx other) {
            return other!=null&&
                   PartitionDbId.Equals(other.PartitionDbId)&&
                   EqualityComparer<IEnumerable<string>>.Default.Equals(DriveLetters, other.DriveLetters)&&
                   EqualityComparer<IEnumerable<Exception>>.Default.Equals(Exceptions, other.Exceptions)&&
                   Size==other.Size&&
                   PartitionFileSystem==other.PartitionFileSystem;
        }

        public override int GetHashCode() {
            var hashCode = -706038837;
            hashCode=hashCode*-1521134295+EqualityComparer<Id<PartitionInfoEx>>.Default.GetHashCode(PartitionDbId);
            hashCode=hashCode*-1521134295+EqualityComparer<IEnumerable<string>>.Default.GetHashCode(DriveLetters);
            hashCode=hashCode*-1521134295+EqualityComparer<IEnumerable<Exception>>.Default.GetHashCode(Exceptions);
            hashCode=hashCode*-1521134295+Size.GetHashCode();
            hashCode=hashCode*-1521134295+PartitionFileSystem.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(PartitionInfoEx left, PartitionInfoEx right) {
            return EqualityComparer<PartitionInfoEx>.Default.Equals(left, right);
        }

        public static bool operator !=(PartitionInfoEx left, PartitionInfoEx right) {
            return !(left==right);
        }
    }
    public interface IDiskInfoExsContainer {
        List<DiskInfoEx> DiskInfoExs { get; set; }

        bool Equals(DiskInfoExsContainer other);
        bool Equals(object obj);
        int GetHashCode();
    }

    public class DiskInfoExsContainer : IEquatable<DiskInfoExsContainer>, IDiskInfoExsContainer {
        public DiskInfoExsContainer(List<DiskInfoEx> diskInfoExs) {
            DiskInfoExs=diskInfoExs??throw new ArgumentNullException(nameof(diskInfoExs));
        }

        public List<DiskInfoEx> DiskInfoExs { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as DiskInfoExsContainer);
        }

        public bool Equals(DiskInfoExsContainer other) {
            return other!=null&&
                   EqualityComparer<List<DiskInfoEx>>.Default.Equals(DiskInfoExs, other.DiskInfoExs);
        }

        public override int GetHashCode() {
            return 130062218+EqualityComparer<List<DiskInfoEx>>.Default.GetHashCode(DiskInfoExs);
        }

        public static bool operator ==(DiskInfoExsContainer left, DiskInfoExsContainer right) {
            return EqualityComparer<DiskInfoExsContainer>.Default.Equals(left, right);
        }

        public static bool operator !=(DiskInfoExsContainer left, DiskInfoExsContainer right) {
            return !(left==right);
        }
    }
    public interface IWalkDiskDriveResult {
        List<Exception> Exceptions { get; set; }
        long LargestPartition { get; set; }
        int NumberOfPartitions { get; set; }

        bool Equals(object obj);
        bool Equals(WalkDiskDriveResult other);
        int GetHashCode();
    }

    public class WalkDiskDriveResult : IEquatable<WalkDiskDriveResult>, IWalkDiskDriveResult {
        public WalkDiskDriveResult() {
        }

        public WalkDiskDriveResult(int numberOfPartitions, long largestPartition, List<Exception> exceptions) {
            NumberOfPartitions=numberOfPartitions;
            LargestPartition=largestPartition;
            Exceptions=exceptions??throw new ArgumentNullException(nameof(exceptions));
        }

        public int NumberOfPartitions { get; set; }
        public long LargestPartition { get; set; }

        public List<Exception> Exceptions { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as WalkDiskDriveResult);
        }

        public bool Equals(WalkDiskDriveResult other) {
            return other!=null&&
                   NumberOfPartitions==other.NumberOfPartitions&&
                   LargestPartition==other.LargestPartition&&
                   EqualityComparer<List<Exception>>.Default.Equals(Exceptions, other.Exceptions);
        }

        public override int GetHashCode() {
            var hashCode = -244615512;
            hashCode=hashCode*-1521134295+NumberOfPartitions.GetHashCode();
            hashCode=hashCode*-1521134295+LargestPartition.GetHashCode();
            hashCode=hashCode*-1521134295+EqualityComparer<List<Exception>>.Default.GetHashCode(Exceptions);
            return hashCode;
        }

        public static bool operator ==(WalkDiskDriveResult left, WalkDiskDriveResult right) {
            return EqualityComparer<WalkDiskDriveResult>.Default.Equals(left, right);
        }

        public static bool operator !=(WalkDiskDriveResult left, WalkDiskDriveResult right) {
            return !(left==right);
        }
    }

    public interface IWalkDiskDriveResultContainer {
        WalkDiskDriveResult WalkDiskDriveResult { get; set; }

        bool Equals(object obj);
        bool Equals(WalkDiskDriveResultContainer other);
        int GetHashCode();
    }

    public class WalkDiskDriveResultContainer : IEquatable<WalkDiskDriveResultContainer>, IWalkDiskDriveResultContainer {
        public WalkDiskDriveResult WalkDiskDriveResult { get; set; }

        public WalkDiskDriveResultContainer(WalkDiskDriveResult walkDiskDriveResult) {
            WalkDiskDriveResult=walkDiskDriveResult??throw new ArgumentNullException(nameof(walkDiskDriveResult));
        }

        public override bool Equals(object obj) {
            return Equals(obj as WalkDiskDriveResultContainer);
        }

        public bool Equals(WalkDiskDriveResultContainer other) {
            return other!=null&&
                   EqualityComparer<WalkDiskDriveResult>.Default.Equals(WalkDiskDriveResult, other.WalkDiskDriveResult);
        }

        public override int GetHashCode() {
            return -1531662794+EqualityComparer<WalkDiskDriveResult>.Default.GetHashCode(WalkDiskDriveResult);
        }

        public static bool operator ==(WalkDiskDriveResultContainer left, WalkDiskDriveResultContainer right) {
            return EqualityComparer<WalkDiskDriveResultContainer>.Default.Equals(left, right);
        }

        public static bool operator !=(WalkDiskDriveResultContainer left, WalkDiskDriveResultContainer right) {
            return !(left==right);
        }
    }
    public class DiskDrivePartitionIdentifier {
        public DiskDrivePartitionIdentifier() : this(new Dictionary<Id<DiskInfoEx>, Id<PartitionInfoEx>>()) { }
        public DiskDrivePartitionIdentifier(IDictionary<Id<DiskInfoEx>, Id<PartitionInfoEx>> diskDrivePartitionInfoGuids) { DiskDrivePartitionInfoGuids=diskDrivePartitionInfoGuids; }
        public IDictionary<Id<DiskInfoEx>, Id<PartitionInfoEx>> DiskDrivePartitionInfoGuids { get; set; }
    }
}



