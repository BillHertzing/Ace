using ATAP.Utilities.TypedGuids;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ATAP.Utilities.GeoLocationData {

public class GeoLocationData {
        public GeoLocationData():this(decimal.Zero,decimal.Zero,string.Empty){}

        public GeoLocationData(decimal latitude, decimal longitude, string address) {
            Latitude=latitude;
            Longitude=longitude;
            Address=address;
        }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Address { get; set; }

}

}