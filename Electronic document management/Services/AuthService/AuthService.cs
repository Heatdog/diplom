using Electronic_document_management.Models;
using Electronic_document_management.Services.Claims;
using Electronic_document_management.Services.PasswordHasher;
using Electronic_document_management.Services.RepositoryService.Interfaces;

namespace Electronic_document_management.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repoUser;
        private readonly IDepartmentRepository _repoDepartment;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IClaimService _claimService;
        private readonly IQueryRepository _queryRepository;
        public AuthService(IUserRepository userRepo, IDepartmentRepository repoDepartment, 
            IPasswordHasher passwordHasher, IClaimService claimService, IQueryRepository queryRepository)
        {
            _repoUser = userRepo;
            _repoDepartment = repoDepartment;
            _passwordHasher = passwordHasher;
            _claimService = claimService;
            _queryRepository = queryRepository;
        }

        public async Task<AuthResult> SignIn(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return new AuthResult(Errors.InvalidArguments);
            if (string.IsNullOrWhiteSpace(password))
                return new AuthResult(Errors.InvalidArguments);

            var user = await _repoUser.GetUserAsync(userName);
            if (user == null)
                return new AuthResult(Errors.InvalidArguments);
            if (!user.IsConfirmed)
                return new AuthResult(Errors.NotVerified);
            if (!_passwordHasher.VerifyPassword(password, user.Password))
                return new AuthResult(Errors.InvalidArguments);
            return new AuthResult(user, _claimService.BuildClaims(user));
        }

        public async Task<Errors> SignUp(string userName, string email, string name,
            string surname, string department, string password)
        {
            var dp = await _repoDepartment.GetDepartmentAsync(department);
            if (dp == null) return Errors.InvalidDepartment;
            var user = new User(userName, name, surname, email, _passwordHasher.HashPassword(password), false, Role.Worker, dp);
            await _queryRepository.AddQueryAsync(new Query(user, dp));
            return Errors.None;
        }

        public void SignOut(string cookie, Action<string> signOutFunc)
        {
            signOutFunc(cookie);
        }
    }
}
