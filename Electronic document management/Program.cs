using Electronic_document_management.Services.Auth.Interfaces;
using Electronic_document_management.Services.Auth.Jwt;
using Electronic_document_management.Services.Databases;
using Electronic_document_management.Services.PasswordHasher.Hasher;
using Electronic_document_management.Services.PasswordHasher.Interfaces;
using Electronic_document_management.Services.Repository.Interfaces;
using Electronic_document_management.Services.Repository.Repos;
using Electronic_document_management.Services.Tokens.Interfaces;
using Electronic_document_management.Services.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

class Programm
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
        if (connection == null)
        {
            Console.WriteLine("Connection did not specified");
            return;
        }

        builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseNpgsql(connection));

        builder.Services.AddControllersWithViews();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
                options.SaveToken = true;
            });
        

        builder.Services.AddTransient<IRepository, Repository>();
        builder.Services.AddTransient<ITokenService, JwtTokenService>();
        builder.Services.AddTransient<IAuthService, JwtAuthService>();
        builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
        
        var app = builder.Build();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}