using Electronic_document_management.Models;
using Electronic_document_management.Services.Databases;
using Electronic_document_management.Services.RepositoryService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Electronic_document_management.Services.RepositoryService.Repository
{
    public class QueryRepository : IQueryRepository
    {
        private ApplicationContext db;
        private readonly IUserRepository _userRepository;
        public QueryRepository(ApplicationContext db, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            this.db = db;
        }
        public async Task<IEnumerable<Query>> GetQueriesAsync()
        {
            return await db.Queries
                .Include(query => query.User)
                .Include(query => query.Department)
                .ToListAsync();
        }
        public async Task<IEnumerable<Query>> GetQueriesByDepartmentAsync(int depId)
        {
            return await db.Queries
                .Where(query => query.DepartmentId == depId)
                .Include(query => query.User)
                .Include (query => query.Department)
                .ToListAsync();
        }
        public async Task AddQueryAsync(Query query)
        {
            db.Queries.Add(query);
            await db.SaveChangesAsync();
        }

        public Errors ConfirmUser(int queryId, string role)
        {
            var query = db.Queries.Where(q => q.Id == queryId).FirstOrDefault();
            if (query == null)
            {
                return Errors.InvalidArguments;
            }
            var err = _userRepository.ConfirmUser(query.UserId, role);
            if (err != Errors.None)
            {
                return err;
            }
            db.Queries.Remove(query);
            db.SaveChanges();
            return Errors.None;
        }

        public Errors CancelUser(int queryId)
        {
            var query = db.Queries.Where(q => q.Id == queryId).FirstOrDefault();
            if (query == null)
            {
                return Errors.InvalidArguments;
            }
            var err = _userRepository.RemoveUser(query.UserId);
            if (err != Errors.None)
            {
                return err;
            }
            return Errors.None;
        }
    }
}
