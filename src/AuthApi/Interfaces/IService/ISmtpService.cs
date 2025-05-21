using AuthApi.Utils;
using MimeKit;

namespace AuthApi.Interfaces.IService
{
    public interface ISmtpService
    {
        Task<Result<bool>> EnviarAsync(MimeMessage message, CancellationToken cancellationToken = default);
    }
}