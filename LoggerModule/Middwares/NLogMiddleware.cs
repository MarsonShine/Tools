using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoggerModule.Middwares
{
    public class NLogMiddleware : IPlatformMiddleware
    {
        public readonly RequestDelegate _next;
        private readonly Logger _logger;
        public NLogMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IHttpContextAccessor accessor, IHostApplicationLifetime lifetime)
        {
            _next = next;
            loggerFactory.AddMSLogger(accessor);
            _logger = LogManager.GetLogger(nameof(MSLoggerMiddleware));
            SafeStopLogger(lifetime);
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
