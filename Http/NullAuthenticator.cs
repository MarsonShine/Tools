using RestSharp;
using RestSharp.Authenticators;

namespace Http {
    public class NullAuthenticator : IAuthenticator {
        public void Authenticate(IRestClient client, IRestRequest request) {

        }
    }
}