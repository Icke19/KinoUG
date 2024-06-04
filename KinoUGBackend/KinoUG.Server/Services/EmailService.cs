using KinoUG.Server.Repository.Interfaces;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace KinoUG.Server.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string html, string text)
        {
            var client = new MailjetClient(_configuration["EmailSettings:MailjetApiKey"], _configuration["EmailSettings:MailjetSecretKey"]);
            var request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, _configuration["EmailSettings:From"])
            .Property(Send.FromName, "KinoUG")
            .Property(Send.Subject, subject)
            .Property(Send.HtmlPart, html)
            .Property(Send.TextPart, text)
            .Property(Send.Recipients, new JArray {
                new JObject {
                    {"Email", to}
                }
            });

            try
            {
                var response = await client.PostAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Email sent to {to}");
                }
                else
                {
                    _logger.LogError($"Failed to send email to {to}. Status Code: {response.StatusCode}");
                    _logger.LogError($"Response: {response.GetData()}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email to {to}: {ex.Message}");
                throw;
            }
        }
    }
}
