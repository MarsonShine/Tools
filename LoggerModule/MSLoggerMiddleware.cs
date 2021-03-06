﻿using LoggerModule.LogEnrichers;
using LoggerModule.Middwares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using Serilog;
using Serilog.Context;
using System;
using System.Threading.Tasks;

// TODO: 最佳实践应该是不同的日志组件分在不同的模块中,归类到不同的dll中发布
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
            catch (Exception ex) when (True(() => Log.Error(ex, "发生错误，错误消息 {exception} ", ex.Message)))
            {
                //var end = DateTimeOffset.Now.Ticks;
                //var timespan = new TimeSpan(end - start);
                //Log
                //.ForContext("ElapsedTime", timespan.TotalMilliseconds + "ms")
                //.Error(ex, "发生错误，错误消息 {exception} ", ex.Message);
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


    public static class PlatformLoggingApplicationBuilderExtensions
    {
        public static void UsePlatformLogger(this IApplicationBuilder builder, LoggerComponent loggerComponent = LoggerComponent.NLog)
        {
            switch (loggerComponent)
            {
                case LoggerComponent.NLog:
                    builder.UseMiddleware<MSLoggerMiddleware>();
                    break;
                case LoggerComponent.Serilog:
                    builder.UseMiddleware<SerilogMiddleware>();
                    builder.UseSerilogRequestLogging();
                    break;
                default:
                    break;
            }
        }
    }
}