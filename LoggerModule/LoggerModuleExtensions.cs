using Microsoft.Extensions.DependencyInjection;
using NLog.Web;
using NLog.Web.LayoutRenderers;

namespace LoggerModule
{
    public static class LoggerModuleExtensions {
        public static void AddMSLoggerService(this IServiceCollection services) {
            RegisterLayoutRenderer();
            services.AddSingleton(NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger());
        }

        private static void RegisterLayoutRenderer() {
            AspNetLayoutRendererBase.Register("requestId",(logInfo,context,cfg)=>
                context.Request.Headers["requestId"].ToString()
            );
            AspNetLayoutRendererBase.Register("platformId",(logInfo,context,cfg)=>context.Request.Headers["platformId"].ToString());
            AspNetLayoutRendererBase.Register("userflag",(logInfo,context,cfg)=>context.Request.Headers["userflag"].ToString());
        }
    }
}