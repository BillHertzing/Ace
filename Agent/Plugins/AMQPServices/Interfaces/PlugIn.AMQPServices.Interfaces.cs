using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Host;
using ATAP.Utilities.ETW;
namespace Ace.PlugIn.AMQPServices {

#if TRACE
    [ETWLogAttribute]
#endif
    public class AMQPService : Service {

    public object Any(VerifyAMQPReqDTO request) {
			return new VerifyAMQPRspDTO { Result = "yes" };
		}
    }

    //public class AMQPServiceController : ServiceController {
    //    private IAppHost appHost;

    //    public AMQPServiceController(ServiceStackHost appHost) : base(appHost) {

    //    }

    //    public AMQPServiceController(IAppHost appHost) : base(appHost as ServiceStackHost) {
    //            this.appHost = appHost;
    //    }

    //    public AMQPServiceController(ServiceStackHost appHost, Func<IEnumerable<Type>> resolveServicesFn) : base(appHost, resolveServicesFn) {

    //    }

    //    public AMQPServiceController(ServiceStackHost appHost, System.Reflection.Assembly[] assembliesWithServices) : base(appHost, assembliesWithServices) {

    //    }
    //}
}
