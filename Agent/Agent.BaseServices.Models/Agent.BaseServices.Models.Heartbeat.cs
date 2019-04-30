using ServiceStack;
using System;
using ATAP.Utilities.LongRunningTasks;

namespace Ace.Agent.BaseServices {
    
    #region IsAlive
    [Route("/isAlive")]
    [Route("/isAlive/{Name}")]
    public class IsAliveReqPayload : IReturn<IsAliveRspPayload> {
        public string Name { get; set; }
    }

    public class IsAliveRspPayload {
        // ToDo: return the return the LongRunningTaskId
        public string Result { get; set; }
    }
    #endregion IsAlive

}
