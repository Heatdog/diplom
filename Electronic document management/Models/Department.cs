using System.ComponentModel.DataAnnotations;

namespace Electronic_document_management.Models
{
    public class Department
    {
        public Department(string name) {
            Name = name;
            Users = new List<User>();
        }
        public Department(string name, List<User> users) : this(name) 
        { 
            Users.AddRange(users);
        }
        [Key]
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }

    }
}
