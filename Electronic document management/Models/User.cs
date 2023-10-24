using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Electronic_document_management.Models
{
    public class User
    {
        public User() { }
        public User(string userName, string name, string surname, string email, 
            string password, bool isConfirmed, Role role = Role.Worker)
        {
            UserName = userName;
            Name = name;
            Surname = surname;
            Password = password;
            Email = email;
            Role = role;
            Department = null;
            DepartmentId = null;
            IsConfirmed = isConfirmed;
        }
        
        public User(string userName, string name, string surname, string email,
            string password, bool isConfirmed, Role role, Department department)
            :this(userName, name, surname, email, password, isConfirmed, role)
        {
            Department = department;
            DepartmentId = department.DepartmentId;
        }
        
        [Key]
        public int Id { get; set; }
        [MinLength(2), MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(50)]
        public string Surname { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime Created { get; set; }
    }
}
