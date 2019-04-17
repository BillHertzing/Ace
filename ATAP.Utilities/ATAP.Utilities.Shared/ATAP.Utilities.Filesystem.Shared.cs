using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ATAP.Utilities.Filesystem {
    public class DirectoryInfoEx {
        public DirectoryInfoEx() : this(-1, Guid.Empty, new DirectoryInfo(string.Empty), new List<Exception>()) { }
        public DirectoryInfoEx(long directoryInfoId, Guid directoryInfoGuid, DirectoryInfo directoryInfo, IEnumerable<Exception> exceptions) {
            DirectoryInfoId=directoryInfoId;
            DirectoryInfoGuid=directoryInfoGuid;
            DirectoryInfo=directoryInfo;
            Exceptions=exceptions;
        }
        public long DirectoryInfoId;
        public Guid DirectoryInfoGuid;
        public DirectoryInfo DirectoryInfo { get; set; }
        public IEnumerable<Exception> Exceptions;
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
