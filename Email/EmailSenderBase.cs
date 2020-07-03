﻿using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BBSS.Platform.Email
{
    public abstract class EmailSenderBase : IEmailSender
    {
        private readonly IEmailSettingProvider _emailSettingProvider;
        protected EmailSenderBase(IEmailSettingProvider emailSettingProvider)
        {
            _emailSettingProvider = emailSettingProvider;
        }
        public virtual async Task SendAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendAsync(new MailMessage
            {
                To = { to },
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            });
        }

        public virtual async Task SendAsync(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendAsync(new MailMessage(from, to, subject, body) { IsBodyHtml = isBodyHtml });
        }

        public async Task SendAsync(MailMessage mail, bool normalize = true)
        {
            if (normalize)
            {
                await NormalizeMailAsync(mail);
            }

            await SendEmailAsync(mail);
        }
        protected virtual async Task NormalizeMailAsync(MailMessage mail)
        {
            if (mail.From == null || string.IsNullOrEmpty(mail.From.Address))
            {
                var emailSetting = _emailSettingProvider.GetEmailSetting();
                mail.From = new MailAddress(
                    emailSetting.FromAddress,
                    emailSetting.FromDisplayName,
                    Encoding.UTF8
                    );
            }

            if (mail.HeadersEncoding == null)
            {
                mail.HeadersEncoding = Encoding.UTF8;
            }

            if (mail.SubjectEncoding == null)
            {
                mail.SubjectEncoding = Encoding.UTF8;
            }

            if (mail.BodyEncoding == null)
            {
                mail.BodyEncoding = Encoding.UTF8;
            }
            await Task.Yield();
        }

        protected abstract Task SendEmailAsync(MailMessage mail);
    }
}
