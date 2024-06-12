using Application.Contracts.Services;
using Application.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Mail
{
    public class EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger) : IEmailService
    {
        public EmailSettings EmailSettings { get; } = emailSettings.Value;
        public ILogger<EmailService> Logger { get; } = logger;

        public async Task<bool> SendEmail(Email email)
        {
            var client = new SendGridClient(EmailSettings.ApiKey);

            var from = new EmailAddress
            {
                Email = EmailSettings.FromAddress,
                Name = EmailSettings.FromName
            };

            var to = new EmailAddress(email.To);
            var sendGridMessage = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Body, email.Body);

            var response = await client.SendEmailAsync(sendGridMessage);

            Logger.LogInformation("Email sent.");

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            Logger.LogError("Email sending failed with status code: {StatusCode}", response.StatusCode);
            return false;
        }
    }

}
