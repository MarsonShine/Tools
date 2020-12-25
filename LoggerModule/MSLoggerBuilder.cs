using LoggerModule.LayoutRenderers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NLog.LayoutRenderers;
using NLog.Web;
using NLog.Web.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerModule
{
    public class MSLoggerBuilder
    {
        private readonly IServiceCollection _services;
        public MSLoggerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public void WithNLogger()
        {
            // 自定义 layout renderer 一定要在加载 nlog.config 之前
            // 详情见 issue#4014：https://github.com/NLog/NLog/issues/4014#issuecomment-644997803
            RegisterLayoutRenderer();
            _services.AddSingleton(NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger());
        }

        private static void RegisterLayoutRenderer()
        {
            AspNetLayoutRendererBase.Register("requestId", (logInfo, context, cfg) => getHeaders(context, "requestId"));
            AspNetLayoutRendererBase.Register("platformId", (logInfo, context, cfg) => getHeaders(context, "platformId"));
            AspNetLayoutRendererBase.Register("userflag", (logInfo, context, cfg) => getHeaders(context, "userflag"));
            LayoutRenderer.Register<RequestDurationLayoutRenderer>("RequestDuration");
            LayoutRenderer.Register<YearLayoutRenderer>("Year");
            LayoutRenderer.Register<MonthLayoutRenderer>("Month");
            LayoutRenderer.Register<HoursLayoutRenderer>("Hours");

            string getHeaders(HttpContext context, string key)
            {
                if (context == null) return default;
                if (context.Request.Headers.TryGetValue(key, out var val))
                    return val.ToString();
                else
                    return default;
            }
        }
    }
}
