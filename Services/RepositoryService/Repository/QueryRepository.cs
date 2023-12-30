using Electronic_document_management.Models;
using Electronic_document_management.Services.Databases;
using Electronic_document_management.Services.RepositoryService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Electronic_document_management.Services.RepositoryService.Repository
{
    public class QueryRepository : IQueryRepository
    {
        private ApplicationContext db;
        public QueryRepository(ApplicationContext db)
        {
            this.db = db;
        }
        public IEnumerable<Query> GetQueries()
        {
            return db.Queries
                .Include(query => query.User)
                .Include(query => query.Department)
                .ToList();
        }
        public IEnumerable<Query> GetQueries(int depId)
        {
            return db.Queries
                .Where(query => query.DepartmentId == depId)
                .Include(query => query.User)
                .Include (query => query.Department)
                .ToList();
        }
        public void AddQuery(Query query)
        {
            db.Queries.Add(query);
            db.SaveChanges();
        }

        public bool RemoveQuery(int queryId)
        {
            var query = db.Queries.FirstOrDefault(query => query.Id == queryId);
            if (query == null)
                return false;
            db.Queries.Remove(query);
            db.SaveChanges();
            return true;
        }

        public Query? GetQuery(int queryId)
        {
            return db.Queries
                .Include (query => query.User)
                .Include(query => query.Department)
                .FirstOrDefault(query => query.Id == queryId);
        }
    }
}
