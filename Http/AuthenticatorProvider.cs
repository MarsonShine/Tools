using System;
using RestSharp.Authenticators;

namespace Http {
    public class AuthenticatorProvider {
        public IAuthenticator Create(AuthenticationLevel level, Func<IAuthenticator> action) {
            IAuthenticator auth;
            switch (level) {
                case AuthenticationLevel.Simple:
                    auth = action?.Invoke();
                    break;
                default:
                    auth = new NullAuthenticator();
                    break;
            }
            return auth;
        }
    }
}