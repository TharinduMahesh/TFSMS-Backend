using System.Net.Mail;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace test6API.Services
{
    // This helper class will hold your SendGrid settings from appsettings.json
    public class SendGridSettings
    {
        public string? ApiKey { get; set; }
        public string? FromEmail { get; set; }
        public string? FromName { get; set; }
    }

    public class SendGridEmailService : IEmailService
    {
        private readonly ILogger<SendGridEmailService> _logger;
        private readonly SendGridSettings _sendGridSettings;

        public SendGridEmailService(IOptions<SendGridSettings> sendGridSettings, ILogger<SendGridEmailService> logger)
        {
            _sendGridSettings = sendGridSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            if (string.IsNullOrEmpty(_sendGridSettings.ApiKey))
            {
                _logger.LogError("SendGrid API Key is not configured. Please check your appsettings.json or user secrets.");
                throw new InvalidOperationException("Email service is not configured.");
            }

            var client = new SendGridClient(_sendGridSettings.ApiKey);
            var from = new EmailAddress(_sendGridSettings.FromEmail, _sendGridSettings.FromName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);

            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Failed to send email to {toEmail}. Status code: {response.StatusCode}");
                var responseBody = await response.Body.ReadAsStringAsync();
                _logger.LogError($"SendGrid response: {responseBody}");
                throw new InvalidOperationException("Failed to send email.");
            }
            else
            {
                _logger.LogInformation($"Email sent to {toEmail} successfully!");
            }
        }
    }
}