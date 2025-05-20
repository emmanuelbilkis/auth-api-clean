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
using Microsoft.EntityFrameworkCore;
using AuthApi.Data;
using AuthApi.Interfaces;

namespace AuthApi.Services.User
{
    public class UserService : IUserService 
    { 
        private readonly UserRepository _repository;
        private readonly TokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly ILogger<UserService> _logger;
        private readonly AppDbContext _context;
        public UserService(UserRepository repository,
                           TokenService tokenService,
                           EmailService emailService,
                           ILogger<UserService> logger,
                           AppDbContext context
                           ) 
        { 
            _repository = repository; 
            _tokenService = tokenService;
            _emailService = emailService;
            _logger = logger; 
            _context = context;
        }

        public async Task<Result<UserModel>> Register(UserRegisterDto NewUser) 
        {
            //validations
            var errorsResult = Validator(NewUser);
            if (!errorsResult.IsSuccessful) return Result<UserModel>.Failure(errorsResult.Error);

            // Aca comienza la transacción para manejar dos actualizaciones en la bd
            //using var transaction = await _context.Database.BeginTransactionAsync();

            //call TokenService - crea un topken en memoria
            var tokenResult = await _tokenService.TokenCreate();
            if (!tokenResult.IsSuccessful) return Result<UserModel>.Failure(tokenResult.Error);
            
            //maps
            var UserToAdd = CreateModelUSer(NewUser, tokenResult.Value);

            // call repository para registrar el usuario en la bd  
            var result = await _repository.Register(UserToAdd);
            if (result is null) return Result<UserModel>.Failure("No se pudo crear el usuario. Problemas al registrarlo en la Bd.");
   
            // call EmailService 
            var mailResult = await _emailService.SendActivationEmail(result.Name, result.Email, tokenResult.Value.Token);
            if (!mailResult.IsSuccessful) return Result<UserModel>.Failure(mailResult.Error);

            //await transaction.CommitAsync();

            return Result<UserModel>.Success(result); 
        }

        public async Task<Result<bool>> ActivateAccountAsync(string email,string token) 
        {
            // traigo el usuario por mail y controlo q exista 
            var user = await _repository.GetUserForEmail(email);
            if (user == null) return Result<bool>.Failure("No existe el usuario con el email solicitado.");

            //con el usuario obtengo el unico token activo del mismo (puede tener varios token pero solo uno activo para usar)
            // no hago aca el control de si el token esta vencido o no, ya que en la consulta de entity traigo solo el unico token activado, si es que existe, sino traera null.
            var activeTokenResult = await _tokenService.GetActiveTokenForUserAsync(user);
            if (!activeTokenResult.IsSuccessful) return Result<bool>.Failure(activeTokenResult.Error);
            
            // contorlo q el token corresponda al mandado por el usuario usuario con respecto al q tiene en la bd. en esta instancia existe y es activo
            if(activeTokenResult.Value.Token != token) return Result<bool>.Failure("El token no corresponde.");

            // Aca comienza la transacción para manejar dos actualizaciones en la bd
            //using var transaction = await _context.Database.BeginTransactionAsync();

            // se activa la cuenta del usuario
            var userActivationResult = await _repository.ActivateUserAsync(user);
            if (!userActivationResult) { 
                //await transaction.RollbackAsync();
                return Result<bool>.Failure($"Hubo problemas al activar el usuario de correo: {email}");
            }
            
            //se desactiva el token previamente usado 
            var tokenDesactivationResult = await _tokenService.DeactivateTokenAsync(activeTokenResult.Value);
            if (!tokenDesactivationResult.IsSuccessful) 
            {
                //await transaction.RollbackAsync();
                return Result<bool>.Failure($"Hubo problemas al desactivar el token. No se pudo activar el usuario: {email}");
            }

            //await transaction.CommitAsync();

            // todo salio bien se retorna true 
            return Result<bool>.Success(true);  
        }

        public async Task<Result<List<UserModel>>> GetAll()
        {
            var result = await _repository.GetAll();
            if (result is null) return Result<List<UserModel>>.Failure("Error inesperado");
            return Result<List<UserModel>>.Success(result);
        }

        private UserModel CreateModelUSer(UserRegisterDto NewUser, ActivationTokenModel token) 
        {
            UserModel UserToAdd = new UserModel
            {
                Name = NewUser.Name,
                LastName = NewUser.LastName,
                Password = NewUser.Password,
                Email = NewUser.Email,
                DateOfBirth = NewUser.DateOfBirth
            };

            UserToAdd.ActivationTokens.Add(token);  

            return UserToAdd;
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
    }
}
