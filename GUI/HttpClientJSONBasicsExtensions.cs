using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ace.AceGUI.HttpClientExtenssions {
    public static class HttpClientSSExtensions {


        // The "semantic type class" ID<T> fails to serialize or deserialize in Core 3.0's  nor Newtonsoft
        // The ServiceStack JSON serializer library will successfully serialize and deserialize them
        // The following HttpClient extensions use the ServiceStack serialzers and deserializers. 

        public static Task PostJsonAsyncSS(this HttpClient httpClient, string requestUri, object content)
    => httpClient.SendJsonAsyncSS(HttpMethod.Post, requestUri, content);

        public static Task<T> PostJsonAsyncSS<T>(this HttpClient httpClient, string requestUri, object content)
    => httpClient.SendJsonAsyncSS<T>(HttpMethod.Post, requestUri, content);

        public static Task SendJsonAsyncSS(this HttpClient httpClient, HttpMethod method, string requestUri, object content)
    => httpClient.SendJsonAsyncSS<IgnoreResponse>(method, requestUri, content);

        public static async Task<T> SendJsonAsyncSS<T>(this HttpClient httpClient, HttpMethod method, string requestUri, object content) {

            var requestJson = ServiceStack.Text.JsonSerializer.SerializeToString(content);
            var response = await httpClient.SendAsync(new HttpRequestMessage(method, requestUri) {
                Content=new StringContent(requestJson, Encoding.UTF8, "application/json")
            });

            // Make sure the call was successful before we
            // attempt to process the response content
            response.EnsureSuccessStatusCode();

            if (typeof(T)==typeof(IgnoreResponse)) {
                return default;
            } else {
                var responseJson = await response.Content.ReadAsStringAsync();
                return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(responseJson);
            }
        }

        class IgnoreResponse { }
    }

}

