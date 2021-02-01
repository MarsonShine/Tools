using Grpc.Core;

namespace Grpcs
{
    public class ServerCallContextProvider : IServerCallContextProvider
    {
        public ServerCallContext ServerCallContext { get; set ; }
    }
}