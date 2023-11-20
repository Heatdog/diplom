using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Electronic_document_management.Models
{
    public class DocumentFile
    {
        public DocumentFile() { }
        public DocumentFile(string path, Document doc) 
        {
            Path = path;
            Doc = doc;
            Version = doc.Version;
        }
        [Key]
        public int Id { get; set; }
        public string Path { get; set; }
        public int Version { get; set; }
        public Document Doc { get; set; }
        [ForeignKey("Document")]
        public int DocId { get; set; }
        public DateTime TimeInsert { get; set; }
    }
}
