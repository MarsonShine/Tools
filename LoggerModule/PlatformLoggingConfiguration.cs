using LoggerModule.Configs;
using LoggerModule.LogEnrichers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Serilog;
using Serilog.Events;
using System;

namespace LoggerModule
{
    public class PlatformLoggingConfiguration
    {
        private readonly IHostBuilder _builder;

        public PlatformLoggingConfiguration(IHostBuilder builder)
        {
            _builder = builder;
        }

        public IHostBuilder NLogConfiguration(LogLevel? logLevel = null)
        {
            return _builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                if (logLevel.HasValue)
                {
                    logging.SetMinimumLevel(logLevel.Value);
                }
            }).UseNLog();
        }

        public IHostBuilder SerilogConfiguration(Action<LoggerConfig> config = null)
        {
            return _builder.ConfigureWebHost(webhostBuilder =>
            {
                var option = new LoggerConfig();
                config?.Invoke(option);

                webhostBuilder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .Enrich.FromLogContext()
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .WriteTo.RollingFile(option.PathFileName,
                    outputTemplate: option.MessageTemplate,
                    fileSizeLimitBytes: option.FileSizeLimit)
                    );
            }).ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Information);
            });
        }
    }
}
