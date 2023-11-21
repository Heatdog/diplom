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
                .Include(doc => doc.Author.Department)
                .ToList();
        }
        public IEnumerable<Document> GetDocuments()
        {
            return db.Documents
                .Include(doc => doc.Author)
                .Include(doc => doc.Author.Department)
                .ToList();
        }
        public Document? GetDocument(int docId)
        {
            return db.Documents
                .Include(doc => doc.Author)
                .Include (doc => doc.Author.Department)
                .Include(doc => doc.DocumentFiles)
                .FirstOrDefault(doc => doc.Id == docId);
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

        public void UpdateDocumnet(Document document)
        {
            db.Documents.Update(document);
            db.SaveChanges();
        }

        public IEnumerable<Document> SearchDocumnets(string text)
        {
            return db.Documents
                .Where(doc => EF.Functions.ToTsVector("russian", doc.Name + " " + doc.Description)
                .Matches(text))
                .Include(doc => doc.Author)
                .Include(doc => doc.DocumentFiles)
                .Include(doc => doc.Author.Department)
                .ToList();
        }

        public IEnumerable<Document> SearchDocumnets(string text, int depId)
        {
            return db.Documents
                .Where(doc => doc.Author.DepartmentId == depId && 
                EF.Functions.ToTsVector("russian", doc.Name + " " + doc.Description)
                .Matches(text))
                .Include(doc => doc.Author)
                .Include(doc => doc.DocumentFiles)
                .Include(doc => doc.Author.Department)
                .ToList();
        }
    }
}
