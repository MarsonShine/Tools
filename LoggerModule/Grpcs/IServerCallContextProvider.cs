using Grpc.Core;

namespace Grpcs
{
    public interface IServerCallContextProvider
    {
        ServerCallContext ServerCallContext {get; set;}
    }
}