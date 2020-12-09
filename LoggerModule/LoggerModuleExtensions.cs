using Microsoft.Extensions.DependencyInjection;
using NLog.Web;
using NLog.Web.LayoutRenderers;

namespace LoggerModule
{
    public static class LoggerModuleExtensions {
        public static void AddMSLoggerService(this IServiceCollection services) {
            // 自定义 layout renderer 一定要在加载 nlog.config 之前
            // 详情见 issue#4014：https://github.com/NLog/NLog/issues/4014#issuecomment-644997803
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