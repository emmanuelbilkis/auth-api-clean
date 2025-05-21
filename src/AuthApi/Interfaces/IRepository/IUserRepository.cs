using AuthApi.Models.Db;

namespace AuthApi.Interfaces.IRepository
{
    public interface IUserRepository
    {
        Task<bool> ActivateUserAsync(UserModel user);
        Task<List<UserModel>> GetAll();
        Task<UserModel> GetUserForEmail(string email);
        Task<UserModel> Register(UserModel newUser);
    }
}