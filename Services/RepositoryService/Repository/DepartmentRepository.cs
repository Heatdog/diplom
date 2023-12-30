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
        public IEnumerable<Department> GetDepartments()
        {
            return db.Departments.ToList();
        }

        public Department? GetDepartment(string departmentName)
        {
            return db.Departments
                .Include(dep => dep.Users.Where(user => user.IsConfirmed == true))
                .FirstOrDefault(dp => dp.Name == departmentName);
        }

        public Department? GetDepartment(int id)
        {
            return db.Departments
                .Include(dep => dep.Users.Where(user => user.IsConfirmed == true))
                .FirstOrDefault(dp => dp.DepartmentId == id);
        }

        public Errors SetDepartment(Department department)
        {
            if (db.Departments.FirstOrDefault(dep => dep.Name == department.Name) != null)
                return Errors.InvalidDepartment;
            db.Departments.Add(department);
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
    }
}
