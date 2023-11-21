using Electronic_document_management.Models;
using Electronic_document_management.Services.Databases;
using Electronic_document_management.Services.RepositoryService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Electronic_document_management.Services.RepositoryService.Repository
{
    public class FileRepository : IFileRepository
    {
        private ApplicationContext db;
        public FileRepository(ApplicationContext db) 
        {
            this.db = db;
        }
        public Errors InsertFile(DocumentFile file)
        {
            db.Files.Add(file);
            try
            {
                db.SaveChanges();
            }catch (Exception) 
            {
                return Errors.SaveDbError;
            }
            return Errors.None;
        }
        public bool RemoveFile(int fileId)
        {
            var file = db.Files.FirstOrDefault(f => f.Id == fileId);
            if (file == null)
            {
                return false;
            }
            db.Files.Remove(file);
            db.SaveChanges();
            return true;
        }

        public DocumentFile? GetFile(int fileId)
        {
            return db.Files
                .Include(f => f.Doc)
                .Include(f => f.Doc.Author)
                .FirstOrDefault(f => f.Id == fileId);
        }
    }
}
