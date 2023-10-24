using Electronic_document_management.Models;
using Electronic_document_management.Services.Databases;
using Electronic_document_management.Services.RepositoryService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Electronic_document_management.Services.RepositoryService.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private ApplicationContext db;
        public DepartmentRepository(ApplicationContext db)
        {
            this.db = db;
        }
        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            return await db.Departments.ToListAsync();
        }

        public async Task<Department?> GetDepartmentAsync(string departmentName)
        {
            return await db.Departments
                .FirstOrDefaultAsync(dp => dp.Name == departmentName);
        }

        public async Task<Department?> GetDepartmentWithUsersAsync(int id)
        {
            return await db.Departments
                .Include(dep => dep.Users.Where(user => user.IsConfirmed == true))
                .FirstOrDefaultAsync(dp => dp.DepartmentId == id);
        }

        public async Task<Errors> AddDepartmentAsync(Department department)
        {
            if (db.Departments.FirstOrDefault(dep => dep.Name == department.Name) != null)
                return Errors.InvalidDepartment;
            db.Departments.Add(department);
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
    }
}
