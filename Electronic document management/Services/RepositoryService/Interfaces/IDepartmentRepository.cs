using Electronic_document_management.Models;

namespace Electronic_document_management.Services.RepositoryService.Interfaces
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetDepartments();
        Department? GetDepartment(string departmentName);
        Department? GetDepartment(int id);
        Errors SetDepartment(Department department);
    }
}
