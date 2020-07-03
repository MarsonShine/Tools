using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BBSS.Platform.Email
{
    public class NullEmailSender : EmailSenderBase
    {
        public ILogger<NullEmailSender> Logger { get; set; }
        public NullEmailSender(IEmailSettingProvider emailSettingProvider) : base(emailSettingProvider)
        {
            Logger = NullLogger<NullEmailSender>.Instance;
        }

        protected override Task SendEmailAsync(MailMessage mail)
        {
            Logger.LogWarning("NullEmailSender Start!");
            Logger.LogDebug("SendEmailAsync:");
            LogEmail(mail);
            return Task.FromResult(0);
        }

        private void LogEmail(MailMessage mail)
        {
            Logger.LogDebug(mail.To.ToString());
            Logger.LogDebug(mail.CC.ToString());
            Logger.LogDebug(mail.Subject);
            Logger.LogDebug(mail.Body);
        }
    }
}
