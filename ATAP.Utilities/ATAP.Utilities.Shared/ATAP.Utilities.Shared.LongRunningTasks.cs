using ATAP.Utilities.TypedGuids;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ATAP.Utilities.LongRunningTasks {
    public interface ILongRunningTaskInfo {
        CancellationToken CT { get; set; }
        Id<CancellationTokenSource> CTSId { get; set; }
        Task LRTask { get; set; }
        Id<LongRunningTaskInfo> LRTId { get; set; }

        bool Equals(LongRunningTaskInfo other);
        bool Equals(object obj);
        int GetHashCode();
    }

    public class LongRunningTaskInfo : IEquatable<LongRunningTaskInfo>, ILongRunningTaskInfo {
        public LongRunningTaskInfo() {
        }

        public LongRunningTaskInfo(Task lRTask, Id<LongRunningTaskInfo> lRTId, Id<CancellationTokenSource> cTSId, CancellationToken cT) {
            LRTask=lRTask??throw new ArgumentNullException(nameof(lRTask));
            LRTId=lRTId;
            CTSId=cTSId;
            CT=cT;
        }

        public Task LRTask { get; set; }
        public Id<LongRunningTaskInfo> LRTId { get; set; }
        public Id<CancellationTokenSource> CTSId { get; set; }
        public CancellationToken CT { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as LongRunningTaskInfo);
        }

        public bool Equals(LongRunningTaskInfo other) {
            return other!=null&&
                   EqualityComparer<Task>.Default.Equals(LRTask, other.LRTask)&&
                   LRTId.Equals(other.LRTId)&&
                   CTSId.Equals(other.CTSId)&&
                   EqualityComparer<CancellationToken>.Default.Equals(CT, other.CT);
        }

        public override int GetHashCode() {
            var hashCode = 1114179746;
            hashCode=hashCode*-1521134295+EqualityComparer<Task>.Default.GetHashCode(LRTask);
            hashCode=hashCode*-1521134295+EqualityComparer<Id<LongRunningTaskInfo>>.Default.GetHashCode(LRTId);
            hashCode=hashCode*-1521134295+EqualityComparer<Id<CancellationTokenSource>>.Default.GetHashCode(CTSId);
            hashCode=hashCode*-1521134295+EqualityComparer<CancellationToken>.Default.GetHashCode(CT);
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
        public LongRunningTaskStatuses() {
            LongRunningTaskStatusList=new List<LongRunningTaskStatus>();
        }

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
        public LongRunningTaskStatus() { }

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



