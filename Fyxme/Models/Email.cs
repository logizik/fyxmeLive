using System.Net.Mail;

namespace Fyxme.Models
{
    public class Email
    {
        public MailMessage mail;
        public SmtpClient SmtpServer;

        public string SmtpHostName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int Port { get; set; }
        public string NetworkCredentialUser { get; set; }
        public string NetworkCredentialPassword { get; set; }

        public Email(string SmtpHostName)
        {
            this.SmtpHostName = SmtpHostName;

            mail = new MailMessage();
            mail.IsBodyHtml = true;

            SmtpServer = new SmtpClient(this.SmtpHostName);
            SmtpServer.EnableSsl = true;
        }

        public void Send()
        {
            mail.From = new MailAddress(this.From);
            mail.To.Add(this.To);
            mail.Subject = this.Subject;
            mail.Body = this.Body;

            SmtpServer.Port = this.Port;
            SmtpServer.Credentials = new System.Net.NetworkCredential(this.NetworkCredentialUser, this.NetworkCredentialPassword);

            this.SmtpServer.Send(this.mail);
        }
    }
}