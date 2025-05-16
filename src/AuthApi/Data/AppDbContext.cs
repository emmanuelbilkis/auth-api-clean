using AuthApi.Models;
using AuthApi.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ActivationTokenModel> ActivationTokens { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
