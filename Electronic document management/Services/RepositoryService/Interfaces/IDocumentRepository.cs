using Electronic_document_management.Models;

namespace Electronic_document_management.Services.RepositoryService.Interfaces
{
    public interface IDocumentRepository
    {
        public IEnumerable<Document> GetDocuments(int depId);
        public IEnumerable<Document> GetDocuments();
        public Document? GetDocument(int docId);
        public int? InsertDocument(Document document);
        public void UpdateDocumnet(Document document);
    }
}
