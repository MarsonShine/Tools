using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

public class EasyRestClient {
    private readonly RestClient _client;

    public EasyRestClient(string baseUrl) {
        _client = new RestClient(baseUrl);
    }

    // Get,query string
    public async Task<string> GetStringAsync(string api, string queryString, Dictionary<string, string> headers = null) {
        var request = CreateRequest(api, Method.GET);
        request.Resource = $"{api}?{queryString}";
        request.SetHeaders(headers);
        // 授权
        SetAuthorization(request, "");
        var response = await _client.ExecuteAsync(request);
        return ExactResponse(response);
    }

    public RestRequest CreateRequest(string api, Method method, DataFormat dataFormat = DataFormat.None) {
        var request = new RestRequest(api, method, dataFormat);
        return request;
    }

    private void SetAuthorization(RestRequest request, string token) {
        request.AddHeader("Authorization", token);
    }

    private string ExactResponse(IRestResponse response) {
        if (response == null) return default;
        if (!response.IsSuccessful) return default;
        return response.Content;
    }

    public async Task<string> GetStringAsync(string api, object queryBody, Dictionary<string, string> headers = null) {
        var request = CreateRequest(api, Method.GET, DataFormat.Json);
        request.AddJsonBody(queryBody);
        request.SetHeaders(headers);
        SetAuthorization(request, "");

        var r = await _client.ExecuteAsync(request);
        return ExactResponse(r);
    }

    public async Task<string> PostAsync(string api, object body, Dictionary<string, string> headers = null) {
        var request = CreateRequest(api, Method.POST, DataFormat.Json);
        request.AddJsonBody(body);
        request.SetHeaders(headers);
        SetAuthorization(request, "");

        var r = await _client.ExecuteAsync(request);
        return ExactResponse(r);
    }
}