using Electronic_document_management.Models;
using Electronic_document_management.Services.Claims;
using Electronic_document_management.Services.PasswordHasher;
using Electronic_document_management.Services.Repository;

namespace Electronic_document_management.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IRepository _repo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IClaimService _claimService;
        public AuthService(IRepository repository, IPasswordHasher passwordHasher, IClaimService claimService)
        {
            _repo = repository;
            _passwordHasher = passwordHasher;
            _claimService = claimService;
        }

        public async Task<AuthResult> SignIn(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return new AuthResult(Errors.InvalidArguments);
            if (string.IsNullOrWhiteSpace(password))
                return new AuthResult(Errors.InvalidArguments);

            var user = await _repo.GetUserAsync(userName);
            if (user == null)
                return new AuthResult(Errors.InvalidArguments);
            if (!_passwordHasher.VerifyPassword(password, user.Password))
                return new AuthResult(Errors.InvalidArguments);
            return new AuthResult(user, _claimService.BuildClaims(user));
        }

        public async Task<AuthResult> SignUp(string userName, string email, string name,
            string surname, string department, string password)
        {
            var dp = await _repo.GetDepartmentAsync(department);
            if (dp == null) return new AuthResult(Errors.InvalidDepartment);
            var user = new User(userName, name, surname, email, _passwordHasher.HashPassword(password), Role.Worker, dp);
            var res = await _repo.AddUserAsync(user);
            if (res != Errors.None)
                return new AuthResult(res);
            return new AuthResult(user, _claimService.BuildClaims(user));
        }

        public void SignOut(string cookie, Action<string> signOutFunc)
        {
            signOutFunc(cookie);
        }
    }
}
