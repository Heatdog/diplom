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
        public IEnumerable<User> GetUsers()
        {
            return db.Users
                .Include(user => user.Department)
                .ToList();
        }
        public User? GetUserByUsername(string userName)
        {
            return db.Users
                .Include(user => user.Department)
                .FirstOrDefault(user => user.UserName == userName);
        }
        public User? GetUser(int userId)
        {
            return db.Users
                .Include(user => user.Department)
                .FirstOrDefault(user => user.Id == userId);
        }
        public User? GetUserByEmail(string email)
        {
            return db.Users
                .Include(user => user.Department)
                .FirstOrDefault(user => user.Email == email);
        }
        public Errors SetUser(User user)
        {
            var userName = db.Users.FirstOrDefault(us => us.UserName == user.UserName);
            if (userName != null) return Errors.InvalidUser;
            var email = db.Users.FirstOrDefault(us => us.Email == user.Email);
            if (email != null) return Errors.InvalidEmailAddress;
            db.Users.Add(user);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                return Errors.SaveDbError;
            }
            return Errors.None;
        }
        public Errors UpdateUser(User user)
        {
            var dbUser = db.Users.FirstOrDefault(us => us.Id == user.Id);
            if (dbUser == null) return Errors.InvalidUser;
            db.Users.Update(user);
            db.SaveChanges();
            return Errors.None;
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
