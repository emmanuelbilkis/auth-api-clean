using AuthApi.Dtos;
using AuthApi.Interfaces.IService;
using AuthApi.Models.Options;
using AuthApi.Services.Smtp;
using AuthApi.Utils;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AuthApi.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpService _smtpService;
        private readonly string _sourceEmail;

        public EmailService(SmtpService smtpService, IOptions<SmtpOptions> smtpOptions)
        {
            _smtpService = smtpService;
            _sourceEmail = smtpOptions.Value.SourceEmail;
        }

        public async Task<Result<bool>> SendActivationEmail(string destName, string destEmail, string token, CancellationToken cancellationToken = default)
        {

            string activationUrl = CreateUrl(token, destEmail);
            var message = CreateMessage(destName, destEmail, activationUrl);
            var result = await _smtpService.EnviarAsync(message, cancellationToken);

            if (!result.IsSuccessful)
            {
                return Result<bool>.Failure(result.Error);
            }

            return Result<bool>.Success(true);
        }

        private string CreateUrl(string token, string destEmail) => $"https://localhost:7128/api/user/activate-account?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(destEmail)}";
        private MimeMessage CreateMessage(string destName, string destEmail, string url)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("App Web Register", _sourceEmail));
            message.To.Add(new MailboxAddress(destName, destEmail));
            message.Subject = "Account activation";

            message = InsertBody(message, url);

            return message;
        }

        private MimeMessage InsertBody(MimeMessage message, string url)
        {
            var builder = new BodyBuilder
            {
                HtmlBody = $"""
                <h1>¡Welcome to the AppWeb!</h1>
                <p>Click the button bellow to activate your account:</p>
                <p><a href='{url}'>Account activation</a></p>
                <p>If you didn't request this, please ignore the email.</p>
                """,
                TextBody = $"¡Welcome to the AppWeb!\n\nClick the button bellow to activate your account:\n{url}\n\nIf you didn't request this, please ignore the email."
            };

            message.Body = builder.ToMessageBody();
            return message;
        }
    }
}
