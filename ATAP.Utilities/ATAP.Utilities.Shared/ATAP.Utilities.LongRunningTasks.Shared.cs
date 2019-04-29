using ATAP.Utilities.TypedGuids;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ATAP.Utilities.LongRunningTasks {



    public interface ILongRunningTaskInfo {
        bool Equals(object obj);
        int GetHashCode();

        CancellationTokenSource CTS { get; set; }
        Task LRTask { get; set; }
        Id<LongRunningTaskInfo> LRTId { get; set; }
    }
    public class LongRunningTaskInfo : IEquatable<LongRunningTaskInfo>, ILongRunningTaskInfo {
        public LongRunningTaskInfo() :this (new Id<LongRunningTaskInfo>(),null,null) { }

        public LongRunningTaskInfo(Id<LongRunningTaskInfo> lRTId, Task lRTask, CancellationTokenSource cTS) {
            LRTId=lRTId;
            LRTask=lRTask;
            CTS=cTS;
        }

        public Id<LongRunningTaskInfo> LRTId { get; set; }
        public Task LRTask { get; set; }
        public CancellationTokenSource CTS { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as LongRunningTaskInfo);
        }

        public bool Equals(LongRunningTaskInfo other) {
            return other!=null&&
                   LRTId.Equals(other.LRTId)&&
                   EqualityComparer<Task>.Default.Equals(LRTask, other.LRTask)&&
                   EqualityComparer<CancellationTokenSource>.Default.Equals(CTS, other.CTS);
        }

        public override int GetHashCode() {
            var hashCode = -1047278827;
            hashCode=hashCode*-1521134295+EqualityComparer<Id<LongRunningTaskInfo>>.Default.GetHashCode(LRTId);
            hashCode=hashCode*-1521134295+EqualityComparer<Task>.Default.GetHashCode(LRTask);
            hashCode=hashCode*-1521134295+EqualityComparer<CancellationTokenSource>.Default.GetHashCode(CTS);
            return hashCode;
        }

        public static bool operator ==(LongRunningTaskInfo left, LongRunningTaskInfo right) {
            return EqualityComparer<LongRunningTaskInfo>.Default.Equals(left, right);
        }

        public static bool operator !=(LongRunningTaskInfo left, LongRunningTaskInfo right) {
            return !(left==right);
        }
    }

    public interface ILongRunningTaskStatuses {
        List<LongRunningTaskStatus> LongRunningTaskStatusList { get; set; }

        bool Equals(LongRunningTaskStatuses other);
        bool Equals(object obj);
        int GetHashCode();
    }

    public class LongRunningTaskStatuses : IEquatable<LongRunningTaskStatuses>, ILongRunningTaskStatuses {
        public LongRunningTaskStatuses() :this(new List<LongRunningTaskStatus>()){ }

        public LongRunningTaskStatuses(List<LongRunningTaskStatus> longRunningTaskStatusList) {
            LongRunningTaskStatusList=longRunningTaskStatusList??throw new ArgumentNullException(nameof(longRunningTaskStatusList));
        }

        public List<LongRunningTaskStatus> LongRunningTaskStatusList { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as LongRunningTaskStatuses);
        }

        public bool Equals(LongRunningTaskStatuses other) {
            return other!=null&&
                   EqualityComparer<List<LongRunningTaskStatus>>.Default.Equals(LongRunningTaskStatusList, other.LongRunningTaskStatusList);
        }

        public override int GetHashCode() {
            return -202825235+EqualityComparer<List<LongRunningTaskStatus>>.Default.GetHashCode(LongRunningTaskStatusList);
        }

        public static bool operator ==(LongRunningTaskStatuses left, LongRunningTaskStatuses right) {
            return EqualityComparer<LongRunningTaskStatuses>.Default.Equals(left, right);
        }

        public static bool operator !=(LongRunningTaskStatuses left, LongRunningTaskStatuses right) {
            return !(left==right);
        }
    }

    public interface ILongRunningTaskStatus {
        Id<LongRunningTaskInfo> Id { get; set; }
        TaskStatus TaskStatus { get; set; }

        bool Equals(LongRunningTaskStatus other);
        bool Equals(object obj);
        int GetHashCode();
    }

    public class LongRunningTaskStatus : IEquatable<LongRunningTaskStatus>, ILongRunningTaskStatus {
        public LongRunningTaskStatus() :this(new Id<LongRunningTaskInfo>(),TaskStatus.Created ) { }

        public LongRunningTaskStatus(Id<LongRunningTaskInfo> id, TaskStatus taskStatus) {
            Id=id;
            TaskStatus=taskStatus;
        }

        public Id<LongRunningTaskInfo> Id { get; set; }
        public TaskStatus TaskStatus { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as LongRunningTaskStatus);
        }

        public bool Equals(LongRunningTaskStatus other) {
            return other!=null&&
                   Id.Equals(other.Id)&&
                   TaskStatus==other.TaskStatus;
        }

        public override int GetHashCode() {
            var hashCode = -126777866;
            hashCode=hashCode*-1521134295+EqualityComparer<Id<LongRunningTaskInfo>>.Default.GetHashCode(Id);
            hashCode=hashCode*-1521134295+TaskStatus.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(LongRunningTaskStatus left, LongRunningTaskStatus right) {
            return EqualityComparer<LongRunningTaskStatus>.Default.Equals(left, right);
        }

        public static bool operator !=(LongRunningTaskStatus left, LongRunningTaskStatus right) {
            return !(left==right);
        }
    }

}



