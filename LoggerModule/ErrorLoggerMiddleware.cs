using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoggerModule
{
    /// <summary>
    /// 封装错误信息，自动发送给日志服务器
    /// 需要定义的格式为：layout={#${longdate}#${nodeName}#${logger}#${uppercase:${level}}#${callsite}#${callsite-linenumber}#${aspnet-request-url}#${aspnet-request-method}#${aspnet-mvc-controller}#${aspnet-mvc-action}#${message}#${exception:format=ToString}#<elapsedTime>${elapsed-time}</elapsedTime>#}
    /// </summary>
    public class ErrorLoggerMiddleware
    {
        public readonly RequestDelegate _next;

        public ErrorLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var log = NLog.LogManager.GetLogger(nameof(ErrorLoggerMiddleware));
                log.Error(ex,"发生错误，错误消息 {exception} ",ex.Message);
            }
            finally
            {

            }
        }
    }
}
