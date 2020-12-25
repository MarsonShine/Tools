using LoggerModule.LogEnrichers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using Serilog;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace LoggerModule
{
    /// <summary>
    /// 封装错误信息，自动发送给日志服务器
    /// 需要定义的格式为：layout={#${longdate}#${nodeName}#${logger}#${uppercase:${level}}#${callsite}#${callsite-linenumber}#${aspnet-request-url}#${aspnet-request-method}#${aspnet-mvc-controller}#${aspnet-mvc-action}#${message}#${exception:format=ToString}#${elapsedTime}#}
    /// </summary>
    public class MSLoggerMiddleware
    {
        public readonly RequestDelegate _next;
        public MSLoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IHttpContextAccessor accessor, IHostApplicationLifetime lifetime)
        {
            _next = next;
            loggerFactory.AddMSLogger(accessor);
            //_logger = LogManager.GetLogger(nameof(MSLoggerMiddleware));
            //SafeStopLogger(lifetime);
        }

        private void SafeStopLogger(IHostApplicationLifetime lifetime)
        {
            lifetime.ApplicationStopped.Register(() =>
            {
                LogManager.Shutdown();
            });
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //var start = DateTimeOffset.Now.Ticks;
            try
            {
                //_ = context.Items.TryAdd("ElapsedTime", start);
                LogContext.Push(new AspNetRequestEnricher(context));
                await _next(context);
            }
            catch (Exception ex)
            {
                //var end = DateTimeOffset.Now.Ticks;
                //var timespan = new TimeSpan(end - start);
                Log
                    //.ForContext("ElapsedTime", timespan.TotalMilliseconds + "ms")
                    .Error(ex, "发生错误，错误消息 {exception} ", ex.Message);
            }
            finally
            {

            }
        }
    }


    public static class PlatformLoggingApplicationBuilderExtensions
    {
        public static void UsePlatformLogger(this IApplicationBuilder builder)
        {
            builder.UseSerilogRequestLogging();
            builder.UseMiddleware<MSLoggerMiddleware>();
        }
    }
}