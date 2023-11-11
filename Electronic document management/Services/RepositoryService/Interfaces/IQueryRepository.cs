using Electronic_document_management.Models;

namespace Electronic_document_management.Services.RepositoryService.Interfaces
{
    public interface IQueryRepository
    {
        public IEnumerable<Query> GetQueries();
        public IEnumerable<Query> GetQueries(int depId);
        public Query? GetQuery(int queryId);
        public void AddQuery(Query query);
        public bool RemoveQuery(int queryId);
    }
}
