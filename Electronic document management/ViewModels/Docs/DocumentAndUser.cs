using Electronic_document_management.Models;

namespace Electronic_document_management.ViewModels.Docs
{
    public class DocumentAndUser
    {
        public DocumentAndUser(Document document, User user) 
        {
            Doc = document;
            User = user;
        }
        public Document Doc { get; set; }
        public User User { get; set; }

    }
}
