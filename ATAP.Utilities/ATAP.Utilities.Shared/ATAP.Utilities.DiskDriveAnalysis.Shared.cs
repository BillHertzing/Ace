

using ATAP.Utilities.ComputerHardware.Enumerations;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using Swordfish.NET.Collections;
using System;
using System.Collections.Generic;

namespace ATAP.Utilities.DiskDrive {
    
    public interface IAnalyzeDiskDriveResult {
        IDiskDriveInfoExs DiskDriveInfoExs { get; set; }
        IPartitionInfoExs PartitionInfoExs { get; set; }

        IList<Exception> Exceptions { get; set; }
    }
    public class AnalyzeDiskDriveResult : IAnalyzeDiskDriveResult {
        public AnalyzeDiskDriveResult() :this(new DiskDriveInfoExs(), new PartitionInfoExs(),  new List<Exception>()) { }

        public AnalyzeDiskDriveResult(IDiskDriveInfoExs diskDriveInfoExs, IPartitionInfoExs partitionInfoExs, IList<Exception> exceptions) {
            DiskDriveInfoExs=diskDriveInfoExs??throw new ArgumentNullException(nameof(diskDriveInfoExs));
            PartitionInfoExs=partitionInfoExs??throw new ArgumentNullException(nameof(partitionInfoExs));
            Exceptions=exceptions??throw new ArgumentNullException(nameof(exceptions));
        }

        public IDiskDriveInfoExs DiskDriveInfoExs { get; set; }
        public IPartitionInfoExs PartitionInfoExs { get; set; }
        public IList<Exception> Exceptions { get; set; }
    }

    public interface IAnalyzeDiskDriveResults {
        ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, IAnalyzeDiskDriveResult> AnalyzeDiskDriveResultsCOD { get; set; }
    }

    public class AnalyzeDiskDriveResults : IAnalyzeDiskDriveResults {
        public AnalyzeDiskDriveResults() :this(new ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, IAnalyzeDiskDriveResult>()) { }

        public AnalyzeDiskDriveResults(ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, IAnalyzeDiskDriveResult> analyzeDiskDriveResultsCOD) {
            AnalyzeDiskDriveResultsCOD=analyzeDiskDriveResultsCOD??throw new ArgumentNullException(nameof(analyzeDiskDriveResultsCOD));
        }

        public ConcurrentObservableDictionary<Id<LongRunningTaskInfo>, IAnalyzeDiskDriveResult> AnalyzeDiskDriveResultsCOD { get; set; }
    }

    public interface IAnalyzeDiskDriveProgress {
        bool Completed { get; set; }
        List<Exception> Exceptions { get; set; }
        int LargestNumberOfPartitions { get; set; }
        long LargestPartition { get; set; }
        int NumberOfDiskDrives { get; set; }
    }

    public class AnalyzeDiskDriveProgress : IAnalyzeDiskDriveProgress {
        public AnalyzeDiskDriveProgress() :this(false, -1, -1, -1, new List<Exception>()) {}

        public AnalyzeDiskDriveProgress(bool completed, int numberOfDiskDrives, int largestNumberOfPartitions, long largestPartition, List<Exception> exceptions) {
            Completed=completed;
            NumberOfDiskDrives=numberOfDiskDrives;
            LargestNumberOfPartitions=largestNumberOfPartitions;
            LargestPartition=largestPartition;
            Exceptions=exceptions??throw new ArgumentNullException(nameof(exceptions));
        }

        public bool Completed { get; set; }
        public int NumberOfDiskDrives { get; set; }
        public int LargestNumberOfPartitions { get; set; }
        public long LargestPartition { get; set; }
        public List<Exception> Exceptions { get; set; }
    }

}



