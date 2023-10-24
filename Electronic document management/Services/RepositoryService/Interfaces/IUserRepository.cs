using Electronic_document_management.Models;

namespace Electronic_document_management.Services.RepositoryService.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<IEnumerable<User>> GetUsersWithDepartmentAsync();
        Task<User?> GetUserWithDepartmentAsync(string userName);
        Task<User?> GetUserWithDepartmentAsync(int userId);
        Task<User?> GetUserAsync(string userName);
        Task<User?> GetUserAsync(int userId);
        Task<Errors> AddUserAsync(User user);
        Errors ConfirmUser(int userID, string role);
        Errors RemoveUser(int userId);
    }
}
