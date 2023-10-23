using Electronic_document_management.Models;
using Electronic_document_management.Services.Tokens.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Electronic_document_management.Services.Tokens.Jwt
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly string? issure;
        private readonly string? audience;
        private readonly string? key;
        private readonly TimeSpan tokenLifetime;

        public JwtTokenService(IConfiguration configuration)
        {
            _config = configuration;
            key = _config["JwtSettings:Key"];
            issure = _config["JwtSettings:Issuer"];
            audience = _config["JwtSettings:Audience"];
            tokenLifetime = TimeSpan.FromMinutes(30);
        }
        private SymmetricSecurityKey GetSymmetricSecurityKey() =>
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));

        private List<Claim> CreateClaims(User user)
        {
            return new List<Claim> {
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.UserName),
                new Claim("role", user.Role.ToString())
            };
        }

        private bool ValidClaims(IEnumerable<Claim> claims)
        {
            var id = claims.Where(cl => cl.Type == "id").FirstOrDefault();
            var username = claims.Where(cl => cl.Type == "username").FirstOrDefault();
            var role = claims.Where(cl => cl.Type == "role").FirstOrDefault();
            if (id == null || username == null || role == null) return false;
            return true;
        }

        public string BuildAccessToken(User user)
        {
            var jwt = new JwtSecurityToken(
                issuer: issure,
                audience: audience,
                claims: CreateClaims(user),
                expires: DateTime.UtcNow.Add(tokenLifetime),
                signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public (bool, IEnumerable<Claim>?) ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = issure,
                    ValidAudience = audience,
                    IssuerSigningKey = GetSymmetricSecurityKey()
                }, out var validatedToken);
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                        return (false, null);
                    else
                    {
                        if (!ValidClaims(jwtSecurityToken.Claims))
                            return (false, null);
                        return (true, jwtSecurityToken.Claims);
                    }
                }else
                    return (false, null);
            }
            catch
            {
                return (false, null);
            }
        }
    }
}
