using AuthApi.Models.Db;

namespace AuthApi.Interfaces.IRepository
{
    public interface ITokenRepository
    {
        Task<ActivationTokenModel> AddToken(ActivationTokenModel activationTokenModel);
        Task<bool> DeactivateTokenAsync(ActivationTokenModel token);
        Task<List<ActivationTokenModel>> GetAll();
        Task<ActivationTokenModel> GetTokenForUSer(UserModel user);
    }
}