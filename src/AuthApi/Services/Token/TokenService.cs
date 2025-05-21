using AuthApi.Interfaces.IService;
using AuthApi.Models.Db;
using AuthApi.Repository;
using AuthApi.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace AuthApi.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly TokenRepository _tokenRepository;

        public TokenService(TokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }
        private string TokenStringCreate() => Guid.NewGuid().ToString();
        public async Task<Result<ActivationTokenModel>> TokenCreate()
        {
            var tokenModel = TokenModelCreate();
            if (tokenModel is null) return Result<ActivationTokenModel>.Failure("No se pudo crear el token.");

            return Result<ActivationTokenModel>.Success(tokenModel);
        }
        private ActivationTokenModel TokenModelCreate()
        {
            ActivationTokenModel tokenModel = new ActivationTokenModel
            {
                Token = TokenStringCreate(),
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

        public async Task<Result<List<ActivationTokenModel>>> GetAll()
        {
            var result = await _tokenRepository.GetAll();
            if (!result) return Result<bool>.Failure("No se pudo obtener los tokens. Revisar conexion a la Bd.");

            return Result<List<ActivationTokenModel>>.Success(result);
        }
    }
}
