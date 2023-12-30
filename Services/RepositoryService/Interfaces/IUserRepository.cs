using Electronic_document_management.Models;

namespace Electronic_document_management.Services.RepositoryService.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User? GetUserByUsername(string userName);
        User? GetUser(int userId);
        User? GetUserByEmail(string email);
        Errors SetUser(User user);
        Errors UpdateUser(User user);
        Errors RemoveUser(int userId);
    }
}
