using AuthApi.Dtos;
using AuthApi.Models;
using AuthApi.Models.Utilities;
using AuthApi.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.CodeAnalysis;
using AuthApi.Validators;
using FluentValidation.Results;

namespace AuthApi.Services.User
{
    public class UserService
    {
        private readonly UserRepository _repository; 
        public UserService(UserRepository repository) { _repository = repository; }

        public async Task<Result<UserModel>> Register(UserRegisterDto NewUser) 
        {
            // validaicones de formatos y campos
            UserRegisterValidator validator = new UserRegisterValidator();
            ValidationResult resultValidator = validator.Validate(NewUser);

            if (!resultValidator.IsValid)
            {
                return Result<UserModel>.Failure(string.Join(" | ", resultValidator.Errors.Select(e => e.ErrorMessage)));
            }

            // luego vendran validaciones de regla de negocio 

            //mapeo 
            UserModel UserToAdd = new UserModel
            {
                Id = NewUser.Id,    
                Name = NewUser.Name,
                LastName = NewUser.LastName,
                Password = NewUser.Password,    
                Email = NewUser.Email,
                DateOfBirth = NewUser.DateOfBirth
            };
            
            //llamo al repositorio 
            var result = await _repository.Register(UserToAdd);

            return Result<UserModel>.Success(result); 
        }

        public async Task<Result<List<UserModel>>> GetAll() 
        {
            // validar  
            var result = await _repository.GetAll();    
            if (result is null) return Result<List<UserModel>>.Failure("Error inesperado"); 
            return Result<List<UserModel>>.Success(result);
        }
    }
}
