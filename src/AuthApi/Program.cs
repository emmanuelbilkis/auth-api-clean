using AuthApi.Data;
using AuthApi.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Configuration Entity
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("AuthDb"));

// add services 
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<UserRepository>();


app.Run();
