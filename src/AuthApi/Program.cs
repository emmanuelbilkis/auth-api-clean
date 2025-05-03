using AuthApi.Data;
using AuthApi.Middlewares;
using AuthApi.Models.Options;
using AuthApi.Repository;
using AuthApi.Services.Email;
using AuthApi.Services.Smtp;
using AuthApi.Services.User;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SMTP"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration Entity
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("AuthDb"));

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", app =>
    {
        app.AllowAnyOrigin();
        app.AllowAnyHeader();
        app.AllowAnyMethod();
    });
});

// add services 
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<SmtpService>();  
var app = builder.Build();

// Configura el middleware para manejar excepciones globalmente
app.UseMiddleware<GlobalExceptionMiddleware>(); // Esto agrega tu middleware personalizado

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();




app.Run();
