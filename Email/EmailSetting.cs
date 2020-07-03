namespace BBSS.Platform.Email
{
    public class EmailSetting
    {
        public string FromAddress { get; set; }
        public string FromDisplayName { get; set; }

        public Smtp Smtp { get; set; }
    }
    public sealed class Smtp
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool EnableSsl { get; set; }
    }
}
