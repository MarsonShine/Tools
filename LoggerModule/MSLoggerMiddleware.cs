using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
        public MSLoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IHttpContextAccessor accessor)
        {
            _next = next;
            loggerFactory.AddMSLogger(accessor);
            _logger = NLog.LogManager.GetLogger(nameof(MSLoggerMiddleware));
        }

        Stopwatch sw;
        public async Task InvokeAsync(HttpContext context)
        {
            sw = Stopwatch.StartNew();
            try
            {
                sw.Start();
                _ = context.Items.TryAdd("ElapsedTime", sw);
                await _next(context);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.WithProperty("elapsedTime", sw.ElapsedMilliseconds + "ms")
                    .Error(ex, "发生错误，错误消息 {exception} ", ex.Message);
            }
            finally
            {

            }
        }
    }
}