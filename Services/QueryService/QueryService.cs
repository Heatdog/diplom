using Electronic_document_management.Models;
using Electronic_document_management.Services.RepositoryService.Interfaces;
using Electronic_document_management.Services.RepositoryService.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Electronic_document_management.Services.QueryService
{
    public class QueryService : IQueryService
    {
        private readonly IQueryRepository _queryRepo;
        private readonly IUserRepository _userRepo;
        public QueryService(IQueryRepository queryRepo, IUserRepository userRepo) 
        {
            _queryRepo = queryRepo;
            _userRepo = userRepo;
        }
        public Errors ConfirmUser(int queryId, string role)
        {
            var query = _queryRepo.GetQuery(queryId);
            if (query == null)
                return Errors.InvalidArguments;
            var user = _userRepo.GetUser(query.UserId);
            if (user == null)
                return Errors.InvalidUser;
            user.Role = RoleTransform.RoleToEnum(role);
            user.IsConfirmed = true;
            var err = _userRepo.UpdateUser(user);
            if (err != Errors.None)
                return err;
            _queryRepo.RemoveQuery(queryId);
            return Errors.None;

        }
        public Errors CancelUser(int queryId)
        {
            var query = _queryRepo.GetQuery(queryId);
            if (query == null ) 
                return Errors.InvalidArguments;
            var err = _userRepo.RemoveUser(query.UserId);
            if (err != Errors.None)
                return err;
            _queryRepo.RemoveQuery(queryId);
            return Errors.None;
        }
        public IEnumerable<Query>? GetAllQueries(string role, int id)
        {
            if (role == "Admin")
            {
                return _queryRepo.GetQueries();
            }
            var user = _userRepo.GetUser(id);
            if (user == null)
                return null;
            return _queryRepo.GetQueries(user.DepartmentId);
        }
    }
}
