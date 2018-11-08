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
    string AcceptHeader {get; set;}

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
  public interface IGatewayEntry
  {
    string Name { get; }
    Type RequestDataPayload { get; set; }
    Type ResponseDataPayload { get; set; }
    string RUriStr { get; set; }
  }
  public class GatewayEntry : IGatewayEntry
  {
    string name;
    public GatewayEntry(string name)
    {
      this.name = name;
    }
    public string Name { get => name; }
    public Type RequestDataPayload { get; set; }

    public Type ResponseDataPayload { get; set; }

    public string RUriStr { get; set; }


  }

  public interface IGateway
  {
    Uri BaseUri { get; }
    Policy DefaultPolicy { get; set; }
    string Name { get; }
    Dictionary<string, IGatewayEntry> GatewayEntries { get; set; }
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
    
      public string GetJsonFromUrl(IGatewayEntry entry, Action<HttpWebRequest> requestFilter = null,
  Action<HttpWebResponse> responseFilter = null)
    {
      string r = new Uri(BaseUri, entry.RUriStr).AbsolutePath.GetJsonFromUrl(requestFilter, responseFilter);
      return r;
      }
    public  Task<string> PostJsonToUrlAsync(IGatewayEntry entry, string json, Action<HttpWebRequest> requestFilter = null,
Action<HttpWebResponse> responseFilter = null)
    {
      return new Uri(BaseUri, entry.RUriStr).AbsolutePath.PostJsonToUrlAsync(json, requestFilter, responseFilter);
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

        public GatewayBuilder AddName(string _name) {
      name = _name;
      return this;
    }

    public GatewayBuilder AddBaseUri(Uri _baseUri)
    {
      baseUri = _baseUri;
      return this;
    }


    public GatewayBuilder()
    {
    }

    public IGateway Build()
    {
      Gateway g = new Gateway(name,baseUri);
      return g;
    }
    public static GatewayBuilder CreateNew()
    {
      return new GatewayBuilder();
    }
  }

  public interface IGateways
  {

  }

  public class Gateways : IGateways
  {
    public Gateways()
    {

    }
  }
  public interface IMultiGatewaysBuilder
  {
    IGateways Build();
  }

  public class MultiGatewaysBuilder : IMultiGatewaysBuilder
  {
    string name;
    Uri baseUri;
    Policy defaultPolicy;
    Dictionary<string, IGatewayEntry> gatewayEntries;

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
