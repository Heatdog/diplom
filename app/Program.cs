using Electronic_document_management.Services.AuthService;
using Electronic_document_management.Services.Claims;
using Electronic_document_management.Services.Databases;
using Electronic_document_management.Services.FileService;
using Electronic_document_management.Services.PasswordHasher;
using Electronic_document_management.Services.QueryService;
using Electronic_document_management.Services.RepositoryService.Interfaces;
using Electronic_document_management.Services.RepositoryService.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

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
        

        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        builder.Services.AddTransient<IClaimService, ClaimService>();
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddTransient<IQueryRepository, QueryRepository>();
        builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
        builder.Services.AddTransient<IQueryService, QueryService>();
        builder.Services.AddTransient<IDocumentRepository, DocumentRepository>();
        builder.Services.AddTransient<IFileRepository, FileRepository>();
        builder.Services.AddTransient<IFileService, FileService>();
        
        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();
        
        

        app.MapControllers();

        app.Run();
    }
}