using Electronic_document_management.Models;
using Electronic_document_management.Services.Databases;
using Electronic_document_management.Services.RepositoryService.Interfaces;

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
    }
}
