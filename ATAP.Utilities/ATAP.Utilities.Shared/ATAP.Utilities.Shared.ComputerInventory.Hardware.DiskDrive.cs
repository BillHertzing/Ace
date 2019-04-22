

using ATAP.Utilities.ComputerHardware.Enumerations;

using ATAP.Utilities.TypedGuids;
using Swordfish.NET.Collections;
using System;
using System.Collections.Generic;

namespace ATAP.Utilities.DiskDrive {
    public interface IDiskDriveInfoEx {
        Id<DiskDriveInfoEx> DiskDriveDbId { get; set; }
        Id<DiskDriveInfoEx> DiskDriveId { get; set; }
        DiskDriveMaker DiskDriveMaker { get; set; }
        DiskDriveType DiskDriveType { get; set; }
        int DriveNumber { get; set; }
        IEnumerable<Exception> Exceptions { get; set; }
        PartitionInfoExs PartitionInfoExs { get; set; }
        string SerialNumber { get; set; }

        bool Equals(DiskDriveInfoEx other);
        bool Equals(object obj);
        int GetHashCode();
    }

    public class DiskDriveInfoEx : IEquatable<DiskDriveInfoEx>, IDiskDriveInfoEx {

        public DiskDriveInfoEx() {
        }
        public DiskDriveInfoEx(int driveNumber, Id<DiskDriveInfoEx> diskDriveId, Id<DiskDriveInfoEx> diskDriveDbId, DiskDriveMaker diskDriveMaker, DiskDriveType diskDriveType, string serialNumber, PartitionInfoExs partitionInfoExs, IEnumerable<Exception> exceptions) {
            DriveNumber=driveNumber;
            DiskDriveId=diskDriveId;
            DiskDriveDbId=diskDriveDbId;
            DiskDriveMaker=diskDriveMaker;
            DiskDriveType=diskDriveType;
            SerialNumber=serialNumber??throw new ArgumentNullException(nameof(serialNumber));
            PartitionInfoExs=partitionInfoExs??throw new ArgumentNullException(nameof(partitionInfoExs));
            Exceptions=exceptions??throw new ArgumentNullException(nameof(exceptions));
        }

        public static bool operator !=(DiskDriveInfoEx left, DiskDriveInfoEx right) {
            return !(left==right);
        }
        public static bool operator ==(DiskDriveInfoEx left, DiskDriveInfoEx right) {
            return EqualityComparer<DiskDriveInfoEx>.Default.Equals(left, right);
        }

        public override bool Equals(object obj) {
            return Equals(obj as DiskDriveInfoEx);
        }
        public bool Equals(DiskDriveInfoEx other) {
            return other!=null&&
                   DriveNumber==other.DriveNumber&&
                   DiskDriveDbId.Equals(other.DiskDriveDbId)&&
                   DiskDriveMaker==other.DiskDriveMaker&&
                   DiskDriveType==other.DiskDriveType&&
                   SerialNumber==other.SerialNumber&&
                   EqualityComparer<PartitionInfoExs>.Default.Equals(PartitionInfoExs, other.PartitionInfoExs)&&
                   EqualityComparer<IEnumerable<Exception>>.Default.Equals(Exceptions, other.Exceptions);
        }
        public override int GetHashCode() {
            var hashCode = -2144340839;
            hashCode=hashCode*-1521134295+DriveNumber.GetHashCode();
            hashCode=hashCode*-1521134295+EqualityComparer<Id<DiskDriveInfoEx>>.Default.GetHashCode(DiskDriveDbId);
            hashCode=hashCode*-1521134295+DiskDriveMaker.GetHashCode();
            hashCode=hashCode*-1521134295+DiskDriveType.GetHashCode();
            hashCode=hashCode*-1521134295+EqualityComparer<string>.Default.GetHashCode(SerialNumber);
            hashCode=hashCode*-1521134295+EqualityComparer<PartitionInfoExs>.Default.GetHashCode(PartitionInfoExs);
            hashCode=hashCode*-1521134295+EqualityComparer<IEnumerable<Exception>>.Default.GetHashCode(Exceptions);
            return hashCode;
        }

        public Id<DiskDriveInfoEx> DiskDriveDbId { get; set; }
        public Id<DiskDriveInfoEx> DiskDriveId { get; set; }
        public DiskDriveMaker DiskDriveMaker { get; set; }
        public DiskDriveType DiskDriveType { get; set; }
        public int DriveNumber { get; set; }
        public IEnumerable<Exception> Exceptions { get; set; }
        public PartitionInfoExs PartitionInfoExs { get; set; }
        public string SerialNumber { get; set; }

    }

    public interface IPartitionInfoEx {
        IEnumerable<string> DriveLetters { get; set; }
        IEnumerable<Exception> Exceptions { get; set; }
        Id<PartitionInfoEx> PartitionDbId { get; set; }
        PartitionFileSystem PartitionFileSystem { get; set; }
        Id<PartitionInfoEx> PartitionId { get; set; }
        long Size { get; set; }

        bool Equals(object obj);
        bool Equals(PartitionInfoEx other);
        int GetHashCode();
    }

    public class PartitionInfoEx : IEquatable<PartitionInfoEx>, IPartitionInfoEx {
        public PartitionInfoEx() {
        }

        public PartitionInfoEx(IEnumerable<string> driveLetters, IEnumerable<Exception> exceptions, Id<PartitionInfoEx> partitionDbId, PartitionFileSystem partitionFileSystem, Id<PartitionInfoEx> partitionId, long size) {
            DriveLetters=driveLetters??throw new ArgumentNullException(nameof(driveLetters));
            Exceptions=exceptions??throw new ArgumentNullException(nameof(exceptions));
            PartitionDbId=partitionDbId;
            PartitionFileSystem=partitionFileSystem;
            PartitionId=partitionId;
            Size=size;
        }

