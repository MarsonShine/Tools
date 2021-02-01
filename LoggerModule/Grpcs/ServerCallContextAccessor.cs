using Grpc.Core;

namespace Grpcs
{
    public class ServerCallContextAccessor : IServerCallContextAccessor
    {
        private readonly ServerCallContext _serverCallContext;
        public ServerCallContextAccessor(ServerCallContext serverCallContext){
            _serverCallContext = serverCallContext;
        }
        public ServerCallContext ServerCallContext => _serverCallContext;
    }
}