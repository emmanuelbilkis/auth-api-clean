using AuthApi.Utils;

namespace AuthApi.Interfaces.IService
{
    public interface IEmailService
    {
        Task<Result<bool>> SendActivationEmail(string destName, string destEmail, string token, CancellationToken cancellationToken = default);
    }
}