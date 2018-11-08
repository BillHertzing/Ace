using System.Net;
using ServiceStack;
using ServiceStack.Configuration;

namespace Ace.Agent.BaseServices
{
    public class BaseServices : Service {
    #region Base Services Initialization
    public object Post(BaseServicesInitializationReqPayload request) {
          return new BaseServicesInitializationRspPayload { };
      }
    #endregion
    #region IsAlive
    public object Any(IsAliveReqPayload request)
      {
        return new IsAliveRspPayload { Result = $"Hello, the Name you sent me is {request.Name}" };
      }
    #endregion
    #region Lat/Lng To Address and reverse
    public object Post(LatLngToAddressReqPayload request)
      {
        // Call the GUI Maps API to convert Latitude/Longitude to a street address
        return new LatLngToAddressRspPayload { Address = $"Latitude sent was {request.Latitude}, Longitude sent was {request.Longitude} " };
      }
      public object Post(AddressToLatLngReqPayload request)
      {
        // Call the GUI Maps API to convert street address to a Latitude/Longitude
        return new AddressToLatLngRspPayload { Latitude = "0.0", Longitude = "1.0" };
      }
    #endregion
    #region Configuration Data
    public object Post(BaseServicesConfigurationDataReqPayload request)
    {
      //RedisCacheConnectionString = request.RedisCacheConnectionString;
      return new BaseServicesConfigurationDataRspPayload { RedisCacheConnectionString = "A HARDCODE sTRING"};
    }
    #endregion
    #region User Data
    public object Post(BaseServicesUserDataReqPayload request)
    {
      //GatewayNameString = request.GatewayNameString;
      //GatewayEntryAPIKeyString = request.GatewayEntryAPIKeyString;
      return new BaseServicesUserDataRspPayload { GatewayEntryAPIKeyString = "A HARDCODE sTRING" };
    }
    #endregion
    /*
             public object Any(BaseServicePutConfiguration request) {
                 if(!IsAuthenticated && AppSettings.Get("LimitRemoteControlToAuthenticatedUsers", false))
                 {
                     throw new HttpError(HttpStatusCode.Forbidden, "You must be authenticated and authorized to use remote control.");
                 }

                 return new PutConfigurationResponse { Result = "SUCCESS OR FAILURE" };
             }

             public IAppSettings AppSettings {
                 get; set;
             }
             */
  }
}
