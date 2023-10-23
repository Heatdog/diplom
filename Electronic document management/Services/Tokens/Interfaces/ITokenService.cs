using Electronic_document_management.Models;
using System.Security.Claims;

namespace Electronic_document_management.Services.Tokens.Interfaces
{
    public interface ITokenService
    {
        public string BuildAccessToken(User user);
        public (bool, IEnumerable<Claim>?) ValidateToken(string token);
    }
}
