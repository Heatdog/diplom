using Electronic_document_management.Models;
using Electronic_document_management.Services.Databases;
using Electronic_document_management.Services.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Electronic_document_management.Services.Repository.Repos
{
    public class Repository : IRepository
    {
        private ApplicationContext db;
        public Repository(ApplicationContext db)
        {
            this.db = db;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await db.Users.ToListAsync();
        }
        public async Task<User?> GetUserAsync(string userName)
        {
            return await db.Users
                .Include(user => user.Department)
                .FirstOrDefaultAsync(user => user.UserName == userName);
        }
        public async Task<User?> GetUserAsync(int userId)
        {
            return await db.Users
                .Include(user => user.Department)
                .FirstOrDefaultAsync(user => user.Id == userId);
        }
        public async Task<Errors> AddUserAsync(User user)
        {
            var userName = db.Users.FirstOrDefault(us => us.UserName == user.UserName);
            if (userName != null) return Errors.InvalidUser;
            var email = db.Users.FirstOrDefault(us => us.Email == user.Email);
            if (email != null) return Errors.InvalidEmailAddress;
            db.Users.Add(user);
            try
            {
                await db.SaveChangesAsync();
            }catch (Exception)
            {
                return Errors.SaveDbError;
            }
            return Errors.None;
        }
        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            return await db.Departments.ToListAsync();
        }

        public async Task<Department?> GetDepartmentAsync(string departmentName)
        {
            return await db.Departments
                .Include(dep => dep.Users)
                .FirstOrDefaultAsync(dp => dp.Name == departmentName);
        }

        public async Task<Errors> AddDepartmentAsync(Department department)
        {
            if (db.Departments.FirstOrDefault(dep => dep.Name == department.Name) != null)
                return Errors.InvalidDepartment;
            db.Departments.Add(department);
            try
            {
                await db.SaveChangesAsync();
            }catch (Exception)
            {
                return Errors.SaveDbError;
            }
            return Errors.None;
        }
    }
}
