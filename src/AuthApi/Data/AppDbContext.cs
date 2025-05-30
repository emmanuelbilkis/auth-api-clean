﻿using AuthApi.Models.Db;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
