using AuthApi.Dtos;
using AuthApi.Models.Db;
using AuthApi.Utils;

namespace AuthApi.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserModel>> Register(UserRegisterDto NewUser);
        Task<Result<bool>> ActivateAccountAsync(string email, string token);
        Task<Result<List<UserModel>>> GetAll();
    }
}
