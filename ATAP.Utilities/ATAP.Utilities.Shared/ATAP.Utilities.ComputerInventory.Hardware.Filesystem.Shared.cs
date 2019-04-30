using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using Swordfish.NET.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ATAP.Utilities.FileSystem {
    public interface IDirectoryInfoEx {
        Id<DirectoryInfoEx> DirectoryDbId { get; set; }
        Id<DirectoryInfoEx> DirectoryId { get; set; }
        DirectoryInfo DirectoryInfo { get; set; }
        IEnumerable<Exception> Exceptions { get; set; }

        bool Equals(DirectoryInfoEx other);
        bool Equals(object obj);
        int GetHashCode();
    }

    public class DirectoryInfoEx : IEquatable<DirectoryInfoEx>, IDirectoryInfoEx {
        public DirectoryInfoEx() :this (new Id<DirectoryInfoEx>(), new Id<DirectoryInfoEx>(),null, new List<Exception>()) { }

        public DirectoryInfoEx(Id<DirectoryInfoEx> directoryDbId, Id<DirectoryInfoEx> directoryId, DirectoryInfo directoryInfo, IEnumerable<Exception> exceptions) {
            DirectoryDbId=directoryDbId;
            DirectoryId=directoryId;
            DirectoryInfo=directoryInfo;
            Exceptions=exceptions??throw new ArgumentNullException(nameof(exceptions));
        }

        public Id<DirectoryInfoEx> DirectoryDbId { get; set; }
        public Id<DirectoryInfoEx> DirectoryId { get; set; }
        public DirectoryInfo DirectoryInfo { get; set; }
        public IEnumerable<Exception> Exceptions { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as DirectoryInfoEx);
        }

        public bool Equals(DirectoryInfoEx other) {
            return other!=null&&
                   DirectoryDbId.Equals(other.DirectoryDbId)&&
                   DirectoryId.Equals(other.DirectoryId)&&
                   EqualityComparer<DirectoryInfo>.Default.Equals(DirectoryInfo, other.DirectoryInfo)&&
                   EqualityComparer<IEnumerable<Exception>>.Default.Equals(Exceptions, other.Exceptions);
        }

        public override int GetHashCode() {
            var hashCode = -260512417;
            hashCode=hashCode*-1521134295+EqualityComparer<Id<DirectoryInfoEx>>.Default.GetHashCode(DirectoryDbId);
            hashCode=hashCode*-1521134295+EqualityComparer<Id<DirectoryInfoEx>>.Default.GetHashCode(DirectoryId);
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

    public interface IDirectoryInfoExs {
        ConcurrentObservableDictionary<Id<DirectoryInfoEx>, DirectoryInfoEx> DirectoryInfoExCOD { get; set; }
    }

    public class DirectoryInfoExs : IDirectoryInfoExs {
        public DirectoryInfoExs():this(new ConcurrentObservableDictionary<Id<DirectoryInfoEx>, DirectoryInfoEx>()) {}

        public DirectoryInfoExs(ConcurrentObservableDictionary<Id<DirectoryInfoEx>, DirectoryInfoEx> directoryInfoExCOD) {
            DirectoryInfoExCOD=directoryInfoExCOD??throw new ArgumentNullException(nameof(directoryInfoExCOD));
        }

        public ConcurrentObservableDictionary<Id<DirectoryInfoEx>, DirectoryInfoEx> DirectoryInfoExCOD { get; set; }
    }

    public interface IFileInfoEx {
        FileInfo FileInfo { get; set; }
        Id<FileInfoEx> FileInfoDbId { get; set; }
        Id<FileInfoEx> FileInfoId { get; set; }
        string Hash { get; set; }
        string Path { get; set; }

        bool Equals(FileInfoEx other);
        bool Equals(object obj);
        int GetHashCode();
    }

    public class FileInfoEx : IEquatable<FileInfoEx>, IFileInfoEx {
        public FileInfoEx() : this (string.Empty, new Id<FileInfoEx>(), new Id<FileInfoEx>(),null,string.Empty, new List<Exception>()) { }

        public FileInfoEx(string path, Id<FileInfoEx> fileInfoDbId, Id<FileInfoEx> fileInfoId, FileInfo fileInfo, string hash, IList<Exception> exceptions) {
            Path=path??throw new ArgumentNullException(nameof(path));
            FileInfoDbId=fileInfoDbId;
            FileInfoId=fileInfoId;
            FileInfo=fileInfo;
            Hash=hash??throw new ArgumentNullException(nameof(hash));
            Exceptions=exceptions??throw new ArgumentNullException(nameof(exceptions));
        }


        // Path will be string.Empty if FileInfo is populated
        public string Path { get; set; }
        public Id<FileInfoEx> FileInfoDbId { get; set; }
        public Id<FileInfoEx> FileInfoId { get; set; }
        public FileInfo FileInfo { get; set; }
        public string Hash { get; set; }
        public IList<Exception> Exceptions;

        public override bool Equals(object obj) {
            return Equals(obj as FileInfoEx);
        }

        public bool Equals(FileInfoEx other) {
            return other!=null&&
                   Path==other.Path&&
                   FileInfoDbId.Equals(other.FileInfoDbId)&&
                   FileInfoId.Equals(other.FileInfoId)&&
                   EqualityComparer<FileInfo>.Default.Equals(FileInfo, other.FileInfo)&&
                   Hash==other.Hash&&
                   EqualityComparer<IList<Exception>>.Default.Equals(Exceptions, other.Exceptions);
        }

        public override int GetHashCode() {
            var hashCode = 218821843;
            hashCode=hashCode*-1521134295+EqualityComparer<string>.Default.GetHashCode(Path);
            hashCode=hashCode*-1521134295+EqualityComparer<Id<FileInfoEx>>.Default.GetHashCode(FileInfoDbId);
            hashCode=hashCode*-1521134295+EqualityComparer<Id<FileInfoEx>>.Default.GetHashCode(FileInfoId);
            hashCode=hashCode*-1521134295+EqualityComparer<FileInfo>.Default.GetHashCode(FileInfo);
            hashCode=hashCode*-1521134295+EqualityComparer<string>.Default.GetHashCode(Hash);
            hashCode=hashCode*-1521134295+EqualityComparer<IList<Exception>>.Default.GetHashCode(Exceptions);
            return hashCode;
        }

        public static bool operator ==(FileInfoEx left, FileInfoEx right) {
            return EqualityComparer<FileInfoEx>.Default.Equals(left, right);
        }

        public static bool operator !=(FileInfoEx left, FileInfoEx right) {
            return !(left==right);
        }
    }

    public interface IFileInfoExs {
        ConcurrentObservableDictionary<Id<FileInfoEx>, FileInfoEx> FileInfoExCOD { get; set; }
    }

    public class FileInfoExs : IFileInfoExs {
        public FileInfoExs() :this (new ConcurrentObservableDictionary<Id<FileInfoEx>, FileInfoEx> ()) {}

        public FileInfoExs(ConcurrentObservableDictionary<Id<FileInfoEx>, FileInfoEx> fileInfoExCOD) {
            FileInfoExCOD=fileInfoExCOD??throw new ArgumentNullException(nameof(fileInfoExCOD));
        }

        public ConcurrentObservableDictionary<Id<FileInfoEx>, FileInfoEx> FileInfoExCOD { get; set; }
    }

   
}
