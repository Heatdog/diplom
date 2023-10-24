using Electronic_document_management.Models;

namespace Electronic_document_management.Services.RepositoryService.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetDepartmentsAsync();
        Task<Department?> GetDepartmentAsync(string departmentName);
        Task<Department?> GetDepartmentWithUsersAsync(int id);
        Task<Errors> AddDepartmentAsync(Department department);
    }
}
