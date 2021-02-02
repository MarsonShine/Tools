using LoggerModule.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerModule.Grpcs
{
    public static class GrpcLoggerServiceCollectionExtensions
    {
        public static MSLoggerBuilder AddGrpcLoggerService(this IServiceCollection services, Action<LoggerConfig> configure)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddOptions();
            services.Configure(configure);
            var loggerConfig = new LoggerConfig();
            configure?.Invoke(loggerConfig);
            LogLevel logLevel = LogLevel.Trace;
            Enum.TryParse(loggerConfig.LogLevel, out logLevel);

            services.Add(ServiceDescriptor.Scoped<IServerCallContextProvider, ServerCallContextProvider>(x => new ServerCallContextProvider()));
            services.Add(ServiceDescriptor.Scoped<IServerCallContextAccessor, ServerCallContextAccessor>(x => new ServerCallContextAccessor(x.GetRequiredService<IServerCallContextProvider>().ServerCallContext)));

            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddFilter("Microsoft", LogLevel.Critical)
                     .AddFilter("System", LogLevel.Critical)
                     .AddFilter("Grpc", LogLevel.Debug);
                logging.SetMinimumLevel(logLevel);
                logging.Services.Add(ServiceDescriptor.Scoped<ILoggerProvider, GrpcLoggerProvider>(x => new GrpcLoggerProvider(x, loggerConfig)));
            });


            return new MSLoggerBuilder(services);
        }
    }
}
