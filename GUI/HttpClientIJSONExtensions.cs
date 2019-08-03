using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ATAP.Utilities.IJSON;
namespace Ace.AceGUI.HttpClientExtensions {
    public static class HttpClientIJSONExtensions {

        public static Task PostJsonAsyncIJ(this HttpClient httpClient, string requestUri, object content, IJSON iJSONServiceImplementation)
    => httpClient.SendJsonAsyncIJSON(HttpMethod.Post, requestUri, content, iJSONServiceImplementation);

        public static Task<T> PostJsonAsyncIJ<T>(this HttpClient httpClient, string requestUri, object content, IJSON iJSONServiceImplementation)
    => httpClient.SendJsonAsyncIJSON<T>(HttpMethod.Post, requestUri, content, iJSONServiceImplementation);

        public static Task SendJsonAsyncIJSON(this HttpClient httpClient, HttpMethod method, string requestUri, object content, IJSON iJSONServiceImplementation)
    => httpClient.SendJsonAsyncIJSON<IgnoreResponse>(method, requestUri, content, iJSONServiceImplementation);

        public static async Task<T> SendJsonAsyncIJSON<T>(this HttpClient httpClient, HttpMethod method, string requestUri, object content, IJSON iJSONServiceImplementation) {

            //********************************************
            // I don't know how to do this next step
            // Get the IJSON service from the DI container
            // iJSONServiceImplementation = ????
            // So I made it a method parameter instead
            //********************************************
            var requestJson = iJSONServiceImplementation.Serialize(content);
            var response = await httpClient.SendAsync(new HttpRequestMessage(method, requestUri)
            {
                Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
            });

            // Make sure the call was successful before we
            // attempt to process the response content
            response.EnsureSuccessStatusCode();

            if (typeof(T) == typeof(IgnoreResponse))
            {
                return default;
            }
            else
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return iJSONServiceImplementation.Deserialize<T>(responseJson);
            }
        }

        class IgnoreResponse { }
    }

}

