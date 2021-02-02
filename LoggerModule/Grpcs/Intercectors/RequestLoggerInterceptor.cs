using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using LoggerModule.Grpcs;

namespace Grpcs.Intercectors
{
    public class RequestLoggerInterceptor : Interceptor
    {
        public IServiceProvider ServiceProvider { get; }

        public RequestLoggerInterceptor(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                var logEvent = GrpcContextConvertor.Convert2LogEvent(context.RequestHeaders);
                // 这里可以对 serverCallContext 做一些额外的数据存储的操作
                ServiceProvider.GetRequiredService<IServerCallContextProvider>().ServerCallContext = context;
                return await base.UnaryServerHandler(request, context, continuation);
            }
            finally
            {
                context.UserState.Remove("logcontext");
            }

        }
    }
}