        public IEnumerable<string> DriveLetters { get; set; }
        public IEnumerable<Exception> Exceptions { get; set; }
        public Id<PartitionInfoEx> PartitionDbId { get; set; }
        public PartitionFileSystem PartitionFileSystem { get; set; }
        public Id<PartitionInfoEx> PartitionId { get; set; }
        public long Size { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as PartitionInfoEx);
        }

        public bool Equals(PartitionInfoEx other) {
            return other!=null&&
                   EqualityComparer<IEnumerable<string>>.Default.Equals(DriveLetters, other.DriveLetters)&&
                   EqualityComparer<IEnumerable<Exception>>.Default.Equals(Exceptions, other.Exceptions)&&
                   PartitionDbId.Equals(other.PartitionDbId)&&
                   PartitionFileSystem==other.PartitionFileSystem&&
                   PartitionId.Equals(other.PartitionId)&&
                   Size==other.Size;
        }

        public override int GetHashCode() {
            var hashCode = -1093398953;
            hashCode=hashCode*-1521134295+EqualityComparer<IEnumerable<string>>.Default.GetHashCode(DriveLetters);
            hashCode=hashCode*-1521134295+EqualityComparer<IEnumerable<Exception>>.Default.GetHashCode(Exceptions);
            hashCode=hashCode*-1521134295+EqualityComparer<Id<PartitionInfoEx>>.Default.GetHashCode(PartitionDbId);
            hashCode=hashCode*-1521134295+PartitionFileSystem.GetHashCode();
            hashCode=hashCode*-1521134295+EqualityComparer<Id<PartitionInfoEx>>.Default.GetHashCode(PartitionId);
            hashCode=hashCode*-1521134295+Size.GetHashCode();
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
        public PartitionInfoExs() {
            PartitionInfoExCOD=new ConcurrentObservableDictionary<Id<PartitionInfoEx>, PartitionInfoEx>();
        }

        public PartitionInfoExs(ConcurrentObservableDictionary<Id<PartitionInfoEx>, PartitionInfoEx> partitionInfoExCOD) {
            PartitionInfoExCOD=partitionInfoExCOD??throw new ArgumentNullException(nameof(partitionInfoExCOD));
        }

        public ConcurrentObservableDictionary<Id<PartitionInfoEx>, PartitionInfoEx> PartitionInfoExCOD { get; set; }
    }

    public interface IDiskDriveInfoExs {
        ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, DiskDriveInfoEx> DiskDriveInfoExCOD { get; set; }
    }

    public class DiskDriveInfoExs : IDiskDriveInfoExs {
        public DiskDriveInfoExs() {
            DiskDriveInfoExCOD=new ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, DiskDriveInfoEx>();
        }

        public DiskDriveInfoExs(ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, DiskDriveInfoEx> DiskDriveInfoExCOD) {
            DiskDriveInfoExCOD=DiskDriveInfoExCOD??throw new ArgumentNullException(nameof(DiskDriveInfoExCOD));
        }

        public ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, DiskDriveInfoEx> DiskDriveInfoExCOD { get; set; }
    }
    public interface IDiskInfoExsContainer {

        bool Equals(DiskInfoExsContainer other);
        bool Equals(object obj);
        int GetHashCode();

        List<DiskDriveInfoEx> DiskInfoExs { get; set; }

    }

    public class DiskInfoExsContainer : IEquatable<DiskInfoExsContainer>, IDiskInfoExsContainer {

        public DiskInfoExsContainer(List<DiskDriveInfoEx> diskInfoExs) {
            DiskInfoExs=diskInfoExs??throw new ArgumentNullException(nameof(diskInfoExs));
        }

        public static bool operator !=(DiskInfoExsContainer left, DiskInfoExsContainer right) {
            return !(left==right);
        }
        public static bool operator ==(DiskInfoExsContainer left, DiskInfoExsContainer right) {
            return EqualityComparer<DiskInfoExsContainer>.Default.Equals(left, right);
        }

        public override bool Equals(object obj) {
            return Equals(obj as DiskInfoExsContainer);
        }
        public bool Equals(DiskInfoExsContainer other) {
            return other!=null&&
                   EqualityComparer<List<DiskDriveInfoEx>>.Default.Equals(DiskInfoExs, other.DiskInfoExs);
        }
        public override int GetHashCode() {
            return 130062218+EqualityComparer<List<DiskDriveInfoEx>>.Default.GetHashCode(DiskInfoExs);
        }

        public List<DiskDriveInfoEx> DiskInfoExs { get; set; }

    }

    public interface IDiskDrivePartitionIdentifier {
        ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, IPartitionInfoExs> DiskDriveInfoPartitionInfoCOD { get; set; }

        bool Equals(DiskDrivePartitionIdentifier other);
        bool Equals(object obj);
        int GetHashCode();
    }

    public class DiskDrivePartitionIdentifier : IEquatable<DiskDrivePartitionIdentifier>, IDiskDrivePartitionIdentifier {
        public DiskDrivePartitionIdentifier() {
            DiskDriveInfoPartitionInfoCOD=new ConcurrentObservableDictionary<Id<DiskDriveInfoEx>, IPartitionInfoExs>();
        }

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


    public interface IDiskDriveAnalysisResult {

         ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo> LookupDiskDriveAnalysisResultsCOD { get; set; }
        IList<Exception> DiskDriveAnalysisExceptions { get; set; }
         DiskDriveInfoExs DiskDriveAnalysisDiskDriveInfoExs { get; set; }

    }


}



