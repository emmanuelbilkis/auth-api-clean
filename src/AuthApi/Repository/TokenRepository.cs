﻿using AuthApi.Data;
using AuthApi.Interfaces.IRepository;
using AuthApi.Models.Db;
using AuthApi.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AuthApi.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TokenRepository> _logger;

        public TokenRepository(AppDbContext context, ILogger<TokenRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ActivationTokenModel> AddToken(ActivationTokenModel activationTokenModel)
        {
            await _context.ActivationTokens.AddAsync(activationTokenModel);
            await _context.SaveChangesAsync();
            return activationTokenModel;
        }
        public async Task<ActivationTokenModel> GetTokenForUSer(UserModel user)
        {
            var token = await _context.ActivationTokens
                              .FirstOrDefaultAsync(t => t.User.Id == user.Id && t.ExpirationDate < DateTime.UtcNow);
            return token;
        }

        public async Task<bool> DeactivateTokenAsync(ActivationTokenModel token)
        {
            token.ExpirationDate = DateTime.UtcNow;
            return true;
        }

        public async Task<List<ActivationTokenModel>> GetAll()
        {
            return _context.ActivationTokens.ToList();
        }
    }
}
