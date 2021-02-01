using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Grpc.Core;
using Grpc.Core.Interceptors;
using System;

namespace Grpcs.Intercectors
{
    public class ServerRequestScope:Interceptor
    {
        public IServiceProvider ServiceProvider { get; }

        public ServerRequestScope(IServiceProvider serviceProvider) {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;
            // 这里可以对 serverCallContext 做一些额外的数据存储的操作
            serviceProvider.GetRequiredService<IServerCallContextProvider>().ServerCallContext = context;
            return await base.UnaryServerHandler(request, context, continuation);
        }
    }
}