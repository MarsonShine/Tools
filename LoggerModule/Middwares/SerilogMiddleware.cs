using LoggerModule.LogEnrichers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace LoggerModule.Middwares
{
    public class SerilogMiddleware : IPlatformMiddleware
    {
        public readonly RequestDelegate _next;
        public SerilogMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IHttpContextAccessor accessor)
        {
            _next = next;
            loggerFactory.AddMSLogger(accessor);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                LogContext.Push(new AspNetRequestEnricher(context));
                await _next(context);
            }
            catch (Exception ex) when (True(() => Log.Error(ex, "发生错误，错误消息 {exception} ", ex.Message)))
            {
                Log
                    //.ForContext("ElapsedTime", timespan.TotalMilliseconds + "ms")
                    .Error(ex, "发生错误2，错误消息 {exception} ", ex.Message);
            }
            finally
            {

            }
        }

        private bool True(Action action)
        {
            action();
            return true;
        }
    }
}
