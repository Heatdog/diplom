using Electronic_document_management.Models;

namespace Electronic_document_management.Services.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<IEnumerable<User>> GetUsersWithDepAsync();
        Task<User?> GetUserAsync(string userName);
        Task<User?> GetUserAsync(int userId);
        Task<Errors> AddUserAsync(User user);
        Task<IEnumerable<Department>> GetDepartmentsAsync();
        Task<Department?> GetDepartmentAsync(string departmentName);
        Task<Department?> GetDepartmentWithUsersAsync(int id);
        Task<Errors> AddDepartmentAsync(Department department);
    }
}
