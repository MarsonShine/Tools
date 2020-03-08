namespace Http.Abstract {
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// TODO:是否加个 abstract 来添加授权
    /// </summary>
    public class DefaultHttpClient : IHttpClient {
        private readonly HttpClient _httpClient;
        public DefaultHttpClient(IHttpClientFactory httpClientFactory) {
            _httpClient = httpClientFactory.CreateClient("default");
        }

        public async Task<T> DeleteAsync<T>(string api, object body = null, RequestType requestType = RequestType.QueryString, Dictionary<string, string> header = null) {
            var request = CreateRequest(api, body, HttpMethod.Delete, requestType);
            AddHeaders(request, header);
            SetAuthorization();

            var r = await _httpClient.SendAsync(request);
            if (r.IsSuccessStatusCode) {
                var content = await r.Content.ReadAsStringAsync();
                return JsonHelper.Deserialize<T>(content);
            } else {
                return default;
            }

        }

        public async Task<T> GetAsync<T>(string api, object body = null, RequestType requestType = default, Dictionary<string, string> header = null) {
            var request = CreateRequest(api, body, HttpMethod.Get, requestType);
            AddHeaders(request, header);
            SetAuthorization();
            return await SendAsync<T>(request);
        }
        private async Task<T> SendAsync<T>(HttpRequestMessage request) {
            var r = await _httpClient.SendAsync(request);
            if (r.IsSuccessStatusCode) {
                var content = await r.Content.ReadAsStringAsync();
                if (typeof(string) == typeof(T)) return (T) (object) content;
                return JsonHelper.Deserialize<T>(content);
            } else {
                return default;
            }
        }
        public async Task<T> PatchAsync<T>(string api, object body = null, RequestType requestType = default, Dictionary<string, string> header = null) {
            var request = CreateRequest(api, body, HttpMethod.Patch, requestType);
            AddHeaders(request, header);
            SetAuthorization();
            return await SendAsync<T>(request);
        }

        public async Task<T> PostAsync<T>(string api, object body = null, RequestType requestType = default, Dictionary<string, string> header = null) {
            var request = CreateRequest(api, body, HttpMethod.Patch, requestType);
            AddHeaders(request, header);
            SetAuthorization();
            return await SendAsync<T>(request);
        }

        private HttpRequestMessage CreateRequest(string api, object body, HttpMethod httpMethod, RequestType requestType) {
            HttpRequestMessage hrm;
            if (body == null) hrm = new HttpRequestMessage(httpMethod, api);
            else {
                if (requestType == RequestType.QueryString) {
                    api = api + "?" + QueryStringHelper.ToQueryString(body);
                }
                hrm = new HttpRequestMessage(httpMethod, api);
                if (requestType == RequestType.JsonBody) {
                    string content = JsonHelper.Serialize(body);
                    hrm.Content = new StringContent(content, Encoding.UTF8, "application/json");
                }
            }
            return hrm;
        }

        private void AddHeaders(HttpRequestMessage hrm, Dictionary<string, string> headers) {
            if (headers == null) return;
            foreach (var item in headers) {
                if (hrm.Headers.Contains(item.Key)) continue;
                hrm.Headers.Add(item.Key, item.Value);
            }
        }
        private void SetAuthorization(string scheme = null, string value = null) {
            if (string.IsNullOrEmpty(scheme) || string.IsNullOrEmpty(value)) return;

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme, value);
        }
    }
}