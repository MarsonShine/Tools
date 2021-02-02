using Grpc.Core;

namespace LoggerModule.Grpcs
{
    public interface IServerCallContextAccessor
    {
        ServerCallContext ServerCallContext {get;}
    }
}