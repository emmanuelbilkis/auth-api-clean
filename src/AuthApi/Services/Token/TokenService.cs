using AuthApi.Models.Db;
using AuthApi.Repository;
using AuthApi.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace AuthApi.Services.Token
{
    public class TokenService
    {
        private readonly TokenRepository _tokenRepository; 
        
        public TokenService (TokenRepository tokenRepository) 
        {
            _tokenRepository = tokenRepository; 
        }
        private string CrearToken() => Guid.NewGuid().ToString();  
        public async Task<Result<ActivationTokenModel>> AddToken(UserModel user) 
        {
            var tokenModel =  TokenModelCreate(CrearToken(),user);
            var token =  await _tokenRepository.AddToken(tokenModel);
            if (token is null) return Result<ActivationTokenModel>.Failure("No se pudo crear el token.");
            
            return Result<ActivationTokenModel>.Success(token); 
        }
        private ActivationTokenModel TokenModelCreate(string token, UserModel user) 
        {
            ActivationTokenModel tokenModel = new ActivationTokenModel
            {
                  Token = token,
                  User = user,
                  UserId = user.Id,
                  CreationDate = DateTime.UtcNow,
                  ExpirationDate = DateTime.UtcNow.AddHours(1),
            };

            return tokenModel; 
        }

        public async Task<Result<ActivationTokenModel>> GetActiveTokenForUserAsync(UserModel user) 
        {
            var token = await _tokenRepository.GetTokenForUSer(user);
            if (token is null) return Result<ActivationTokenModel>.Failure("No se encontro el token activo con el usuario solicitado.");

            return Result<ActivationTokenModel>.Success(token); 
        }

        public async Task<Result<bool>> DeactivateTokenAsync(ActivationTokenModel token)
        {
            var result = await _tokenRepository.DeactivateTokenAsync(token);
            if (!result) return Result<bool>.Failure("No se pudo desactivar el Token. Consultar con el desarrollador de backend encargado.");
            
            return Result<bool>.Success(result);
        }
    }
}
