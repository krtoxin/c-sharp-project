using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using StudyBuddy.Services.IServices;

namespace StudyBuddy.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var username = _config["Email:Mailjet:Username"];
            var password = _config["Email:Mailjet:Password"];
            var from = _config["Email:Mailjet:From"];

            using var smtpClient = new SmtpClient("in-v3.mailjet.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(from!),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mail.To.Add(to);
            await smtpClient.SendMailAsync(mail);
        }
    }
}
