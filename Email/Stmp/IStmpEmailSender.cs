using System.Net.Mail;
using System.Threading.Tasks;

namespace BBSS.Platform.Email.Stmp
{
    public interface IStmpEmailSender
    {
        Task<SmtpClient> BuildClientAsync();
    }
}
