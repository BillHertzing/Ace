using ServiceStack;
using ServiceStack.Logging;

namespace Ace.AceService.MinerServices.Models
{

    [Route("/StartMiner")]
    [Route("/StartMiner/{ID}")]
    public class StartMinerRequest : IReturn<StartMinerResponse>
    {
        public string ID { get; set; }
    }
    public class StartMinerResponse
    {
        public string Result { get; set; }
    }
    [Route("/StopMiner")]
    [Route("/StopMiner/{ID}")]
    public class StopMinerRequest : IReturn<StopMinerResponse>
    {
        public string ProcessName { get; set; }
        public string ID { get; set; }
    }
    public class StopMinerResponse
    {
        public string Result { get; set; }
    }
    [Route("/ListMiner")]
    [Route("/ListMiner/{Kind};{Version}")]
    public class ListMinerRequest : IReturn<ListMinersResponse>
    {
        public string ID { get; set; }
    }
    public class ListMinersResponse
    {
        public int ProcessID { get; set; }
    }
  /*
    [Route("/StatusMiners")]
    [Route("/StatusMiner/{ID}")]
    public class StatusMinerRequest : IReturn<StatusMinersResponse>
    {
        public string ID { get; set; }
    }
    public class StatusMinersResponse
    {
        public MinerStatus Result { get; set; }
    }
  
    [Route("/TuneMinerGPU")]
    [Route("/TuneMinerGPU/{ID}")]
    public class TuneMinerGPURequest : IReturn<TuneMinerGPUResponse>
    {
        public string ID { get; set; }
    }
    public class TuneMinerGPUResponse
    {
        public TuneMinerGPUsResult[] Result { get; set; }
    }
*/
}
