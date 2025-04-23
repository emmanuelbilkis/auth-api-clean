using AuthApi.Dtos;
using AuthApi.Models;
using AuthApi.Repository;
using System.Diagnostics.CodeAnalysis;

namespace AuthApi.Services.User
{
    public class UserService
    {
        private readonly UserRepository _repository; 
        public UserService(UserRepository repository) { _repository = repository; }

        public bool Register(UserRegisterDto NewUser) 
        {
            // validar 
            
            // mapear

            UserModel UserToAdd = new UserModel
            {
                Name = NewUser.Name,
                LastName = NewUser.LastName,
                Password = NewUser.Password,    
                Email = NewUser.Email,
                DateOfBirth = NewUser.DateOfBirth
            };   

            var UserNew = _repository.Register(UserToAdd);

            return true;    
        }
    }
}
