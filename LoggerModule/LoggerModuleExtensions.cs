using LoggerModule.LayoutRenderers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using NLog.LayoutRenderers;
using NLog.Targets;
using NLog.Web;
using NLog.Web.LayoutRenderers;

namespace LoggerModule
{
    public static class LoggerModuleExtensions
    {
        public static void AddMSLoggerService(this IServiceCollection services)
        {
            // 自定义 layout renderer 一定要在加载 nlog.config 之前
            // 详情见 issue#4014：https://github.com/NLog/NLog/issues/4014#issuecomment-644997803
            RegisterLayoutRenderer();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger());
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