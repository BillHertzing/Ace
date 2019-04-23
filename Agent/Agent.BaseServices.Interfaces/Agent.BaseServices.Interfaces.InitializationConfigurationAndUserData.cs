using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Logging;

namespace Ace.Agent.BaseServices {
    public partial class BaseServices : Service {
        public static ILog Log = LogManager.GetLogger(typeof(BaseServices));

        #region  Interfaces:GetConfigurationDataRequest
        public object Post(GetConfigurationDataRequest request) {
            GetConfigurationDataResponse getConfigurationDataResponse = new GetConfigurationDataResponse(BaseServicesData.ConfigurationData);
            return getConfigurationDataResponse;
        }
        #endregion

        #region Interfaces:GetUserDataRequest
        public object Post(GetUserDataRequest request) {
            GetUserDataResponse getUserDataResponse = new GetUserDataResponse(BaseServicesData.UserData);
            return getUserDataResponse;
        }
        #endregion


        // ServiceStack will autowire this property with the corresponding instance from the DI Container
        public BaseServicesData BaseServicesData { get; set; }
        /*
        public object Any(BaseServicePutConfiguration request) {
        if(!IsAuthenticated && AppSettings.Get("LimitRemoteControlToAuthenticatedUsers", false))
        {
        throw new HttpError(HttpStatusCode.Forbidden, "You must be authenticated and authorized to use remote control.");
        }

        return new PutConfigurationResponse { Result = "SUCCESS OR FAILURE" };
        }

        public IAppSettings AppSettings {
        get; set;
        }
        */
    }
}
