using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BBSS.Platform.Email.Stmp
{
    public class StmpEmailSender : EmailSenderBase, IStmpEmailSender
    {
        protected EmailSetting EmailSetting { get; }
        public StmpEmailSender(IEmailSettingProvider emailSettingProvider) : base(emailSettingProvider)
        {
            EmailSetting = emailSettingProvider.GetEmailSetting();
        }

        public async Task<SmtpClient> BuildClientAsync()
        {
            var smtpClient = string.IsNullOrEmpty(EmailSetting.Smtp.Port) ?
               new SmtpClient(EmailSetting.Smtp.Host) :
               new SmtpClient(EmailSetting.Smtp.Host, int.Parse(EmailSetting.Smtp.Port));
            try
            {
                if (EmailSetting.Smtp.EnableSsl)
                {
                    smtpClient.EnableSsl = true;
                }
                if (EmailSetting.Smtp.UseDefaultCredentials)
                {
                    smtpClient.UseDefaultCredentials = true;
                }
                else
                {
                    smtpClient.UseDefaultCredentials = false;

                    var userName = EmailSetting.Smtp.UserName;
                    if (!string.IsNullOrEmpty(userName))
                    {
                        var password = EmailSetting.Smtp.Password;
                        var domain = EmailSetting.Smtp.Domain;
                        smtpClient.Credentials = !string.IsNullOrEmpty(domain)
                            ? new NetworkCredential(userName, password, domain)
                            : new NetworkCredential(userName, password);
                    }
                }

                return await Task.FromResult(smtpClient);
            }
            catch
            {
                smtpClient.Dispose();
                throw;
            }
        }

        protected override async Task SendEmailAsync(MailMessage mail)
        {
            using (var smtpClient = await BuildClientAsync())
            {
                await smtpClient.SendMailAsync(mail);
            }
        }
    }
}
