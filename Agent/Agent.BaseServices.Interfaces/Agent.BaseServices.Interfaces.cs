using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
      // Resolve the gateway from the Base Services Data Gateways that handles this Service
      //The associate "Route To Gateway" will reurn a structure that identifies the gateway and gatewayEntry to use
      var route = "LatLngToAddress";
      var gatewayName = "GoogleMapsGeoCoding";
      var gatewayEntryName = "GeoCaching";
      var gateway = BaseServicesData.Gateways.Get(gatewayName);
      var gatewayEntry = gateway.GatewayEntries[gatewayEntryName];
      var completeURI = new Uri(gateway.BaseUri, gatewayEntry.RUri);
      var gatewayPolicy = gateway.DefaultPolicy;
      Type gatewayEntryReqDataPayloadType = gatewayEntry.ReqDataPayloadType;
      Type gatewayEntryRspDataPayloadType = gatewayEntry.RspDataPayloadType;
      // Get the cancellationtoken from the GatewayEntrymonitor for this specific GatewayEntryRequest
      CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
      CancellationToken cancellationToken = cancellationTokenSource.Token;
      var gatewayEntryRspDataPayload = Activator.CreateInstance(gatewayEntryRspDataPayloadType);
      var gatewayEntryReqDataPayload = Activator.CreateInstance(gatewayEntryReqDataPayloadType);
      Func<string, string, string, string> gatewayDecodeAPIKey = (aPIKeyEncoded, aPIKeyPassphrase, aPIKeyEncryptionIV) => { return $"{aPIKeyEncoded},{aPIKeyPassphrase},{aPIKeyEncryptionIV}"; };
      string completeUrl = completeURI.ToString().AddQueryParam("latlng",$"{request.Latitude},{request.Longitude}").AddQueryParam("key",$"{gatewayDecodeAPIKey("AKE","AKP","AKEIV")}");
      string msg = "notinitialized";
      while (!cancellationToken.IsCancellationRequested)
      {
        try
        {
          // Execute the following call according to the policy.
           gatewayPolicy.Execute( () =>
          {
            // This code is executed within the Policy 
            // Make a request and get a response
            msg = completeUrl.GetJsonFromUrl();

          });
        }
        catch (Exception e)
        {
          throw new Exception("caught something", e);
        }
      }

        return new LatLngToAddressRspPayload { Address = $"msg returned was {msg}" };
      }
      public async Task<AddressToLatLngRspPayload> Post(AddressToLatLngReqPayload request)
      {
      var baseServicesData = HostContext.TryResolve<BaseServicesData>();
      var gatewayName = "GoogleMapsGeoCoding";
      var gatewayEntryName = "GeoCaching";
      var gateway = baseServicesData.Gateways.Get(gatewayName);
      var gatewayEntry = gateway.GatewayEntries[gatewayEntryName];
      var completeURI = new Uri(gateway.BaseUri, gatewayEntry.RUri);
      var gatewayPolicy = gateway.DefaultPolicy;
      Type gatewayEntryReqDataPayloadType = gatewayEntry.ReqDataPayloadType;
      Type gatewayEntryRspDataPayloadType = gatewayEntry.RspDataPayloadType;
      // Get the cancellationtoken from the GatewayEntrymonitor for this specific GatewayEntryRequest
      CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
      CancellationToken cancellationToken = cancellationTokenSource.Token;
      var gatewayEntryRspDataPayload = Activator.CreateInstance(gatewayEntryRspDataPayloadType);
      var gatewayEntryReqDataPayload = Activator.CreateInstance(gatewayEntryReqDataPayloadType);
      Func<string, string, string, string> gatewayDecodeAPIKey = (aPIKeyEncoded, aPIKeyPassphrase, aPIKeyEncryptionIV) => { return $"{aPIKeyEncoded},{aPIKeyPassphrase},{aPIKeyEncryptionIV}"; };
      //string aKE=  
      string completeUrl = completeURI.ToString().AddQueryParam("address", $"{request.Address}").AddQueryParam("key", $"{gatewayDecodeAPIKey("AKE", "AKP", "AKEIV")}");
      string msg = "notinitialized";
      while (!cancellationToken.IsCancellationRequested)
      {
        try
        {
          // Execute the following call according to the policy.
          gatewayPolicy.Execute(async () =>
          {
            // This code is executed within the Policy 
            // Make a request and get a response
               msg = await completeUrl.GetStringFromUrlAsync();

          });
        }
        catch (Exception e)
        {
          throw new Exception("caught something", e);
        }
      }

      // Call the GUI Maps API to convert street address to a Latitude/Longitude
      return new AddressToLatLngRspPayload { Latitude = msg, Longitude = msg };
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

    BaseServicesData BaseServicesData { get; set; }
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
