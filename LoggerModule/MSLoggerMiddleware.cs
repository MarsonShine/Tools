using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
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
        private readonly NLog.Logger _logger;
        public MSLoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IHttpContextAccessor accessor, IHostApplicationLifetime lifetime)
        {
            _next = next;
            loggerFactory.AddMSLogger(accessor);
            _logger = LogManager.GetLogger(nameof(MSLoggerMiddleware));

            SafeStopLogger(lifetime);
        }

        private void SafeStopLogger(IHostApplicationLifetime lifetime)
        {
            lifetime.ApplicationStopped.Register(() => {
                LogManager.Shutdown();
            });
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var start = DateTimeOffset.Now.Ticks;
            try
            {
                _ = context.Items.TryAdd("ElapsedTime", start);
                await _next(context);
            }
            catch (Exception ex)
            {
                var end = DateTimeOffset.Now.Ticks;
                var timespan = new TimeSpan(end - start);
                _logger.WithProperty("elapsedTime", timespan.TotalMilliseconds + "ms")
                    .Error(ex, "发生错误，错误消息 {exception} ", ex.Message);
            }
            finally
            {

            }
        }
    }
}