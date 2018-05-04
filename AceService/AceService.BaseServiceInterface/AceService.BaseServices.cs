using System.Net;
using Ace.AceService.BaseServiceModel;
using ServiceStack;
using ServiceStack.Configuration;

namespace Ace.AceService.BaseServiceInterface {
    public class BaseServices : Service {
        public object Any(BaseServiceIsAlive request) {
            var dto = new IsAliveResponse { Result = $"Hello!" };
            return new HttpResult(dto) { Headers =
                {
                                         { "Access-Control-Allow-Origin", "*" },
                                                 { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
                                                 { "Access-Control-Allow-Headers", "Content-Type" },
                                         }
            };
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