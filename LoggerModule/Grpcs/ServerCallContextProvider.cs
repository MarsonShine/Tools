using Grpc.Core;

namespace LoggerModule.Grpcs
{
    public class ServerCallContextProvider : IServerCallContextProvider
    {
        public ServerCallContext ServerCallContext { get; set ; }
    }
}