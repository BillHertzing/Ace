using System;
using System.Collections.Specialized;
using System.ComponentModel;
using ServiceStack.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Ace.Agent.BaseServices {

    public class LRTaskInfo {

        public LRTaskInfo(Guid iD, Task task) {
            //Log.Debug("Starting LRTaskInfo ctor");
            ID=iD;
            Task=task;
            //Log.Debug("Leaving LRTaskInfo ctor");
        }
        #region EventHandlers

        #endregion

        #region String Constants
        #region String Constants:Configuration Key strings

        #endregion 

        #region String Constants:Exception Messages

        #endregion

        #endregion 

        #region Static Fields:Logger
        // Create a logger for this class
        public static ILog Log = LogManager.GetLogger(typeof(LRTaskInfo));
        #endregion 

        #region Properties
        public Guid ID { get; set; }
        public Task Task { get; set; }

        #endregion 
    }
    public partial class BaseServicesData {
        #region Properties:LongRunningTasks
        public Dictionary<Guid, LRTaskInfo> LongRunningTasks { get; set; }
        #endregion
    }

}
