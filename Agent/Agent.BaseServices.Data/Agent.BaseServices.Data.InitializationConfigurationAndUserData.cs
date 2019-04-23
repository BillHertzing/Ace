using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using ATAP.Utilities.Http;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Logging;
using ServiceStack.Redis;
using Swordfish.NET.Collections;
using Polly;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;

namespace Ace.Agent.BaseServices {
    public partial class BaseServicesData : IDisposable {
        #region Properties:ConfigurationData
        public ConfigurationData ConfigurationData { get; set; }
        #endregion
        #region Properties:ConfigurationData
        public UserData UserData { get; set; }
        #endregion

		#region IndirectConstructors
        void ConstructConfigurationData () {
			ConfigurationData=new ConfigurationData(RedisCacheConnectionString, MySqlConnectionString);
        }
        void ConstructUserData() {
            UserData=new BaseServices.UserData();
        }
        #endregion
		
    }

}
