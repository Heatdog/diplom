using Electronic_document_management.Models;
using System.Security.Claims;

namespace Electronic_document_management.Services.Claims
{
    public interface IClaimService
    {
        ClaimsPrincipal BuildClaims(User user);
    }
}
