using ServiceStack;


namespace Ace.Agent.BaseServices
{
  #region Base Services Initialization
  [Route("/BaseServicesInitialization")]
        public class BaseServicesInitializationReqPayload : IReturn<BaseServicesInitializationRspPayload>
  {
    //public string Name { get; set; }
  }
  public class BaseServicesInitializationRspPayload
  {
    //public string Result { get; set; }
  }
  #endregion

  #region IsAlive
  [Route("/isAlive")]
    [Route("/isAlive/{Name}")]
    public class IsAliveReqPayload : IReturn<IsAliveRspPayload>
    {
        public string Name { get; set; }
    }

    public class IsAliveRspPayload
    {
        public string Result { get; set; }
    }
  #endregion IsAlive

  #region Lat/Lng To Address and reverse
  [Route("/LatLngToAddress")]
  public class LatLngToAddressReqPayload : IReturn<LatLngToAddressRspPayload>
  {
    public string Latitude { get; set; }
    public string Longitude { get; set; }
  }

  public class LatLngToAddressRspPayload
  {
    public string Address { get; set; }
  }

  [Route("/AddressToLatLng")]
  public class AddressToLatLngReqPayload : IReturn<AddressToLatLngRspPayload>
  {
    public string Address { get; set; }
  }
  public class AddressToLatLngRspPayload
  {
      public string Latitude { get; set; }
      public string Longitude { get; set; }
    }
  #endregion

  #region Configuration Data
  [Route("/BaseServicesConfigurationData")]
  public class BaseServicesConfigurationDataReqPayload : IReturn<BaseServicesConfigurationDataRspPayload>
  {
    public string RedisCacheConnectionString { get; set; }
  }
  public class BaseServicesConfigurationDataRspPayload
  {
    public string RedisCacheConnectionString { get; set; }
  }

  #endregion

  #region User Data
  [Route("/BaseServicesUserData")]
  public class BaseServicesUserDataReqPayload : IReturn<BaseServicesUserDataRspPayload>
  {
    public string GatewayNameString { get; set; }
    public string GatewayEntryAPIKeyString { get; set; }
  }
  public class BaseServicesUserDataRspPayload
  {
    public string GatewayNameString { get; set; }
    public string GatewayEntryAPIKeyString { get; set; }
  }
  #endregion

}
