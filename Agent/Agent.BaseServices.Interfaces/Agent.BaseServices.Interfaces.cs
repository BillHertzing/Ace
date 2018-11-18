using System;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Logging;

namespace Ace.Agent.BaseServices {
    public class BaseServices : Service {
        static readonly ILog Log = LogManager.GetLogger(typeof(BaseServices));

    #region IsAlive
        public object Any(IsAliveReqPayload request) {
            return new IsAliveRspPayload { Result = $"Hello, the Name you sent me is {request.Name}" };
        }
    #endregion

    #region BaseServices Initialization
    public object Post(BaseServicesInitializationReqPayload request) {
        return new BaseServicesInitializationRspPayload { };
    }
    #endregion
    #region User Data
    public object Post(BaseServicesUserDataReqPayload request) {
        //GatewayNameString = request.GatewayNameString;
        //GatewayEntryAPIKeyString = request.GatewayEntryAPIKeyString;
        return new BaseServicesUserDataRspPayload { GatewayEntryAPIKeyString = "A HARDCODE sTRING" };
    }
    #endregion

    #region Lat/Lng To Address and reverse
    public object Post(LatLngToAddressReqPayload request) {
        Log.Debug("Entering Post LatLngToAddressReqPayload request handler");
        // Resolve the GatewayEntry from the BaseServices Data Gateways that handles this Service
        // The associate "Route To Gateway" will return a structure that identifies the gateway and gatewayEntry to use
        var baseServicesData = HostContext.TryResolve<BaseServicesData>();
        var gatewayEntryName = "ReverseGeoCode";
        var gatewayName = "GoogleMaps";
        var gateway = baseServicesData.Gateways
            .Get(gatewayName);
        var gatewayEntry = gateway.GatewayEntries[gatewayEntryName];
        var completeURI = new Uri(gateway.BaseUri, gatewayEntry.RUri);
        var gatewayPolicy = gateway.DefaultPolicy;
        Type gatewayEntryReqDataPayloadType = gatewayEntry.ReqDataPayloadType;
        Type gatewayEntryRspDataPayloadType = gatewayEntry.RspDataPayloadType;
        // ToDo replace DefaultAPIKey auth with a more robust and extendable solution
        var defaultAPIKey = gateway.DefaultAPIKey;
        // Get the cancellationtoken from the GatewayEntryMonitor for this specific GatewayEntryRequest
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        var gatewayEntryRspDataPayload = Activator.CreateInstance(gatewayEntryRspDataPayloadType);
        var gatewayEntryReqDataPayload = Activator.CreateInstance(gatewayEntryReqDataPayloadType);

        //string completeUrl = completeURI.ToString().AddQueryParam("latlng",$"{request.Latitude},{request.Longitude}").AddQueryParam("key",$"{gatewayDecodeAPIKey("AKE","AKP","AKEIV")}");
        string completeUrl = completeURI.ToString()
            .AddQueryParam("latlng", $"{request.Latitude},{request.Longitude}")
            .AddQueryParam("key", defaultAPIKey);

        //string completeUrl = completeURI.ToString().AddQueryParam("latlng", $"{request.Latitude},{request.Longitude}");
        string msg = "notinitialized";
        try {
        // See servicestack using Interfaces for typed DTOs and making this apply to all requests
        var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36";
        var referer = "https://localhost/";
        msg = completeUrl.GetJsonFromUrl(requestFilter: req => {
          req.UserAgent = userAgent;
          req.Referer = referer;
        });
        } catch(Exception e) {
            throw new Exception("caught something", e);
        }
        Task<string> msgTask;
        /*
       * Log.Debug($"Calling the Gateway:GatewayEntry Action {gateway.Name}:{gatewayEntry.Name} with completeUrl= {completeUrl}");
      msg = gatewayPolicy.ExecuteAsync(async (url, ct) => {
        return  url.GetJsonFromUrlAsync();
      }, cancellationToken);
      */
        /*
        while (!cancellationToken.IsCancellationRequested)
        {
        try
        {
        // Execute the following call according to the policy.
        gatewayPolicy.ExecuteAsync(async () =>
        {
            Log.Debug("in Policy executeAsync");
            // This code is executed within the Policy 
            // Make a request and get a response
            Log.Debug("calling GetJsonFromUrlAsync");
        
        msgTask = completeUrl.GetJsonFromUrlAsync();
        Log.Debug("returned from GetJsonFromUrlAsync");
        });
        }
        catch (Exception e)
        {
        throw new Exception("caught something", e);
        }

        }
      */
        Log.Debug("returned from Calling the Gateway:GatewayEntry Action");

        return new LatLngToAddressRspPayload { Address = $"msg returned was {msg}" };
    }
        public async Task<AddressToLatLngRspPayload> Post(AddressToLatLngReqPayload request) {
            var baseServicesData = HostContext.TryResolve<BaseServicesData>();
            var gatewayName = "GoogleMapsGeoCoding";
            var gatewayEntryName = "GeoCaching";
            var gateway = baseServicesData.Gateways
                .Get(gatewayName);
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
            Func<string, string, string, string> gatewayDecodeAPIKey = (aPIKeyEncoded, aPIKeyPassphrase, aPIKeyEncryptionIV) => {
                return $"{aPIKeyEncoded},{aPIKeyPassphrase},{aPIKeyEncryptionIV}";
            };
            //string aKE=  
            string completeUrl = completeURI.ToString()
                .AddQueryParam("address", $"{request.Address}")
                .AddQueryParam("key", $"{gatewayDecodeAPIKey("AKE", "AKP", "AKEIV")}");
            string msg = "notinitialized";
            while(!cancellationToken.IsCancellationRequested) {
                try {
                    // Execute the following call according to the policy.
                    gatewayPolicy.Execute(async() => {
                        // This code is executed within the Policy 
                        // Make a request and get a response
                        msg = await completeUrl.GetStringFromUrlAsync();
                    });
                } catch(Exception e) {
                    throw new Exception("caught something", e);
                }
            }

            // Call the GUI Maps API to convert street address to a Latitude/Longitude
            return new AddressToLatLngRspPayload { Latitude = msg, Longitude = msg };
        }
    #endregion
    #region Configuration Data
    public object Post(BaseServicesConfigurationDataReqDTO request) {
        var baseServicesData = HostContext.TryResolve<BaseServicesData>();
        baseServicesData.RedisCacheConnectionString = request.RedisCacheConnectionString;
        baseServicesData.MySqlConnectionString = request.MySqlConnectionString;
        return new BaseServicesConfigurationDataRspDTO { RedisCacheConnectionString =
            baseServicesData.RedisCacheConnectionString
            , MySqlConnectionString = baseServicesData.MySqlConnectionString
        };
    }
        public object Get(BaseServicesConfigurationDataReqDTO request) {
            var baseServicesData = HostContext.TryResolve<BaseServicesData>();
            //RedisCacheConnectionString = request.RedisCacheConnectionString;
            return new BaseServicesConfigurationDataRspDTO { RedisCacheConnectionString =
                baseServicesData.RedisCacheConnectionString
                , MySqlConnectionString = baseServicesData.MySqlConnectionString
            };
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
        */    }
}
