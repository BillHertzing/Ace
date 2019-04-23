using ServiceStack;
using System;
using ATAP.Utilities.LongRunningTasks;

namespace Ace.Agent.BaseServices {

    #region Lat/Lng To Address and reverse
    [Route("/LatLngToAddress")]
    public class LatLngToAddressReqPayload : IReturn<LatLngToAddressRspPayload> {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class LatLngToAddressRspPayload {
        // ToDo: return the return the LongRunningTaskId
        public string Address { get; set; }
    }

    [Route("/AddressToLatLng")]
    public class AddressToLatLngReqPayload : IReturn<AddressToLatLngRspPayload> {
        public string Address { get; set; }
    }
    public class AddressToLatLngRspPayload {
        // ToDo: return the return the LongRunningTaskId
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
    #endregion

}
