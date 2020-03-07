namespace Http.Abstract {
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IHttpClient {
        Task<T> GetAsync<T>(string api, object body = null, RequestType requestType = RequestType.QueryString, Dictionary<string, string> header = null);
        Task<T> DeleteAsync<T>(string api, object body = null, RequestType requestType = RequestType.QueryString, Dictionary<string, string> header = null);
        Task<T> PostAsync<T>(string api, object body = null, RequestType requestType = RequestType.JsonBody, Dictionary<string, string> header = null);
        Task<T> PatchAsync<T>(string api, object body = null, RequestType requestType = RequestType.JsonBody, Dictionary<string, string> header = null);
    }
}