using System.Net.Mail;
using System.Net;
using Paradaim.Core.Common.Models.Domains;
namespace Paradaim.Core.Common
{
    public class Office365MailService
    {
        private readonly string _from;
        private readonly string _username;
        private readonly string _password;
        private readonly string _server;
        private readonly int _port;
        private readonly bool _enableSsl;

        public Office365MailService()
        {
            _from = EmailConstants.from;
            _username = EmailConstants.username;
            _password = EmailConstants.password;
            _server = EmailConstants.server;
            _port = EmailConstants.port;
            _enableSsl = EmailConstants.enableSsl;
        }

        public void Send(EmailDomain model)
        {
            try
            {
                // Create a MailMessage object
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(_from),
                    Subject = model.Subject,
                    Body = model.Body,
                    IsBodyHtml = true // Set to false if you don't want HTML content
                };

                // Add recipient
                mail.To.Add(model.To);

                // Create an SMTP client and configure it
                using (SmtpClient smtpClient = new SmtpClient(_server, _port))
                {
                    smtpClient.Credentials = new NetworkCredential(_username, _password);
                    smtpClient.EnableSsl = _enableSsl;

                    // Send the email
                    smtpClient.Send(mail);
                }

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email. Error: " + ex.Message);
            }
        }
    }
}




