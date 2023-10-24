using Electronic_document_management.Models;

namespace Electronic_document_management.Services.RepositoryService.Interfaces
{
    public interface IQueryRepository
    {
        public Task<IEnumerable<Query>> GetQueriesAsync();
        public Task<IEnumerable<Query>> GetQueriesByDepartmentAsync(int depId);
        public Task AddQueryAsync(Query query);
        public Errors ConfirmUser(int queryId, string role);
        public Errors CancelUser(int queryId);
    }
}
