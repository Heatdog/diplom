using Electronic_document_management.Models;

namespace Electronic_document_management.Services.QueryService
{
    public interface IQueryService
    {
        public Errors ConfirmUser(int queryId, string role);
        public Errors CancelUser(int queryId);
        public IEnumerable<Query>? GetAllQueries(string role, int id);
    }
}
