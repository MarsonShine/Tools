using Grpc.Core;

namespace LoggerModule.Grpcs
{
    public interface IServerCallContextProvider
    {
        ServerCallContext ServerCallContext {get; set;}
    }
}