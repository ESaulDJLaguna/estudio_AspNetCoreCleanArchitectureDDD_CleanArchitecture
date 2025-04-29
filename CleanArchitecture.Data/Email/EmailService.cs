using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CleanArchitecture.Infrastructure.Email
{
	public class EmailService : IEmailService
	{
		public EmailSettings _emailSettings { get; }
		public ILogger<EmailService> _logger { get; }

		public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
		{
			_emailSettings = emailSettings.Value;
			_logger = logger;
		}

		public async Task<bool> SendEmail(Application.Models.Email email)
		{
			var client = new SendGridClient(_emailSettings.ApiKey);
			var subject = email.Subject;
			var to = new EmailAddress(email.To);
			var emailBody = email.Body;

			var from = new EmailAddress
			{
				Email = _emailSettings.FromAddress,
				Name = _emailSettings.FromName,
			};

			var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
			var response = await client.SendEmailAsync(sendGridMessage);

			if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return true;
			}

			_logger.LogError("El email no pudo enviarse, existen errores");
			return false;
		}
	}
}
