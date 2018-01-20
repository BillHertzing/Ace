using ServiceStack;
using Ace.AceService.BaseServiceModel;
using System.Net;
using ServiceStack.Configuration;

namespace Ace.AceService.BaseServiceInterface
{
    public class BaseServices : Service
    {
        public IAppSettings AppSettings { get; set; }

        public object Any(BaseServiceIsAlive request)
        {
            return new IsAliveResponse { Result = $"Hello!" };
        }
        //public object Any(BaseServiceIsAlive request)
        //{
        //    return new IsAliveResponse { Result = $"Hello, {request.Name}!" };
        //}
        public object Any(BaseServiceGetConfiguration request)
        {
            return new GetConfigurationResponse { Result = AppSettings.ToJson() };
        }
        public object Any(BaseServicePutConfiguration request)
        {
            if (!IsAuthenticated && AppSettings.Get("LimitRemoteControlToAuthenticatedUsers", false))
                throw new HttpError(HttpStatusCode.Forbidden, "You must be authenticated to use remote control.");

            return new PutConfigurationResponse { Result = "SUCCESS OR FAILURE" };
        }
    }
}