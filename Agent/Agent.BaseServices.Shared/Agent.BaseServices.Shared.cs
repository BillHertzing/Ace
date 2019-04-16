using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ace.Agent.BaseServices
{
    public class DiskDriveToDBGraphTasks {
        public DiskDriveToDBGraphTasks() : this(new List<Task>()) { }
        public DiskDriveToDBGraphTasks(IEnumerable<Task> tasks) {
            Tasks=tasks;
        }
        IEnumerable<Task> Tasks { get; set; }
    }
}
