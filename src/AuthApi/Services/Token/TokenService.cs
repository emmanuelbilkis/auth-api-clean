using AuthApi.Models.Db;
using AuthApi.Repository;
using AuthApi.Utils;
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
        public async Task<Result<string>> AddToken() 
        {
            var tokenModel =  TokenModelCreate(CrearToken());
            var token =  await _tokenRepository.AddToken(tokenModel);
            if (token is null) return Result<string>.Failure("No se pudo crear el token.");
            
            return Result<string>.Success(token.Token); 
        }
        private ActivationTokenModel TokenModelCreate(string token) 
        {
            ActivationTokenModel tokenModel = new ActivationTokenModel
            {
                  Token = token,
                  CreationDate = DateTime.UtcNow,
                  ExpirationDate = DateTime.UtcNow.AddHours(1),
                  Active = true
            };

            return tokenModel; 
        }
    }
}
