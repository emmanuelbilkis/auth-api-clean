using AuthApi.Dtos;
using AuthApi.Services.Smtp;
using MimeKit;

namespace AuthApi.Services.Email
{
    public class EmailService
    {
        private readonly SmtpService _smtpService;
        private const string Source = "emmanuelbilkis@gmail.com"; 
        
        public EmailService(SmtpService smtpService) {
            _smtpService = smtpService; 
        }

        public async Task<bool> SendActivationEmail(string destName,string destEmail, string token, CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("App Web Register",Source));
            message.To.Add(new MailboxAddress(destName, destEmail));
            message.Subject = "Account activation";


            string activationUrl = $"https://localhost:7128/api/user/activate-account?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(destEmail)}";

            var builder = new BodyBuilder
            {
                HtmlBody = $"""
            <h1>¡Welcome to the AppWeb!</h1>
            <p>Click the button bellow to activate your account:</p>
            <p><a href='{activationUrl}'>Account activation</a></p>
            <p>If you didn't request this, please ignore the email.</p>
        """,
                TextBody = $"¡Welcome to the AppWeb!\n\nClick the button bellow to activate your account:\n{activationUrl}\n\nIf you didn't request this, please ignore the email."
            };

            message.Body = builder.ToMessageBody();

            await _smtpService.EnviarAsync(message,cancellationToken);
            return true;
        }
    }
}
