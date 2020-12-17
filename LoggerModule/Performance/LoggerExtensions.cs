using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerModule.Performance
{
    public static class LoggerExtensions
    {
        static LoggerExtensions()
        {

        }

        public static void PLogInformation<T1,T2>(this ILogger logger,string format,T1 arg1,T2 arg2)
        {
            var message = new OptimisedLoggingMessage(logger);
            var pmessage = message.Define<T1, T2>(LogLevel.Information, 1, format);
            pmessage(logger, arg1, arg2, null);
        }

        private static class PLog
        {
            internal static readonly Action<ILogger, string, int, Exception> _informationLoggerMessage = LoggerMessage.Define<string, int>(
                LogLevel.Information,
                1,
                "一周天气预报，星期{index}天气为：{weather}");
        }
    }
}
