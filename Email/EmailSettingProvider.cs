using Microsoft.Extensions.Options;
using System;

namespace BBSS.Platform.Email
{
    public class EmailSettingProvider : IEmailSettingProvider
    {
        private readonly IOptions<EmailSetting> _emailSetting;
        public EmailSettingProvider(IOptions<EmailSetting> options)
        {
            _emailSetting = options;
        }
        public EmailSetting GetEmailSetting()
        {
            var emailSetting = _emailSetting.Value;
            if (emailSetting == null) throw new ArgumentNullException(nameof(emailSetting));

            return emailSetting;
        }
    }
}
