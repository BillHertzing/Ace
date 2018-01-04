using ServiceStack;
using AceAgent.ServiceModel;

namespace AceAgent.ServiceInterface
{
    public class MyServices : Service
    {
        public object Any(MinerStatus request)
        {
            return new MinerStatusResponse { Result = $"Hello, {request.ID}!" };
        }

        public object Any(Hello request)
        {
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }
    }
}