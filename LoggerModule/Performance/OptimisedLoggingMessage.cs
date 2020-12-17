using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerModule.Performance
{
    public class OptimisedLoggingMessage
    {
        private readonly ILogger _logger;
        public OptimisedLoggingMessage(ILogger logger)
        {
            _logger = logger;
        }

        public Action<ILogger, T1, T2, Exception> Define<T1, T2>(LogLevel logLevel, EventId eventId, string format)
        {
            var messageDefine = LoggerMessage.Define<T1, T2>(
                logLevel,
                eventId,
                format);
            return messageDefine;
        }
    }
}
