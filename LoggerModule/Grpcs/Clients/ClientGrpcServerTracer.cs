using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Overt.Core.Grpc.Intercept;
using System;

namespace LoggerModule.Grpcs.Clients
{
    public class ClientGrpcServerTracer : IClientTracer
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClientGrpcServerTracer(
            ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = loggerFactory.CreateLogger(nameof(ClientGrpcServerTracer));
            _httpContextAccessor = httpContextAccessor;
        }
        public void Exception<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, Exception exception, TRequest request = null)
            where TRequest : class
            where TResponse : class
        {
            _logger.LogError(exception, "ClientGrpcServerTracer was wrong...");
        }

        public void Finish<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context)
            where TRequest : class
            where TResponse : class
        {
            _logger.LogTrace(context.Method + "Client Finished...");

        }

        public void Request<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context)
            where TRequest : class
            where TResponse : class
        {
            AddServerCallContextOptionHeaders(context.Options.Headers, _httpContextAccessor.HttpContext);
        }

        private void AddServerCallContextOptionHeaders(Metadata headers, HttpContext httpContext)
        {
            var logEvent = GrpcContextConvertor.Convert2LogEvent(httpContext);
            headers.Add(new Metadata.Entry(ServerCallContextHttpContextLogEventConst.PlatformId, logEvent.PlatformId));
            headers.Add(new Metadata.Entry(ServerCallContextHttpContextLogEventConst.UserFlag, logEvent.UserFlag));
            headers.Add(new Metadata.Entry(ServerCallContextHttpContextLogEventConst.RequestId, logEvent.RequestId));
            headers.Add(new Metadata.Entry(ServerCallContextHttpContextLogEventConst.Url, logEvent.Url));
            headers.Add(new Metadata.Entry(ServerCallContextHttpContextLogEventConst.SourceIp, logEvent.SourceIp));
        }

        public void Response<TRequest, TResponse>(TResponse response, ClientInterceptorContext<TRequest, TResponse> context)
            where TRequest : class
            where TResponse : class
        {
            _logger.LogTrace($"{context.Host},{context.Method} Client Response...");
        }
    }
}
