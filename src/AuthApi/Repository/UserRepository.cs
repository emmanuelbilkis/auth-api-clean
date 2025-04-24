using AuthApi.Data;
using AuthApi.Models;
using AuthApi.Models.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<Result<UserModel>> Register(UserModel newUser)
        {
            try
            {
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync(); 

                return Result<UserModel>.Success(newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user"); 
                return Result<UserModel>.Failure(ex.Message);
            }
        }

        public async Task<Result<List<UserModel>>> GetAll()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Result<List<UserModel>>.Success(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users"); 
                return Result<List<UserModel>>.Failure(ex.Message); 
            }
        }
    }

}
