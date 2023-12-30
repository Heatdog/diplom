using System.Security.Claims;

namespace Electronic_document_management.Models
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public ErrorMessage? Message { get; set; }
        public Errors Errors { get; set; }
        public User? User { get; set; }
        public ClaimsPrincipal? ClaimsPrincipal { get; set; }
        public AuthResult() { Success = false; }
        public AuthResult(Errors error) : this()
        {
            Errors = error;
            Message = new ErrorMessage(error);
        }
        public AuthResult(User user, ClaimsPrincipal claimsPrincipal)
        {
            Success = true;
            User = user;
            ClaimsPrincipal = claimsPrincipal;
        }
    }
}
