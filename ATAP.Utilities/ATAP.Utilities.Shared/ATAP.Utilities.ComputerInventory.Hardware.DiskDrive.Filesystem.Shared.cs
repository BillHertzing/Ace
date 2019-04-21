using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ATAP.Utilities.Filesystem {
    public interface IDirectoryInfoEx {
        DirectoryInfo DirectoryInfo { get; set; }
        Guid DirectoryInfoGuid { get; set; }
        long DirectoryInfoId { get; set; }
        IEnumerable<Exception> Exceptions { get; set; }

        bool Equals(DirectoryInfoEx other);
        bool Equals(object obj);
        int GetHashCode();
    }

    public class DirectoryInfoEx : IEquatable<DirectoryInfoEx>, IDirectoryInfoEx {
        public DirectoryInfoEx() : this(-1, Guid.Empty, new DirectoryInfo(string.Empty), new List<Exception>()) { }
        public DirectoryInfoEx(long directoryInfoId, Guid directoryInfoGuid, DirectoryInfo directoryInfo, IEnumerable<Exception> exceptions) {
            DirectoryInfoId=directoryInfoId;
            DirectoryInfoGuid=directoryInfoGuid;
            DirectoryInfo=directoryInfo;
            Exceptions=exceptions;
        }
        public long DirectoryInfoId { get; set; }
        public Guid DirectoryInfoGuid { get; set; }
        public DirectoryInfo DirectoryInfo { get; set; }
        public IEnumerable<Exception> Exceptions { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as DirectoryInfoEx);
        }

        public bool Equals(DirectoryInfoEx other) {
            return other!=null&&
                   DirectoryInfoId==other.DirectoryInfoId&&
                   DirectoryInfoGuid.Equals(other.DirectoryInfoGuid)&&
                   EqualityComparer<DirectoryInfo>.Default.Equals(DirectoryInfo, other.DirectoryInfo)&&
                   EqualityComparer<IEnumerable<Exception>>.Default.Equals(Exceptions, other.Exceptions);
        }

        public override int GetHashCode() {
            var hashCode = -741419411;
            hashCode=hashCode*-1521134295+DirectoryInfoId.GetHashCode();
            hashCode=hashCode*-1521134295+EqualityComparer<Guid>.Default.GetHashCode(DirectoryInfoGuid);
            hashCode=hashCode*-1521134295+EqualityComparer<DirectoryInfo>.Default.GetHashCode(DirectoryInfo);
            hashCode=hashCode*-1521134295+EqualityComparer<IEnumerable<Exception>>.Default.GetHashCode(Exceptions);
            return hashCode;
        }

        public static bool operator ==(DirectoryInfoEx left, DirectoryInfoEx right) {
            return EqualityComparer<DirectoryInfoEx>.Default.Equals(left, right);
        }

        public static bool operator !=(DirectoryInfoEx left, DirectoryInfoEx right) {
            return !(left==right);
        }
    }

    public interface IDirectoryInfoExContainer {
        DirectoryInfoEx DirectoryInfoEx { get; set; }
    }

    public class FileInfoEx {
        public FileInfoEx() : this(string.Empty, -1, Guid.Empty, new FileInfo(string.Empty), string.Empty, new List<Exception>()) { }
        public FileInfoEx(string path, long fileInfoId, Guid fileInfoGuid, IList<Exception> exceptions) {
            Path=path;
            FileInfoId=fileInfoId;
            FileInfoGuid=fileInfoGuid;
            Exceptions=exceptions;
        }
        public FileInfoEx(string path, long fileInfoId, Guid fileInfoGuid, FileInfo fileInfo, string hash, IList<Exception> exceptions) {
            //ToDo: Add validation during construction that Path and FileInfo are mutually exclusive
            Path=path;
            FileInfoId=fileInfoId;
            FileInfoGuid=fileInfoGuid;
            FileInfo=fileInfo;
            Hash=hash;
            Exceptions=exceptions;
        }
        // Path will be string.Empty if FileInfo is populated
        public string Path { get; set; }
        public long FileInfoId { get; set; }
        public Guid FileInfoGuid { get; set; }
        // FileInfo will be null if Path is not string.Empty
        public FileInfo FileInfo { get; set; }
        public string Hash { get; set; }
        public IList<Exception> Exceptions;
    }

    public interface IFileInfoExContainer {
        FileInfoEx FileInfoEx { get; set; }
    }

    public interface IWalkFilesystemResult {
        int DeepestDirectoryTree { get; set; }
        List<Exception> Exceptions { get; set; }
        long LargestFile { get; set; }
        int NumberOfDirectories { get; set; }
        int NumberOfFiles { get; set; }
    }

    public interface IWalkFilesystemResultContainer {
        IWalkFilesystemResult WalkFilesystemResult { get; set; }
    }

    public class WalkFilesystemResult : IWalkFilesystemResult {
        public WalkFilesystemResult(int numberOfDirectories, int numberOfFiles, int deepestDirectoryTree, long largestFile, List<Exception> exceptions) {
            NumberOfDirectories=numberOfDirectories;
            NumberOfFiles=numberOfFiles;
            DeepestDirectoryTree=deepestDirectoryTree;
            LargestFile=largestFile;
            Exceptions=exceptions??throw new ArgumentNullException(nameof(exceptions));
        }

        public int NumberOfDirectories { get; set; }
        public int NumberOfFiles { get; set; }
        public int DeepestDirectoryTree { get; set; }
        public long LargestFile { get; set; }

        public List<Exception> Exceptions { get; set; }
    }
}
