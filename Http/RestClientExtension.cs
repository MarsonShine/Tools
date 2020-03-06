using System.Collections.Generic;
using RestSharp;

public static class RestClientExtension {
    public static RestRequest SetHeaders(this RestRequest request, Dictionary<string, string> headers) {
        if (headers == null) return request;
        request.AddHeaders(headers);
        return request;
    }

    public static RestRequest SetHeaders(this RestRequest request, string name, string value) {
        if (name != "" && value != "")
            request.AddHeader(name, value);
        return request;
    }
}