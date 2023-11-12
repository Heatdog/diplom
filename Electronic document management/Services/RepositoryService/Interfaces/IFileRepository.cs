using Electronic_document_management.Models;

namespace Electronic_document_management.Services.RepositoryService.Interfaces
{
    public interface IFileRepository
    {
        public Errors InsertFile(DocumentFile file);
    }
}
