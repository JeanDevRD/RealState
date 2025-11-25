using Microsoft.Extensions.Logging;
using MimeKit;
using RealState.Core.Application.DTOs.Email;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Settings;

namespace RealState.Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        public readonly MailSettings _mailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(MailSettings mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings;
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailRequestDto emailRequest)
        {
            try
            {
                emailRequest.ToRange?.Add(emailRequest.To ?? "");
                MimeMessage email = new()
                {
                    Sender = MailboxAddress.Parse(_mailSettings.EmailFrom),
                    Subject = emailRequest.Subject
                };

                foreach (var to in emailRequest.ToRange ?? [])
                {
                    email.To.Add(MailboxAddress.Parse(to));
                }

                BodyBuilder bodyBuilder = new()
                {
                    HtmlBody = emailRequest.HtmlBody
                };

                email.Body = bodyBuilder.ToMessageBody();
                using MailKit.Net.Smtp.SmtpClient smtp = new();
                await smtp.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occured {Exception}.", ex);
            }
        }
    }
}
