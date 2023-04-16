using Core.Dto;
using Infrastructure.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Services
{
    public sealed class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(EmailDto request)
        {
            MimeMessage email = new();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailConfiguration:EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using SmtpClient smtp = new();
            smtp.Connect(_configuration.GetSection("EmailConfiguration:EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration.GetSection("EmailConfiguration:EmailUsername").Value, _configuration.GetSection("EmailConfiguration:EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

    }
}
