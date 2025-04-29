using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

// Para más información sobre envío de correos con Gmail: https://www.youtube.com/watch?v=TWQAfMpxlXs&t=1289s
// Generar una "Contraseña de aplicación": https://myaccount.google.com/apppasswords

namespace CleanArchitecture.Infrastructure.Email
{
	public class GmailService : IEmailService
	{
		public EmailSettings _emailSettings { get; }
		public ILogger<GmailService> _logger { get; }

		public GmailService(IOptions<EmailSettings> emailSettings, ILogger<GmailService> logger)
		{
			_emailSettings = emailSettings.Value;
			_logger = logger;
		}

		public async Task<bool> SendEmail(Application.Models.Email email)
		{
			try
			{
				var fromEmail = _emailSettings.FromAddress;
				var password = _emailSettings.ApiKey;
				var message = new MailMessage();
				message.From = new MailAddress(fromEmail);
				message.Subject = email.Subject;
				message.To.Add(new MailAddress(email.To));
				message.Body = email.Body;
				// Si se quiere que tenga un formato HTML, se apliquen estilos y tags HTML
				message.IsBodyHtml = true;

				var smtpClient = new SmtpClient("smtp.gmail.com")
				{
					Port = 587,
					Credentials = new NetworkCredential(fromEmail, password),
					EnableSsl = true
				};

				await smtpClient.SendMailAsync(message);

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError($"El email no pudo enviarse, existen errores: {ex}");
				return false;
			}
		}
	}
}
