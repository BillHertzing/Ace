using ServiceStack;


namespace Ace.Agent.BaseServices
{
  #region BaseServices Initialization
  [Route("/BaseServicesInitialization")]
  public class InitializationRequest : IReturn<InitializationResponse>
  {
    public InitializationRequest() { }
  }
  public class InitializationResponse
  {
    public InitializationResponse() : this(new InitializationData()) { }
    public InitializationResponse(InitializationData initializationData)
    {
      InitializationData = initializationData;
    }
    public InitializationData InitializationData { get; set; }
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

  #region GetConfigurationDataRequest, GetConfigurationDataResponse, and route GetBaseServicesConfigurationData
  [Route("/GetBaseServicesConfigurationData")]
  public class GetConfigurationDataRequest : IReturn<GetConfigurationDataResponse>
  {
    public GetConfigurationDataRequest() : this(new ConfigurationData()) { }
    public GetConfigurationDataRequest(ConfigurationData ConfigurationData)
    {
      ConfigurationData = ConfigurationData;
    }
    public ConfigurationData ConfigurationData { get; set; }

  }
  public class GetConfigurationDataResponse
  {
    public GetConfigurationDataResponse() : this(new ConfigurationData()) { }
    public GetConfigurationDataResponse(ConfigurationData configurationData)
    {
      ConfigurationData = configurationData;
    }
    public ConfigurationData ConfigurationData { get; set; }
  }
  #endregion

  #region GetConfigurationDataRequest
  [Route("/GetBaseServicesUserData")]
  public class GetUserDataRequest : IReturn<GetUserDataResponse>
  {
    public GetUserDataRequest() : this(new UserData()) { }
    public GetUserDataRequest(UserData UserData)
    {
      UserData = UserData;
    }
    public UserData UserData { get; set; }

  }
  public class GetUserDataResponse
  {
    public GetUserDataResponse() : this(new UserData()) { }
    public GetUserDataResponse(UserData userData)
    {
      UserData = userData;
    }
    public UserData UserData { get; set; }
  }

  #endregion

}
