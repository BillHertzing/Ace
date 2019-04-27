using System;
using System.Collections.Specialized;
using System.ComponentModel;
using ServiceStack.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.LongRunningTasks;
using System.Threading;

namespace Ace.Agent.BaseServices {

    public partial class BaseServicesData {
        #region Properties:LongRunningTasks
        public Dictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo> LongRunningTasks { get; set; }
        public Dictionary<Id<CancellationTokenSource>, CancellationTokenSource> CancellationTokenSources { get; set; }
        public Dictionary<Id<CancellationToken>, CancellationToken> CancellationTokens { get; set; }
        #endregion

        void ConstructLongRunningTasks() {
            LongRunningTasks=new Dictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo>();
            Container.Register<Dictionary<Id<LongRunningTaskInfo>, LongRunningTaskInfo>>(c => LongRunningTasks);
            CancellationTokenSources=new Dictionary<Id<CancellationTokenSource>, CancellationTokenSource>();
            Container.Register<Dictionary<Id<CancellationTokenSource>, CancellationTokenSource>>(c => CancellationTokenSources);
            CancellationTokens=new Dictionary<Id<CancellationToken>, CancellationToken>();
            Container.Register<Dictionary<Id<CancellationToken>, CancellationToken>>(c => CancellationTokens);
        }
    }

}
