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
        private string requestId;
        private string userflag;
        private string platformId;

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
                exactContext(context);
                LoggerFormatByConfig(ex);
            }
            finally
            {

            }

            void exactContext(HttpContext context)
            {
                var headers = context.Request.Headers;

                if (headers.TryGetValue("requestId", out var svs))
                    requestId = svs.ToString();
                if (headers.TryGetValue("userflag", out svs))
                    userflag = svs.ToString();
                if (headers.TryGetValue("platformId", out svs))
                    platformId = svs.ToString();
            }
        }

        private void LoggerFormatByConfig(Exception ex)
        {
            var logInfo = new NLog.LogEventInfo() { Level = NLog.LogLevel.Error };
            logInfo.Exception = ex;
            logInfo.Message = ex.Message;
            logInfo.Properties["requestId"] = requestId;
            logInfo.Properties["userflag"] = userflag;
            logInfo.Properties["platformId"] = platformId;
            logInfo.Properties["message"] = ex.Message;
            var log = NLog.LogManager.GetLogger(nameof(ErrorLoggerMiddleware));
            log.Log(logInfo);
        }
    }
}
