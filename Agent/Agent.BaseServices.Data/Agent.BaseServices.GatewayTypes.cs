using System;
using System.Net;
using System.Threading;
using ServiceStack;
using ServiceStack.Configuration;

namespace Ace.Agent.BaseServices
{
  // Type definitions for the gatewayentry's ReqType and RspType
  public class GatewayEntryTypes
  {
  }
  public class Base_GoogleMapsGeoCoding_ReverseGeoCode_ReqDTO
  {
    public string latlng { get; set; }
  }
    public class Base_GoogleMapsGeoCoding_ReverseGeoCode_RspDTO
  {

    public class Rootobject
    {
      public Result[] results { get; set; }
      public string status { get; set; }
    }

    public class Result
    {
      public Address_Components[] address_components { get; set; }
      public string formatted_address { get; set; }
      public Geometry geometry { get; set; }
      public string place_id { get; set; }
      public string[] types { get; set; }
    }

    public class Geometry
    {
      public Location location { get; set; }
      public string location_type { get; set; }
      public Viewport viewport { get; set; }
    }

    public class Location
    {
      public float lat { get; set; }
      public float lng { get; set; }
    }

    public class Viewport
    {
      public Northeast northeast { get; set; }
      public Southwest southwest { get; set; }
    }

    public class Northeast
    {
      public float lat { get; set; }
      public float lng { get; set; }
    }

    public class Southwest
    {
      public float lat { get; set; }
      public float lng { get; set; }
    }

    public class Address_Components
    {
      public string long_name { get; set; }
      public string short_name { get; set; }
      public string[] types { get; set; }
    }

  }
}
