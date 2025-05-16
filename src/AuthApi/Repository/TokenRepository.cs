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
    }
}
