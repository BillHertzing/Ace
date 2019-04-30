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
        #region Properties:AuthenticationData
        public IAuthenticationData AuthenticationData { get; set; }
        #endregion
        #region Properties:AuthorizationData
        public IAuthorizationData AuthorizationData { get; set; }
        #endregion

        #region IndirectConstructors
        void ConstructAuthenticationData(IAppHost appHost) {
            
            // See if the MySQL configuration key exists, if so register MySQL as the RDBMS behind ORMLite
            if (appHost.AppSettings
                .Exists(configKeyPrefix+configKeyMySqlConnectionString)) {
                var appSettingsConfigValueMySqlConnectionString = appHost.AppSettings
                    .GetString(configKeyPrefix+
                    configKeyMySqlConnectionString);
                // Configure OrmLiteConnectionFactory and register it
                Container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(appSettingsConfigValueMySqlConnectionString, MySqlDialect.Provider));
                // Access the OrmLiteConnectionFactory
                var dbFactory = Container.TryResolve<IDbConnectionFactory>();
                // Try to open the RDBMS to ensure the RDBMS is listening and the connection string is correct
                try {
                    using (var db = dbFactory.Open()) {
                        // do nothing, just open a connection to the registered  RDBMS
                        Log.Debug($"In BaseServicesData .ctor: Successfully opened connection to RDBMS");
                    }
                }
                catch (Exception e) {
                    Log.Debug($"In BaseServicesData .ctor: Exception when trying to connect to the MySQL RDBMS: Message = {e.Message}");
                    throw new Exception(MySqlCannotConnectExceptionMessage, e);
                }
            } else {
                throw new NotImplementedException(MySqlConnectionStringKeyNotFoundExceptionMessage);
            }
            // Register an Auth Repository
            Container.Register<IAuthRepository>(c => new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));
            /// Create the  UserAuth and UserAuthDetails tables in the RDBMS if they do not already exist
            Container.Resolve<IAuthRepository>().InitSchema();
            AuthenticationData=new AuthenticationData();
        }
        void ConstructAuthorizationData(IAppHost appHost) {
            AuthorizationData=new AuthorizationData();
        }
        #endregion

    }

}
