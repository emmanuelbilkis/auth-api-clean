using AuthApi.Models.Options;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace AuthApi.Services.Smtp
{
    public class SmtpService
    {
        public readonly SmtpOptions _options;

        public SmtpService(IOptionsMonitor<SmtpOptions> options)
        {
            _options = options.CurrentValue;
        }   // IoptionMonitor nos permite que si en el archivo de configuracion cambia algo, esta configuracion podra ser cargada nuevamente sin necesidad de reiniciar la app

        public async Task EnviarAsync(MimeMessage message, CancellationToken cancellationToken = default)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_options.Server, _options.Port, _options.UseSsl, cancellationToken);

                if (!string.IsNullOrEmpty(_options.Username))
                    await client.AuthenticateAsync(_options.Username, _options.Password, cancellationToken);

                await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);
            }
        }

    }
}
