using AuthApi.Data;
using AuthApi.Models;
//using AuthApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AuthApi.Repository
{
    public class UserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserModel> Register(UserModel newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<List<UserModel>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }
    }
}

