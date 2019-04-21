using System;
using System.Collections.Specialized;
using System.ComponentModel;
using ServiceStack.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using ATAP.Utilities.ComputerInventory;
using ATAP.Utilities.ComputerHardware.Enumerations;
using ATAP.Utilities.DiskDrive;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.LongRunningTasks;

namespace Ace.Agent.DiskAnalysisServices {

    public partial class DiskAnalysisServicesData {
        public List<WalkDiskDriveResult> WalkDiskDriveResults { get; set; }
        //ToDo: Make the creation of the List of Containers Lazy
        public Dictionary<Id<LongRunningTaskInfo>, WalkDiskDriveResultContainer> WalkDiskDriveResultContainers { get; set; }

        void ConstructDiskAnalysisData() {
            WalkDiskDriveResults=new List<WalkDiskDriveResult>();
            WalkDiskDriveResultContainers=new Dictionary<Id<LongRunningTaskInfo>, WalkDiskDriveResultContainer>();
        }
    }
}


