using Grpc.Core;
using Microsoft.Extensions.Logging;
using Overt.Core.Grpc.Intercept;
using System;

namespace LoggerModule.Grpcs.Servers
{
    public class NLogGrpcServerTracer : IServerTracer
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        public NLogGrpcServerTracer(ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            _logger = loggerFactory.CreateLogger(nameof(NLogGrpcServerTracer));
            _serviceProvider = serviceProvider;
        }
        public string ServiceName { get; set; }

        public void Exception<TRequest>(ServerCallContext context, Exception exception, TRequest request = default)
        {
            _logger.LogError(exception, exception.Message);
        }

        public void Finish(ServerCallContext context)
        {
            _logger.LogTrace(context.Method + " Finished...");
        }

        public void Request<TRequest>(TRequest request, ServerCallContext context)
        {
            _logger.LogTrace($"{context.Host},{context.Method},{context.Peer} Request...");
        }

        public void Response<TResponse>(TResponse response, ServerCallContext context)
        {
            _logger.LogTrace($"{context.Host},{context.Method},{context.Peer} Response...");
        }
    }
}
