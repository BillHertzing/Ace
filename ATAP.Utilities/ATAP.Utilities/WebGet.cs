using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ATAP.Utilities.Http;
using Polly;
using Polly.Registry;
using Polly.Timeout;
namespace ATAP.Utilities.Http
{
    /*
    public class WebGetRegistryValue : IWebGetRegistryValue
    {
        Policy pol;
        HttpRequestMessage req;
        WebGetHttpResponseHowToHandle rsp;
        public WebGetRegistryValue(Policy pol, HttpRequestMessage req, WebGetHttpResponseHowToHandle rsp)
        {
            this.pol = pol;
            this.req = req;
            this.rsp = rsp;
        }
        //public WebGetRegistryValue(string pol, HttpRequestMessage req, WebGetHttpResponseCharacteristics rsp)
        //{
        //    this.pol = pol;
        //    this.req = req;
        //    this.rsp = rsp;
        //}
        public Policy Pol { get => pol; set => pol = value; }
        public HttpRequestMessage Req { get => req; set => req = value; }
        public WebGetHttpResponseHowToHandle Rsp { get => rsp; set => rsp = value; }

        //public ICloneable();
    }
}

public interface IWebGetRegistryValueBuilder
{
    WebGetRegistryValue Build();
}
public class WebGetRegistryValueBuilder
{
    Policy pol;
    HttpRequestMessage req;
    WebGetHttpResponseHowToHandle rsp;
    public WebGetRegistryValueBuilder()
    {
    }

    public WebGetRegistryValueBuilder AddRequest(HttpRequestMessage req)
    {
        this.req = req;
        return this;
    }
    public WebGetRegistryValueBuilder AddResponse(WebGetHttpResponseHowToHandle rsp)
    {
        this.rsp = rsp;
        return this;
    }
    public WebGetRegistryValueBuilder AddPolicy(Policy pol)
    {
        this.pol = pol;
        return this;
    }
    public WebGetRegistryValue Build()
    {
        return new WebGetRegistryValue(pol, req, rsp);
    }
    public static WebGetRegistryValueBuilder CreateNew()
    {
        return new WebGetRegistryValueBuilder();
    }

}
public class WebGetRegistryKey : ValueObject<WebGetRegistryKey>, IWebGetRegistryKey
    {
        string registryKey;
        public WebGetRegistryKey(string registryKey)
        {
            this.registryKey = registryKey;
        }
        public string RegistryKey { get => registryKey; set => registryKey = value; }

    }

    public class WebGetRegistry : IEnumerable<WebGetRegistryValue>, IWebGetRegistry
    {
        Dictionary<WebGetRegistryKey, WebGetRegistryValue> registry;
        public WebGetRegistry()
        {
            registry = new Dictionary<WebGetRegistryKey, WebGetRegistryValue>();
        }

        public Dictionary<WebGetRegistryKey, WebGetRegistryValue> Registry { get => registry; set => registry = value; }

        // to use a collection initializer, the collection must implement GetEnumerator in two different ways, 
        public IEnumerator<WebGetRegistryValue> GetEnumerator()
        {
            return registry.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            //forces use of the non-generic implementation on the Values collection
            return ((IEnumerable)registry.Values).GetEnumerator();
        }
        // to use a collection initializer, the collection must implement Add 
        public void Add(WebGetRegistryKey webGetRegistryKey, WebGetRegistryValue webGetRegistryValue)
        {
            registry.Add(webGetRegistryKey, webGetRegistryValue);
        }
        // to use a [] as an indexer the collection must implement the ITEM property
        public WebGetRegistryValue this[WebGetRegistryKey key] { get { return registry[key]; } set { registry[key] = value; } }
    }

    // see this article for "why" https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
    // This article was used as a basis for ensuring that there is only one instance of WebGet in an app: http://csharpindepth.com/Articles/General/Singleton.aspx
    public sealed class WebGet : IWebGet
    {
        static readonly Lazy<WebGet> lazy = new Lazy<WebGet>(() => new WebGet());
        HttpClient httpClient;
        List<HttpStatusCode> httpStatusCodesWorthRetrying;
        PolicyRegistry policyRegistry;
        WebGetRegistry webGetRegistry;
        WebGet()
        {
            httpClient = new HttpClient();
            policyRegistry = new PolicyRegistry();

            httpStatusCodesWorthRetrying = new List<HttpStatusCode> { HttpStatusCode.RequestTimeout, // 408
 HttpStatusCode.InternalServerError, // 500
 HttpStatusCode.BadGateway, // 502
 HttpStatusCode.ServiceUnavailable, // 503
 HttpStatusCode.GatewayTimeout // 504
 };
            TimeoutPolicy policyTimeout30Seconds = Policy.TimeoutAsync(new TimeSpan(0, 0, 30), TimeoutStrategy.Optimistic);
            // ToDo, add a BulkheadPolicy that pairs with an action to run if the bulkhead rejects and the queue is full
            policyRegistry.Add("policyBulkhead50Q150", Policy.BulkheadAsync(50, 150));
            policyRegistry.Add("policyTimeout30Seconds", Policy.TimeoutAsync(new TimeSpan(0, 0, 30), TimeoutStrategy.Optimistic));
            policyRegistry.Add("policyWaitAndRetry3TimesOnResponseContainsHttpStatusCodesWorthRetrying", Policy.HandleResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                                                                                                             .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt)));
            policyRegistry.Add("policyWaitAndRetry3TimesOnRequestException", Policy.Handle<HttpRequestException>()
                                                                                 .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt)));

            // PolicyWrap policyFullyResilient = Policy.Wrap(fallback, cache, retry, breaker, bulkhead, timeout);


            webGetRegistry = new WebGetRegistry();



        }
        public async Task<T> ASyncWebGetFast<T>(WebGetRegistryKey reqID)
        {
            WebGetRegistryValue registryValue = webGetRegistry[reqID];
            Policy p = registryValue.Pol;


            return await p.ExecuteAsync<T>(async () =>
            {
                HttpRequestMessage httpRequestMessageInner = HttpRequestMessageBuilder.CreateNew()
                                                 .AddMethod(registryValue.Req.Method)
                                                 .AddRequestUri(registryValue.Req.RequestUri)
                                                 // .AddHeaders(registryValue.Req.Headers);
                                                 .Build();
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessageInner);

                return httpResponseMessage.AsType<T>();
            });
        }
        public Task<T> WebGetFast<T>(WebGetRegistryKey reqID)
        {
            WebGetRegistryValue registryValue = webGetRegistry[reqID];
            Policy p = registryValue.Pol;


            return p.ExecuteAsync<T>(async () =>
            {
                HttpRequestMessage httpRequestMessageInner = HttpRequestMessageBuilder.CreateNew()
                                                 .AddMethod(registryValue.Req.Method)
                                                 .AddRequestUri(registryValue.Req.RequestUri)
                                                 // .AddHeaders(registryValue.Req.Headers);
                                                 .Build();
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessageInner);
                // the .AsType helper includes .Result, which turns this into a synchronous operation
                return httpResponseMessage.AsType<T>();
            });
        }
        public async Task<string> ASyncWebGetFast(WebGetRegistryKey reqID)
        {
            WebGetRegistryValue registryValue = webGetRegistry[reqID];
            Policy p = registryValue.Pol;
            HttpRequestMessage httpRequestMessageInner = HttpRequestMessageBuilder.CreateNew()
                                                             .AddMethod(registryValue.Req.Method)
                                                             .AddRequestUri(registryValue.Req.RequestUri)
                                                             // .AddHeaders(registryValue.Req.Headers);
                                                             .Build();
            return await p.ExecuteAsync(async () =>
            {
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessageInner);
                return httpResponseMessage.AsString();
            });
        }
        public async Task<string> ASyncWebGetFast(Policy p, HttpRequestMessage httpRequestMessage)
        {
            return await p.ExecuteAsync(async () =>
            {
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                return httpResponseMessage.AsString();
            });
        }
        public Task<T> AsyncWebGetSafe<T>(WebGetRegistryKey reqID)
        {
            throw new NotImplementedException("'AsyncWebGetSafe<T> not yet implemented");
            //if (string.IsNullOrWhiteSpace(requestUri))
            //{
            //    //ToDo: Better error handling on the throw
            //    throw new ArgumentException("message", nameof(requestUri));
            //}
            // ToDo Add validation tests
            // ToDo does the dictionary contain this key
            //return ASyncWebGetFast<T>(reqID);
        }
        public List<HttpStatusCode> HttpStatusCodesWorthRetrying { get => httpStatusCodesWorthRetrying; set => httpStatusCodesWorthRetrying = value; }
        public static WebGet Instance { get { return lazy.Value; } }
        public PolicyRegistry PolicyRegistry { get => policyRegistry; set => policyRegistry = value; }
        public WebGetRegistry WebGetRegistry { get => webGetRegistry; set => webGetRegistry = value; }
    }
    public class HttpRequestMessageBuilder : IHttpRequestMessageBuilder
    {
        string acceptHeader;
        string bearerToken;
        HttpContent content;
        // The HttpRequestHeaders are a System.Collections.Specialized.NameValueCollection() with a ADD(string,string) method
        HttpRequestHeaders httpRequestHeaders;
        HttpMethod method;
        Uri requestUri;
        public HttpRequestMessageBuilder()
        {
        }
        public HttpRequestMessageBuilder AddAcceptHeader(string acceptHeader)
        {
            this.acceptHeader = acceptHeader;
            return this;
        }
        public HttpRequestMessageBuilder AddBearerToken(string bearerToken)
        {
            this.bearerToken = bearerToken;
            return this;
        }
        public HttpRequestMessageBuilder AddContent(HttpContent content)
        {
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
        public HttpRequestMessageBuilder AddMethod(HttpMethod method)
        {
            this.method = method;
            return this;
        }
        public HttpRequestMessageBuilder AddRequestUri(Uri requestUri)
        {
            this.requestUri = requestUri;
            return this;
        }
        public HttpRequestMessage Build()
        {
            HttpRequestMessage hrm = new HttpRequestMessage(method, requestUri);
            if (content != default(HttpContent)) { hrm.Content = content; };
            // ToDo Figure out how to replace the entire HttpRequestHeaders
            // if (httpRequestHeaders != default(HttpRequestHeaders) { hrm.Headers = httpRequestHeaders; }
            if (bearerToken != default(string)) { hrm.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken); };
            //ToDo: further research, is .Headers.Accept.Clear() needed on a newly created HttpRequestMessage?
            hrm.Headers.Accept.Clear();
            if (acceptHeader != default(string)) { hrm.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeader)); };
            return hrm;
        }
        public static HttpRequestMessageBuilder CreateNew()
        {
            return new HttpRequestMessageBuilder();
        }
    }
    */
}
