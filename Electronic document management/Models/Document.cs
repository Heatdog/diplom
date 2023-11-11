using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Electronic_document_management.Models
{
    public class Document
    {
        public Document() { }
        public Document(string name, string path, User author, string description)
        {
            Name = name;
            Path = path;
            Author = author;
            Description = description;
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        InDeveloping,
        OnConfirmation,
        Ready
    }
}
