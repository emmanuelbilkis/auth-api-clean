using AuthApi.Models.Db;
using AuthApi.Utils;

namespace AuthApi.Interfaces.IService
{
    public interface ITokenService
    {
        Task<Result<bool>> DeactivateTokenAsync(ActivationTokenModel token);
        Task<Result<ActivationTokenModel>> GetActiveTokenForUserAsync(UserModel user);
        Task<Result<List<ActivationTokenModel>>> GetAll();
        Task<Result<ActivationTokenModel>> TokenCreate();
    }
}