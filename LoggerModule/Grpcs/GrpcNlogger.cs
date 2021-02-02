using LoggerModule.Configs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace LoggerModule.Grpcs
{
    public class GrpcNlogger : ILogger
    {
        private readonly LoggerConfig _config;
        private readonly IServiceProvider _serviceProvider;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string logName;

        public GrpcNlogger(string name, LoggerConfig loggerConfig, IServiceProvider serviceProvider)
        {
            _config = loggerConfig;
            _serviceProvider = serviceProvider;
            logName = name;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel) => Enum.TryParse(_config.LogLevel, out LogLevel currentLevel) && logLevel >= currentLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;
            var s = _serviceProvider.GetRequiredService<IServerCallContextProvider>();
            var grpcContext = s.ServerCallContext;
            if (grpcContext != null)
            {
                var logEvent = grpcContext.UserState["logcontext"] as ServerCallContextHttpContextLogEvent;
                if (logEvent == null) return;

                logger.SetProperty("host", grpcContext.Host);
                logger.SetProperty("url", logEvent.Url);
                logger.SetProperty("method", grpcContext.Method);
                logger.SetProperty("requestId", logEvent.RequestId);
                logger.SetProperty("userflag", logEvent.UserFlag);
                logger.SetProperty("platformId", logEvent.PlatformId);
                logger.SetProperty("duration", logEvent.End() + " ms");
            }
            logger.Log(NLog.LogEventInfo.Create(NLog.LogLevel.FromOrdinal((int)logLevel), logName, null, state));
        }
    }
}