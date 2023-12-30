using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Electronic_document_management.Models
{
    public class Document
    {
        public Document() { }
        public Document(string name, User author, string description)
        {
            Name = name;
            Author = author;
            Description = description;
            DocumentFiles = new List<DocumentFile>();
            Status = Status.InDeveloping;
            Version = 1;
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public IEnumerable<DocumentFile> DocumentFiles { get; set; }
        public int Version { get; set; }
    }

    public enum Status
    {
        InDeveloping,
        OnConfirmation,
        Ready
    }
}
