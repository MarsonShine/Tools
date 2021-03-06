﻿using Microsoft.Extensions.Logging;
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

        public static void PLogInformation<T1>(this ILogger logger, string format, T1 arg1)
        {
            var message = new OptimisedLoggingMessage(logger);
            var pmessage = message.Define<T1>(LogLevel.Information, 1, format);
            pmessage(logger, arg1, null);
        }

        public static void PLogInformation<T1, T2>(this ILogger logger, string format, T1 arg1, T2 arg2)
        {
            var message = new OptimisedLoggingMessage(logger);
            var pmessage = message.Define<T1, T2>(LogLevel.Information, 2, format);
            pmessage(logger, arg1, arg2, null);
        }

        public static void PLogInformation<T1, T2, T3>(this ILogger logger, string format, T1 arg1, T2 arg2, T3 arg3)
        {
            var message = new OptimisedLoggingMessage(logger);
            var pmessage = message.Define<T1, T2, T3>(LogLevel.Information, 3, format);
            pmessage(logger, arg1, arg2, arg3, null);
        }
        public static void PLogInformation<T1, T2, T3, T4>(this ILogger logger, string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var message = new OptimisedLoggingMessage(logger);
            var pmessage = message.Define<T1, T2, T3, T4>(LogLevel.Information, 4, format);
            pmessage(logger, arg1, arg2, arg3, arg4, null);
        }
        public static void PLogInformation<T1, T2, T3, T4, T5>(this ILogger logger, string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var message = new OptimisedLoggingMessage(logger);
            var pmessage = message.Define<T1, T2, T3, T4, T5>(LogLevel.Information, 5, format);
            pmessage(logger, arg1, arg2, arg3, arg4, arg5, null);
        }
        public static void PLogInformation<T1, T2, T3, T4, T5, T6>(this ILogger logger, string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            var message = new OptimisedLoggingMessage(logger);
            var pmessage = message.Define<T1, T2, T3, T4, T5, T6>(LogLevel.Information, 6, format);
            pmessage(logger, arg1, arg2, arg3, arg4, arg5, arg6, null);
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
