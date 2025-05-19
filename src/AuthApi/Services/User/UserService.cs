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

        public async Task<Result<bool>> ActiveCount(string email,string token) 
        {
            // traigo el usuario por mail y controlo q exista 
            var user = await _repository.GetUserForEmail(email);
            if (user == null) return Result<bool>.Failure("No existe el usuario con el email solicitado.");

            //con el usuario obtengo el unico token activo del mismo (puede tener varios token pero solo uno activo para usar)
            // no hago aca el control e si esta vencido o no ya que en la consulta de entyti traigoo solo el token activado siu es q hay sino traera null
            var tokenResult = await _tokenService.GetTokenForUSer(user);
            if (!tokenResult.IsSuccessful) return Result<bool>.Failure(tokenResult.Error);
            
            // contorlo q el token corresponda al mandado por el usuario usuario con respecto al q tiene en la bd. en esta instancia existe y es activo
            if(tokenResult.Value.Token != token) return Result<bool>.Failure("El token no corresponde.");

            // se activa la cuenta del usuario
            var userActivationResult = await _repository.Activar(user);
            if (!userActivationResult) return Result<bool>.Failure("Hubo problemas al activar el usuario.");
            
            //se desactiva el token previamente usado 
            var tokenDesactivationResult = await _tokenService.Desactivar(tokenResult.Value);
            if (!tokenDesactivationResult.IsSuccessful) return Result<bool>.Failure("Hubo problemas al desactivar el token."); 

            // todo salio bien se retorna true 
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
