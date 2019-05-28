using ServiceStack;
using ATAP.Utilities.GeoLocationData;
using ATAP.Utilities.LongRunningTasks;

namespace Ace.Agent.BaseServices {

    #region Lat/Lng To Address and reverse
    [Route("/LatLngToAddress")]
    public class GetAddressFromLatLongRequest : IReturn<GetAddressFromLatLongResponse> {
        public GetAddressFromLatLongRequest():this(new GeoLocationData()) {
        }

        public GetAddressFromLatLongRequest(GeoLocationData geoLocationData) {
            GeoLocationData=geoLocationData;
        }

        public GeoLocationData GeoLocationData { get; set; }
    }

    public class GetAddressFromLatLongResponse {
        public GetAddressFromLatLongResponse() : this(new LongRunningTaskStartupinfo()) {  }

        public GetAddressFromLatLongResponse(LongRunningTaskStartupinfo longRunningTaskStartupInfo) {
            LongRunningTaskStartupInfo=longRunningTaskStartupInfo;
        }

        // ToDo: return the LongRunningTaskStartupInfo
        public LongRunningTaskStartupinfo LongRunningTaskStartupInfo { get; set; }
        //public GeoLocationData GeoLocationData { get; set; }
    }

    [Route("/GetLatLongFromAddress")]
    public class GetLatLongFromAddressRequest : IReturn<GetLatLongFromAddressResponse> {
        public GetLatLongFromAddressRequest() : this(new GeoLocationData()) {
        }

        public GetLatLongFromAddressRequest(GeoLocationData geoLocationData) {
            GeoLocationData=geoLocationData;
        }

        public GeoLocationData GeoLocationData { get; set; }
    }
    public class GetLatLongFromAddressResponse {
        // ToDo: return the return the LongRunningTaskId
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
    #endregion

}
