using System.ComponentModel.DataAnnotations.Schema;

namespace Electronic_document_management.Models
{
    public class Query
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public Query(User user, Department dep) 
        {
            User = user;
            Department = dep;
        }
        public Query() { }
    }
}
