using Electronic_document_management.Models;

namespace Electronic_document_management.ViewModels.Register
{
    public class RegisterModel
    {
        public IEnumerable<Department> Departments { get; set; }
        public AuthResult? AuthResult { get; set; }
        public RegisterModel(AuthResult? result, IEnumerable<Department> departments)
        {
            AuthResult = result;
            Departments = departments;
        }
    }
}
