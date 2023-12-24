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
        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<DocumentFile> Files { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options, IPasswordHasher passwordHasher)
            : base(options)
        {
            _passwordHasher = passwordHasher;
            Database.EnsureCreated();  
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasAlternateKey(user => user.UserName);
            modelBuilder.Entity<User>().HasIndex(user => user.Email).IsUnique();
            modelBuilder.Entity<User>()
                .Property(user => user.Created)
                .HasDefaultValueSql("NOW()");
            modelBuilder.Entity<User>()
                .HasMany(user => user.Docs)
                .WithOne(docs => docs.Author)
                .HasForeignKey(docs => docs.AuthorId)
                .IsRequired(true);

            modelBuilder.Entity<Department>().HasAlternateKey(dep => dep.Name);
            modelBuilder.Entity<Department>()
                .HasMany(dep => dep.Users)
                .WithOne(user => user.Department)
                .HasForeignKey(user => user.DepartmentId)
                .IsRequired(true);

            modelBuilder.Entity<Document>()
                .Property(doc => doc.Created)
                .HasDefaultValueSql("NOW()");
            modelBuilder.Entity<Document>()
                .HasMany(doc => doc.DocumentFiles)
                .WithOne(file => file.Doc)
                .HasForeignKey(file => file.DocId)
                .IsRequired(true);
            modelBuilder.Entity<Document>()
                .HasIndex(doc => new { doc.Name, doc.Description })
                .HasMethod("GIN")
                .IsTsVectorExpressionIndex("russian");

            modelBuilder.Entity<DocumentFile>()
                .Property(file => file.TimeInsert)
                .HasDefaultValueSql("NOW()");


            var mainDp = new Department("Главный отдел");
            mainDp.DepartmentId = 1;
            var infDp = new Department("Отдел информационных технологий");
            infDp.DepartmentId = 2;
            var admin = new User()
            {
                UserName = "Admin",
                Name = "Admin",
                Surname = "Admin",
                Email = "bleckbird9@yandex.ru",
                Password = _passwordHasher.HashPassword("1"),
                IsConfirmed = true,
                Role = Role.Admin,
                DepartmentId = mainDp.DepartmentId
            };
            admin.Id = 1;
            var head = new User()
            {
                UserName = "Heater",
                Name = "Александр",
                Surname = "Овсянников",
                Email = "bleckbird19@gmail.com",
                Password = _passwordHasher.HashPassword("2"),
                IsConfirmed = true,
                Role = Role.HeadOfDepartment,
                DepartmentId = infDp.DepartmentId
            };
            head.Id = 2;
            modelBuilder.Entity<Department>().HasData(mainDp, infDp);
            modelBuilder.Entity<User>().HasData(admin, head);
        }
    }
}
