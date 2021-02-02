using LoggerModule.Configs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace LoggerModule.Grpcs
{
    public class GrpcLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, GrpcNlogger> _loggers =
            new ConcurrentDictionary<string, GrpcNlogger>();

        private readonly IServiceProvider _serviceProvider;
        private readonly LoggerConfig _loggerConfig;
        public GrpcLoggerProvider(IServiceProvider serviceProvider, LoggerConfig config)
        {
            _serviceProvider = serviceProvider;
            _loggerConfig = config ?? new LoggerConfig();
        }

        public ILogger CreateLogger(string categoryName)
        {
            var logger = _loggers.GetOrAdd(categoryName, name => new GrpcNlogger(name, _loggerConfig, _serviceProvider));
            return logger;
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
