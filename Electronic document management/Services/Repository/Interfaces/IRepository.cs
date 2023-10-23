using Electronic_document_management.Models;

namespace Electronic_document_management.Services.Repository.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserAsync(string userName);
        Task<User?> GetUserAsync(int  userId);
        Task<Errors> AddUserAsync(User user);
        Task<IEnumerable<Department>> GetDepartmentsAsync();
        Task<Department?> GetDepartmentAsync(string departmentName);
        Task<Errors> AddDepartmentAsync(Department department);
    }
}
