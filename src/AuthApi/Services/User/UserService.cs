using AuthApi.Dtos;
using AuthApi.Models;
using AuthApi.Models.Utilities;
using AuthApi.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.CodeAnalysis;

namespace AuthApi.Services.User
{
    public class UserService
    {
        private readonly UserRepository _repository; 
        public UserService(UserRepository repository) { _repository = repository; }

        public async Task<Result<UserModel>> Register(UserRegisterDto NewUser) 
        {
            // validar - aca llamare al servicio de validaciones mas adelante  
            
            UserModel UserToAdd = new UserModel
            {
                Id = NewUser.Id,    
                Name = NewUser.Name,
                LastName = NewUser.LastName,
                Password = NewUser.Password,    
                Email = NewUser.Email,
                DateOfBirth = NewUser.DateOfBirth
            };   

            var result = await _repository.Register(UserToAdd);
            if (!result.IsSuccessful) return Result<UserModel>.Failure(result.Error);

            return Result<UserModel>.Success(result.Value); 
        }

        public async Task<Result<List<UserModel>>> GetAll() 
        {
            // validar  
            var result = await _repository.GetAll();    
            if (!result.IsSuccessful) return Result<List<UserModel>>.Failure(result.Error); 
            return Result<List<UserModel>>.Success(result.Value);
        }
    }
}
