using ServiceStack;

namespace Ace.PlugIn.AMQPServices
{

    // This route will ensure that ServiceStack has the AMQP PlugIn loaded
    [Route("/VerifyAMQP")]
    public class VerifyAMQPReqDTO : IReturn<VerifyAMQPRspDTO>
    {
    }
    public class VerifyAMQPRspDTO
    {
        public ResponseStatus ResponseStatus { get; set; }
        public string Result { get; set; }
    }
}
