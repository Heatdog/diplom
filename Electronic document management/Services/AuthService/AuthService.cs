using Electronic_document_management.Models;
using Electronic_document_management.Services.Claims;
using Electronic_document_management.Services.PasswordHasher;
using Electronic_document_management.Services.RepositoryService.Interfaces;
using System.Text.RegularExpressions;

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
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                return new AuthResult(Errors.EmptyValue);

            var user = _repoUser.GetUserByUsername(userName);
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
            if (string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(surname) ||
                string.IsNullOrWhiteSpace(department) ||
                string.IsNullOrWhiteSpace(password))
                return Errors.EmptyValue;
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(email))
                return Errors.IncorrectEmail;

            var dp = _repoDepartment.GetDepartment(department);
            if (dp == null) 
                return Errors.InvalidDepartment;

            if (_repoUser.GetUserByUsername(userName) != null) 
                return Errors.InvalidUser;
            if (_repoUser.GetUserByEmail(email) != null) 
                return Errors.InvalidEmailAddress;

            var user = new User(userName, name, surname, email, _passwordHasher.HashPassword(password), false, Role.Worker, dp);
            _queryRepository.AddQuery(new Query(user, dp));
            return Errors.None;
        }

        public void SignOut(string cookie, Action<string> signOutFunc)
        {
            signOutFunc(cookie);
        }
    }
}
