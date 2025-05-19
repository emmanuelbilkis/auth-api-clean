using AuthApi.Dtos;
using AuthApi.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.CodeAnalysis;
using AuthApi.Validators;
using FluentValidation.Results;
using AuthApi.Utils;
using AuthApi.Services.Token;
using AuthApi.Services.Email;
using AuthApi.Models.Db;

namespace AuthApi.Services.User
{
    public class UserService
    { 
        private readonly UserRepository _repository;
        private readonly TokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly ILogger<UserService> _logger;
        public UserService(UserRepository repository,
                           TokenService tokenService,
                           EmailService emailService,
                           ILogger<UserService> logger
                           ) 
        { 
            _repository = repository; 
            _tokenService = tokenService;
            _emailService = emailService;
            _logger = logger; 
        }

        public async Task<Result<UserModel>> Register(UserRegisterDto NewUser) 
        {
            //validations
            var errorsResult = Validator(NewUser);
            if (!errorsResult.IsSuccessful) return Result<UserModel>.Failure(errorsResult.Error);
            
            //maps
            var UserToAdd = CreateModelUSer(NewUser);
            
            // call repository  
            var result = await _repository.Register(UserToAdd);
            if (result is null) return Result<UserModel>.Failure("No se pudo crear el usuario. Problemas al registrarlo en la Bd.");

            //call TokenService 
            var tokenResult = await _tokenService.AddToken();
            if (!tokenResult.IsSuccessful) return Result<UserModel>.Failure(tokenResult.Error);
   
            // call EmailService 
            var mailResult = await _emailService.SendActivationEmail(result.Name, result.Email, tokenResult.Value);
            if (!mailResult.IsSuccessful) return Result<UserModel>.Failure(mailResult.Error);
            
            return Result<UserModel>.Success(result); 
        }

        private Result<bool> Validator(UserRegisterDto NewUser) 
        {
            UserRegisterValidator validator = new UserRegisterValidator();
            ValidationResult resultValidator = validator.Validate(NewUser);

            if (!resultValidator.IsValid)
            {
                return Result<bool>.Failure(string.Join(" | ", resultValidator.Errors.Select(e => e.ErrorMessage)));
            } 

            return Result<bool>.Success(true);
        }


        private UserModel CreateModelUSer(UserRegisterDto NewUser) {
            
            UserModel UserToAdd = new UserModel
            {
                Id = NewUser.Id,
                Name = NewUser.Name,
                LastName = NewUser.LastName,
                Password = NewUser.Password,
                Email = NewUser.Email,
                DateOfBirth = NewUser.DateOfBirth
            };

            return UserToAdd;
        }

        public async Task<Result<List<UserModel>>> GetAll() 
        {  
            var result = await _repository.GetAll();    
            if (result is null) return Result<List<UserModel>>.Failure("Error inesperado"); 
            return Result<List<UserModel>>.Success(result);
        }
    }
}
