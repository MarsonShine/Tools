using BBSS.Platform.Email.Stmp;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BBSS.Platform.Email
{
    public static class EmailServiceCollections
    {
        public static void AddEmailService(this IServiceCollection services, Action<EmailSetting> configure)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            services.Configure(configure);
            services.AddSingleton<IEmailSettingProvider, EmailSettingProvider>();
            services.AddTransient<IEmailSender, NullEmailSender>();
            services.AddTransient<IEmailSender, StmpEmailSender>();
        }
    }
}
