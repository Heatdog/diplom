using Electronic_document_management.Services.AuthService;
using Electronic_document_management.Services.Claims;
using Electronic_document_management.Services.Databases;
using Electronic_document_management.Services.PasswordHasher;
using Electronic_document_management.Services.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/accessdenied";
            });
        builder.Services.AddAuthorization();
        

        builder.Services.AddTransient<IRepository, Repository>();
        builder.Services.AddTransient<IClaimService, ClaimService>();
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
        
        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}