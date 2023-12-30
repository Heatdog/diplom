using Electronic_document_management.Models;

namespace Electronic_document_management.Services.AuthService
{
    public interface IAuthService
    {
        public Task<AuthResult> SignIn(string userName, string password);
        public Task<Errors> SignUp(string userName, string email, string name,
            string surname, string department, string password);
        public void SignOut(string cookie, Action<string> signOutFunc);
    }
}
