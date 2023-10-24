using Electronic_document_management.Models;
using Electronic_document_management.Services.Databases;
using Electronic_document_management.Services.RepositoryService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Electronic_document_management.Services.RepositoryService.Repository
{
    public class UserRepository : IUserRepository
    {
        private ApplicationContext db;
        public UserRepository(ApplicationContext db)
        {
            this.db = db;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await db.Users
                .ToListAsync();
        }
        public async Task<IEnumerable<User>> GetUsersWithDepartmentAsync()
        {
            return await db.Users
                .Include(user => user.Department)
                .ToListAsync();
        }
        public async Task<User?> GetUserWithDepartmentAsync(string userName)
        {
            return await db.Users
                .Include(user => user.Department)
                .FirstOrDefaultAsync(user => user.UserName == userName);
        }
        public async Task<User?> GetUserWithDepartmentAsync(int userId)
        {
            return await db.Users
                .Include(user => user.Department)
                .FirstOrDefaultAsync(user => user.Id == userId);
        }
        public async Task<User?> GetUserAsync(string userName)
        {
            return await db.Users
                .FirstOrDefaultAsync(user => user.UserName == userName);
        }
        public async Task<User?> GetUserAsync(int userId)
        {
            return await db.Users
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
            }
            catch (Exception)
            {
                return Errors.SaveDbError;
            }
            return Errors.None;
        }
        public Errors ConfirmUser(int userID, string role)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == userID);
            if (user == null) 
            {
                return Errors.InvalidUser;
            }
            else
            {
                user.Role = RoleTransform.RoleToEnum(role);
                user.IsConfirmed = true;
                db.SaveChanges();
                return Errors.None;
            }
        }

        public Errors RemoveUser(int userId)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return Errors.InvalidUser;
            }
            else
            {
                db.Users.Remove(user);
                db.SaveChanges();
                return Errors.None;
            }
        }
    }
}
