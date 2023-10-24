using Electronic_document_management.Models;
using Electronic_document_management.Services.PasswordHasher;
using Microsoft.EntityFrameworkCore;

namespace Electronic_document_management.Services.Databases
{
    public class ApplicationContext : DbContext
    {
        private readonly IPasswordHasher _passwordHasher;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Query> Queries { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options, IPasswordHasher passwordHasher)
            : base(options)
        {
            _passwordHasher = passwordHasher;
            Database.EnsureCreated();  
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasAlternateKey(user => user.UserName);
            modelBuilder.Entity<User>().HasAlternateKey(user => user.Email);
            modelBuilder.Entity<User>()
                .Property(user => user.Created)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<Department>().HasAlternateKey(dep => dep.Name);
            modelBuilder.Entity<Department>()
                .HasMany(dep => dep.Users)
                .WithOne(user => user.Department)
                .HasForeignKey(user => user.DepartmentId)
                .IsRequired(false);

            var mainDp = new Department("Главный отдел");
            mainDp.DepartmentId = 1;
            var infDp = new Department("Отдел информационных технологий");
            infDp.DepartmentId = 2;
            var admin = new User("Admin", "Admin", "Admin", "bleckbird9@yandex.ru", _passwordHasher.HashPassword("1"), 
                true, Role.Admin);
            admin.Id = 1;
            var head = new User { DepartmentId = 1, Name = "Александр", Id = 2, Surname = "Овсянников",
                Email = "bleckbird19@gmail.com", Password = _passwordHasher.HashPassword("2"), Role = Role.HeadOfDepartment,
                UserName = "Heater", IsConfirmed = true
            };
            modelBuilder.Entity<Department>().HasData(mainDp, infDp);
            modelBuilder.Entity<User>().HasData(admin, head);
        }
    }
}
