using Electronic_document_management.Models;
using Electronic_document_management.Services.Databases;
using Electronic_document_management.Services.RepositoryService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Electronic_document_management.Services.RepositoryService.Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        private ApplicationContext db;
        public DocumentRepository(ApplicationContext db)
        {
            this.db = db;
        }
        public IEnumerable<Document> GetDocuments(int depId)
        {
            return db.Documents
                .Include(doc => doc.Author)
                .Where(doc => doc.Author.DepartmentId == depId)
                .ToList();
        }
        public IEnumerable<Document> GetDocuments()
        {
            return db.Documents
                .Include(doc => doc.Author)
                .ToList();
        }
        public int? InsertDocument(Document document)
        {
            db.Documents.Add(document);
            try
            {
                db.SaveChanges();
            }catch (Exception)
            {
                return null;
            }
            return document.Id;
        }
    }
}
