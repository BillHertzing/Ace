using System.Net;
using Ace.AceService.BaseServicesModel;
using ServiceStack;
using ServiceStack.Configuration;

namespace Ace.AceService.BaseServicesInterface {
    public class BaseServices : Service {
        public object Any(BaseServiceIsAlive request) {
            return new IsAliveResponse { Result = $"Hello!" };
            
        }
        //public object Any(BaseServiceIsAlive request)
        //{
        //    return new IsAliveResponse { Result = $"Hello, {request.Name}!" };
        //}
        public object Any(BaseServiceGetConfiguration request) {
            return new GetConfigurationResponse { Result = AppSettings.Get<string>("Ace.AceService:ListeningOn")};
        }
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
    }
}
