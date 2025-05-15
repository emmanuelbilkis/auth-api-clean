
using AuthApi.Models;
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

        private string CreateToken() 
        {
            var token = Guid.NewGuid();
            var tokenString = token.ToString();

            return tokenString?? string.Empty;
        }

        public Result<string> AddToken() 
        {
            string newTokenString = CreateToken();

            if (!string.IsNullOrEmpty(newTokenString))
            {
               var tokenModel = TokenModelCreate(newTokenString);
               var token = _tokenRepository.AddToken(tokenModel);
               
                return Result<string>.Success(token.Result.Token);
            }

            return Result<string>.Failure("No se pudo crear el token");
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
