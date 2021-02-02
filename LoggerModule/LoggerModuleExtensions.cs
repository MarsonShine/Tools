using LoggerModule.LayoutRenderers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.LayoutRenderers;
using NLog.Web;
using NLog.Web.LayoutRenderers;
using System;

namespace LoggerModule
{
    public static class LoggerModuleExtensions
    {
        /// <summary>
        /// 日志模板，请使用 ${event-properties:propertyName}
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static MSLoggerBuilder AddMSLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return new MSLoggerBuilder(services);
        }
    }

    public static class LoggerHostBuilderExtensions
    {
        public static IHostBuilder ConfigurePlatformLogging(this IHostBuilder hostBuilder, Action<PlatformLoggingConfiguration> configure)
        {
            var configuration = new PlatformLoggingConfiguration(hostBuilder);
            configure(configuration);
            return hostBuilder;
        }
    }
}