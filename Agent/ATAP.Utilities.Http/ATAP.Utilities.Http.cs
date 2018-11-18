using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ServiceStack.Text;
using ServiceStack;
using Swordfish.NET.Collections;

namespace ATAP.Utilities.Http
{

  public sealed class SingletonHttpClient {
    static readonly Lazy<SingletonHttpClient> lazy = new Lazy<SingletonHttpClient>(() => new SingletonHttpClient());
    HttpClient httpClient;

    SingletonHttpClient() {
      httpClient = new HttpClient();
    }

    static SingletonHttpClient SingleInstanceOfHttpClient { get { return lazy.Value; } }

    public static async Task<HttpResponseMessage> AsyncFetch(Policy policy, HttpRequestMessage httpRequestMessage) {
      return await policy.ExecuteAsync(async () => {
        return await SingletonHttpClient.SingleInstanceOfHttpClient.httpClient.SendAsync(httpRequestMessage);
      });
    }
  }

  public interface IHttpRequestMessageBuilder {
    HttpRequestMessage Build();
  }

  public class HttpRequestMessageBuilder : IHttpRequestMessageBuilder {

    HttpContent content;
    // The HttpRequestHeaders are a System.Collections.Specialized.NameValueCollection() with a ADD(string,string) method
    HttpRequestHeaders httpRequestHeaders;
    HttpMethod method;


    public HttpRequestMessageBuilder() {
    }

