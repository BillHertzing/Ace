using System;
using System.Collections.Specialized;
using System.ComponentModel;
using ServiceStack.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.LongRunningTasks;
namespace Ace.Agent.BaseServices {

     public partial class BaseServicesData {
        #region Properties:LongRunningTasks
        public Dictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo> LongRunningTasks { get; set; }
        #endregion
    }

}