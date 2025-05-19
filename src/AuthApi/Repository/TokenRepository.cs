using AuthApi.Data;
using AuthApi.Models.Db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AuthApi.Repository
{
    public class TokenRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TokenRepository> _logger;

        public TokenRepository(AppDbContext context, ILogger<TokenRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ActivationTokenModel> AddToken (ActivationTokenModel activationTokenModel)
        {
            await _context.ActivationTokens.AddAsync (activationTokenModel);
            await _context.SaveChangesAsync(); 
            return activationTokenModel;    
        }
        public async Task<ActivationTokenModel> GetTokenForUSer(UserModel user)
        {
            var token = await _context.ActivationTokens
                              .FirstOrDefaultAsync(t => t.UserId == user.Id && t.ExpirationDate < DateTime.UtcNow);
            return token;
        }

        public async Task<bool> Desactivar(ActivationTokenModel token) 
        {
            token.ExpirationDate = DateTime.UtcNow;
            return true; 
        }
    }
}