    public HttpRequestMessageBuilder AddAcceptHeader(string acceptHeader) {
      AcceptHeader = acceptHeader;
      return this;
    }
    public HttpRequestMessageBuilder AddBearerToken(string bearerToken) {
      BearerToken = bearerToken;
      return this;
    }
    public HttpRequestMessageBuilder AddContent(HttpContent content) {
      this.content = content;
      return this;
    }
    // Figure out some way to replace all of the headers with a new 
    //public HttpRequestMessageBuilder AddHeaders(HttpRequestMessage httpRequestMessage)
    //{
    //    foreach(var h in httpRequestMessage.Headers) { switch(h.Key) { defaulthttpRequestHeaders[h.Key] = h.Value; break; } }
    //    httpRequestHeaders = httpRequestHeaders;
    //    return this;
    //}
    public HttpRequestMessageBuilder AddMethod(HttpMethod method) {
      this.method = method;
      return this;
    }
    public HttpRequestMessageBuilder AddRequestUri(Uri requestUri) {
      RequestUri = requestUri;
      return this;
    }
    public HttpRequestMessage Build() {
      HttpRequestMessage hrm = new HttpRequestMessage(method, RequestUri);
      if (content != default(HttpContent)) {
        hrm.Content = content;
      };
      // ToDo Figure out how to replace the entire HttpRequestHeaders
      // if (httpRequestHeaders != default(HttpRequestHeaders) { hrm.Headers = httpRequestHeaders; }
      if (BearerToken != default(string)) {
        hrm.Headers.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);
      };
      //ToDo: further research, is .Headers.Accept.Clear() needed on a newly created HttpRequestMessage?
      hrm.Headers.Accept.Clear();
      if (AcceptHeader != default(string)) {
        hrm.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(AcceptHeader));
      };
      return hrm;
    }
    public static HttpRequestMessageBuilder CreateNew() {
      return new HttpRequestMessageBuilder();
    }
    string AcceptHeader { get; set; }

    string BearerToken { get; set; }
    Uri RequestUri { get; set; }
  }

  public abstract class WebGet<TResult> {
    public WebGet(Policy policy, HttpRequestMessage httpRequestMessage) {
      Policy = policy;
      HttpRequestMessage = httpRequestMessage;
    }

    public virtual Task<TResult> FetchAsync(string str) {
      throw new NotImplementedException("Abstract class method called");
    }

    public virtual async Task<HttpResponseMessage> FetchAsync(Policy policy, HttpRequestMessage httpRequestMessage) {
      return await SingletonHttpClient.AsyncFetch(policy, httpRequestMessage);
    }

    public virtual async Task<HttpResponseMessage> GetAsync() {
      return await SingletonHttpClient.AsyncFetch(Policy, HttpRequestMessage);
    }

    public HttpRequestMessage HttpRequestMessage { get; set; }

    public Policy Policy { get; set; }
  }

  public class GenericWebGet : WebGet<HttpResponseMessage> {
    public GenericWebGet(Policy policy, HttpRequestMessage httpRequestMessage) : base(policy, httpRequestMessage) {
    }
  }

  // Create the concept of a Gateway, a collection of related API entries and a base URL
  #region GatewayEntry
  public interface IGatewayEntry
  {
    string Name { get; }
    Type ReqDataPayloadType { get; set; }
    Type RspDataPayloadType { get; set; }
    string RUri { get;  }
    // ToDo replace APIKEy auth with a more robust and extendable solution
    string APIKey { get; set; }
  }
  public class GatewayEntry : IGatewayEntry
  {
    string name;
    string rUri;
    public GatewayEntry(string name, string rUri)
    {
      this.name = name;
      this.rUri = rUri;
    }
    public string Name { get => name; }
    public Type ReqDataPayloadType { get; set; }

    public Type RspDataPayloadType { get; set; }

    public string RUri { get=>rUri;  }
    // ToDo replace APIKEy auth with a more robust and extendable solution
    public string APIKey { get; set; }

  }
  public interface IGatewayEntryBuilder
  {
    GatewayEntry Build();
  }
  public class GatewayEntryBuilder : IGatewayEntryBuilder
  {
    string name;
    string rUri;
    Type reqDataPayloadType;
    Type rspDataPayloadType;
    // ToDo replace APIKEy auth with a more robust and extendable solution
    string aPIKey;

    public GatewayEntry Build() {
      GatewayEntry ge = new GatewayEntry(name, rUri);
      ge.ReqDataPayloadType = reqDataPayloadType;
      ge.RspDataPayloadType = rspDataPayloadType;
      ge.APIKey = aPIKey;
      return ge;
    }
    public GatewayEntryBuilder()
    {
    }
    public GatewayEntryBuilder AddName(string name)
    {
      this.name = name;
      return this;
    }
    public GatewayEntryBuilder AddRUri(string rUri)
    {
      this.rUri = rUri;
      return this;
    }
    public GatewayEntryBuilder AddReqDataPayloadType(Type reqDataPayloadType)
    {
      this.reqDataPayloadType = reqDataPayloadType;
      return this;
    }
    public GatewayEntryBuilder AddRspDataPayloadType(Type rspDataPayloadType)
    {
      this.rspDataPayloadType = rspDataPayloadType;
      return this;
    }
    // ToDo replace APIKEy auth with a more robust and extendable solution
    public GatewayEntryBuilder AddAPIKey(string aPIKey)
    {
      this.aPIKey = aPIKey;
      return this;
    }
    public static GatewayBuilder CreateNew()
    {
      return new GatewayBuilder();
    }
  }
  #endregion

  #region Gateway
  public interface IGateway
  {
    Uri BaseUri { get; }
    Policy DefaultPolicy { get; set; }
    string Name { get; }
    Dictionary<string, IGatewayEntry> GatewayEntries { get; set; }
    // ToDo replace DefaultAPIKEy auth with a more robust and extendable solution
    string DefaultAPIKey { get; set; }

  }
  public class Gateway : IGateway
  {
    string name;
    Uri baseUri;
    
    public Gateway(string name, Uri baseUrI)
    {
      this.name = name;
      this.baseUri = baseUrI;
    }
    public string Name { get => name; }
    public Uri BaseUri { get => baseUri; }
    public Policy DefaultPolicy { get; set; }
    public Dictionary<string, IGatewayEntry> GatewayEntries { get; set; }

    // ToDo replace DefaultAPIKEy auth with a more robust and extendable solution
    public string DefaultAPIKey { get; set; }

    public string GetJsonFromUrl(IGatewayEntry entry, Action<HttpWebRequest> requestFilter = null,
Action<HttpWebResponse> responseFilter = null)
    {
      string r = new Uri(BaseUri, entry.RUri).AbsolutePath.GetJsonFromUrl(requestFilter, responseFilter);
      return r;
    }
    public Task<string> PostJsonToUrlAsync(IGatewayEntry entry, string json, Action<HttpWebRequest> requestFilter = null,
Action<HttpWebResponse> responseFilter = null)
    {
      return new Uri(BaseUri, entry.RUri).AbsolutePath.PostJsonToUrlAsync(json, requestFilter, responseFilter);
    }
  }

  public interface IGatewayBuilder
  {
    IGateway Build();
  }

  public class GatewayBuilder : IGatewayBuilder
  {
    string name;
    Uri baseUri;
    Policy defaultPolicy;
    Dictionary<string, IGatewayEntry> gatewayEntries;
    // ToDo replace DefaultAPIKEy auth with a more robust and extendable solution
    string defaultAPIKey;

    public GatewayBuilder AddName(string name) {
      this.name = name;
      return this;
    }

    public GatewayBuilder AddBaseUri(Uri baseUri)
    {
      this.baseUri = baseUri;
      return this;
    }

    public GatewayBuilder AddDefaultPolicy(Policy defaultPolicy)
    {
      this.defaultPolicy = defaultPolicy;
      return this;
    }
    public GatewayBuilder AddGatewayEntries(Dictionary<string, IGatewayEntry> gatewayEntries)
    {
      this.gatewayEntries = gatewayEntries;
      return this;
    }
    public GatewayBuilder AddGatewayEntry(IGatewayEntry gatewayEntry)
    {
      this.gatewayEntries.Add(gatewayEntry.Name, gatewayEntry);
      return this;
    }
    // ToDo replace DefaultAPIKEy auth with a more robust and extendable solution
    public GatewayBuilder AddDefaultAPIKey(string defaultAPIKey)
    {
      this.defaultAPIKey = defaultAPIKey;
      return this;
    }
    public GatewayBuilder()
    {
      gatewayEntries = new Dictionary<string, IGatewayEntry>();
    }

    public IGateway Build()
    {
      Gateway g = new Gateway(name, baseUri);
      g.DefaultPolicy = this.defaultPolicy;
      g.GatewayEntries = this.gatewayEntries;
      g.DefaultPolicy = this.defaultPolicy;
      g.DefaultAPIKey = this.defaultAPIKey;
      // ToDo replace DefaultAPIKEy auth with a more robust and extendable solution
      return g;
    }
    public static GatewayBuilder CreateNew()
    {
      return new GatewayBuilder();
    }
  }
  #endregion

  #region Gateways
  public interface IGateways
  {
    void Add(string Key, IGateway gateway);
    IGateway Get(string Key);
  }

  public class Gateways : IGateways
  {
    ConcurrentObservableDictionary<string, IGateway> gateways;
    public Gateways()
    {
      gateways = new ConcurrentObservableDictionary<string, IGateway>(); 
   }
    public void Add(string Key, IGateway gateway) { gateways.Add(Key, gateway); }
    public IGateway Get(string Key) { return gateways[Key]; }
  }
  public interface IMultiGatewaysBuilder
  {
    IGateways Build();
  }

  public class MultiGatewaysBuilder : IMultiGatewaysBuilder
  {


    public MultiGatewaysBuilder AddDictionarySettings(Dictionary<string, IGateway> map)
    {
      throw new NotImplementedException();
      return this;
    }
    public MultiGatewaysBuilder AddEnvironmentalVariables()
    {
      throw new NotImplementedException();
      return this;
    }
    public MultiGatewaysBuilder AddEnvironmentalVariables(string tier)
    {
      throw new NotImplementedException();
      return this;
    }
    public MultiGatewaysBuilder AddTextFile(string path)
    {
      throw new NotImplementedException();
      return this;
    }
    public MultiGatewaysBuilder AddTextFile(string path, string delimeter)
    {
      throw new NotImplementedException();
      return this;
    }
    public MultiGatewaysBuilder AddTextFile(string path, string delimeter, string tier)
    {
      throw new NotImplementedException();
      return this;
    }

    public MultiGatewaysBuilder(string tier = null)
    {
    }

    public IGateways Build()
    {
      Gateways mg = new Gateways();
      return mg;
    }
    public static MultiGatewaysBuilder CreateNew()
    {
      return new MultiGatewaysBuilder();
    }
  }
  #endregion

  #region GatewayEntryMonitor
  public interface IGatewayEntryMonitor {
    string Name { get; }
  }
  public class GatewayEntryMonitor : IGatewayEntryMonitor
  {
    string name;
    public string Name { get => name; }
    public GatewayEntryMonitor(string name)
    {
      this.name = name;
    }
  }

  public interface IGatewayEntryMonitorBuilder
  {
    GatewayEntryMonitor Build();
  }

  public class GatewayEntryMonitorBuilder : IGatewayEntryMonitorBuilder {
    string name;

        public GatewayEntryMonitorBuilder()
  {
  }
    public IGatewayEntryMonitorBuilder AddName(string name)
    {
      this.name = name;
      return this;
    }
    public static GatewayEntryMonitorBuilder CreateNew()
    {
      return new GatewayEntryMonitorBuilder();
    }

    public GatewayEntryMonitor Build()
  {
    GatewayEntryMonitor mg = new GatewayEntryMonitor(name);
    return mg;
  }
  }
  #endregion

  #region GatewayMonitor
  public interface IGatewayMonitor
  {
    string Name { get; }
    Dictionary<string, IGatewayEntryMonitor> GatewayEntryMonitors { get; set; }
  }
  public class GatewayMonitor : IGatewayMonitor
  {
    string name;
    public string Name { get => name; }
    public GatewayMonitor(string name)
    {
      this.name = name;
    }
    public Dictionary<string, IGatewayEntryMonitor> GatewayEntryMonitors { get; set; }
  }

  public interface IGatewayMonitorBuilder
  {
    GatewayMonitor Build();
    IGatewayMonitorBuilder AddName(string name);
    IGatewayMonitorBuilder AddGatewayEntryMonitor(GatewayEntryMonitor gatewayEntryMonitor);
  }

  public class GatewayMonitorBuilder : IGatewayMonitorBuilder
  {
    string name;
    Dictionary<string, IGatewayEntryMonitor> gatewayEntryMonitors;
    public GatewayMonitorBuilder() {
      gatewayEntryMonitors = new Dictionary<string, IGatewayEntryMonitor>();
      }
    public IGatewayMonitorBuilder AddName(string name)
    {
      this.name = name;
      return this;
    }
    public IGatewayMonitorBuilder AddGatewayEntryMonitor(GatewayEntryMonitor gatewayEntryMonitor)
    {
      gatewayEntryMonitors.Add(gatewayEntryMonitor.Name, gatewayEntryMonitor);
      return this;
    }
    public GatewayMonitor Build()
    {
      GatewayMonitor gm = new GatewayMonitor(name);
      gm.GatewayEntryMonitors = gatewayEntryMonitors;
      return gm;
    }
    public static GatewayMonitorBuilder CreateNew()
    {
      return new GatewayMonitorBuilder();
    }
  }
  #endregion

  #region GatewayMonitors
  public interface IGatewayMonitors
  {
    Dictionary<string, GatewayMonitor> GatewayMonitorColl { get; set; }
  }
  public class GatewayMonitors : IGatewayMonitors
  {
    string name;
    public string Name { get=>name; }
    public GatewayMonitors(string name)
    {
      this.name = name;
      GatewayMonitorColl = new Dictionary<string, GatewayMonitor>(); 
    }
    public Dictionary<string, GatewayMonitor> GatewayMonitorColl { get; set; }
  }

  public interface IGatewayMonitorsBuilder
  {
    GatewayMonitors Build();
  }

  public class GatewayMonitorsBuilder : IGatewayMonitorsBuilder
  {
    string name;
    Dictionary<string, GatewayMonitor> gatewayMonitors;
    public GatewayMonitorsBuilder(string name)
    {
      this.name = name;
      gatewayMonitors = new Dictionary<string, GatewayMonitor>();
    }
    public IGatewayMonitorsBuilder AddGatewayMonitor(GatewayMonitor gatewayMonitor)
    {
      gatewayMonitors.Add(gatewayMonitor.Name, gatewayMonitor);
      return this;
    }
    public GatewayMonitors Build()
    {
      GatewayMonitors gm = new GatewayMonitors(name);
      return gm;
    }
    public static GatewayMonitorsBuilder CreateNew(string name)
    {
      return new GatewayMonitorsBuilder(name);
    }
  }
  #endregion
  /*
      // Within an application, there should only be one static instance of a HTTPClient. This class provides that, and a set of static async tasks to interact with it.
      public interface IWebGet
      {
          Task<T> ASyncWebGetFast<T>(IWebGetRegistryKey reqID);
          Task<string> ASyncWebGetFast(IWebGetRegistryKey reqID);
          Task<string> ASyncWebGetFast(Policy p, HttpRequestMessage httpRequestMessage);
          Task<T> AsyncWebGetSafe<T>(IWebGetRegistryKey reqID);
          Task<T> WebGetFast<T>(IWebGetRegistryKey reqID);

          List<HttpStatusCode> HttpStatusCodesWorthRetrying { get; set; }
          PolicyRegistry PolicyRegistry { get; set; }
          IWebGetRegistry WebGetRegistry { get; set; }
      }
      public interface IWebGetRegistryValue
      {
          Policy Pol { get; set; }
          HttpRequestMessage Req { get; set; }
          WebGetHttpResponseHowToHandle Rsp { get; set; }
      }
      public interface IWebGetRegistryKey
      {
          string RegistryKey { get; set; }
      }
      public interface IHttpRequestMessageBuilder
      {
          HttpRequestMessage Build();
      }

      public interface IWebGetRegistry
      {
          void Add(IWebGetRegistryKey webGetRegistryKey, IWebGetRegistryKey webGetRegistryValue);

          Dictionary<IWebGetRegistryKey, IWebGetRegistryKey> Registry { get; set; }
      }
      */


}
