using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;


namespace AceAgent.ServiceModel
{
    [Route("/minerstatus")]
    public class MinerStatus : IReturn<MinerStatusResponse>
    {
        public string ID { get; set; }
    }
    public class MinerStatusResponse
    {
        public string Result { get; set; }
    }
}
