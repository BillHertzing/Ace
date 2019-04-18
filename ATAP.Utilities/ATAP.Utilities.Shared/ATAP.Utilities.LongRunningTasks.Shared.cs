using ATAP.Utilities.TypedGuids;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATAP.Utilities.LongRunningTasks {
    public interface ILongRunningTaskInfo {
        Task LRTask { get; set; }
        Id<LongRunningTaskInfo> Id { get; set; }

        bool Equals(LongRunningTaskInfo other);
        bool Equals(object obj);
        int GetHashCode();
    }

    public class LongRunningTaskInfo : IEquatable<LongRunningTaskInfo>, ILongRunningTaskInfo {
        public LongRunningTaskInfo() {
        }

        public LongRunningTaskInfo(Id<LongRunningTaskInfo> id, Task lRTask) {
            LRTask=lRTask??throw new ArgumentNullException(nameof(lRTask));
            Id=id;
        }

        public Task LRTask { get; set; }
        public Id<LongRunningTaskInfo> Id { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as LongRunningTaskInfo);
        }

        public bool Equals(LongRunningTaskInfo other) {
            return other!=null&&
                   EqualityComparer<Task>.Default.Equals(LRTask, other.LRTask)&&
                   Id.Equals(other.Id);
        }

        public override int GetHashCode() {
            var hashCode = -288306430;
            hashCode=hashCode*-1521134295+EqualityComparer<Task>.Default.GetHashCode(LRTask);
            hashCode=hashCode*-1521134295+EqualityComparer<Id<LongRunningTaskInfo>>.Default.GetHashCode(Id);
            return hashCode;
        }

        public static bool operator ==(LongRunningTaskInfo left, LongRunningTaskInfo right) {
            return EqualityComparer<LongRunningTaskInfo>.Default.Equals(left, right);
        }

        public static bool operator !=(LongRunningTaskInfo left, LongRunningTaskInfo right) {
            return !(left==right);
        }
    }



}



