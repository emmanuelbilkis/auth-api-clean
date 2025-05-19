using AuthApi.Data;
using AuthApi.Models.Db;
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

        public async Task<UserModel> GetUserForEmail(string email) 
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Email == email);
            return user; 
        }   

        public async Task<List<UserModel>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<bool> Activar (UserModel user) 
        {
            user.IsActive = true; 
            _context.SaveChangesAsync();

            return true; 
        }
    }
}

