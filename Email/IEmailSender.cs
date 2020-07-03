using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BBSS.Platform.Email
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string body, bool isBodyHtml = true);

        Task SendAsync(string from, string to, string subject, string body, bool isBodyHtml = true);

        Task SendAsync(MailMessage mail, bool normalize = true);
    }
}
