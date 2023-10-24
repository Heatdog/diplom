using Electronic_document_management.Models;
using System.Security.Claims;

namespace Electronic_document_management.Services.Claims
{
    public class ClaimService : IClaimService
    {
        public ClaimsPrincipal BuildClaims(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var claimIdentity = new ClaimsIdentity(claims, "Cookies");
            return new ClaimsPrincipal(claimIdentity);
        }
    }
}
