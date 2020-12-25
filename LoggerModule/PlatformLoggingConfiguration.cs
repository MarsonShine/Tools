using LoggerModule.LogEnrichers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Serilog;
using Serilog.Events;

namespace LoggerModule
{
    public class PlatformLoggingConfiguration
    {
        private readonly IHostBuilder _builder;

        public PlatformLoggingConfiguration(IHostBuilder builder)
        {
            _builder = builder;
        }

        public IHostBuilder NLogConfiguration()
        {
            return _builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Information);
            }).UseNLog();
        }

        public IHostBuilder SerilogConfiguration()
        {
            return _builder.ConfigureWebHost(webhostBuilder =>
            {
                webhostBuilder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .Enrich.FromLogContext()
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .WriteTo.RollingFile("logs/log-{Hour}.log", outputTemplate: "[{RequestId}] [{Timestamp:HH:mm:ss} [{AppRequestId} {PlatformId} {UserFlag}] {Level:u3}] {Message:lj} {NewLine}{Exception}"));
            }).ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Information);
            });
        }
    }
}
