using Electronic_document_management.Services.Tokens.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Electronic_document_management.Filters.Auhorization
{
    public class JwtAuthFilter : Attribute, IAuthorizationFilter
    {
        private readonly string? _claimType;
        private readonly string? _claimValue;
        public JwtAuthFilter(string claimType, string claimValue)
        {
            _claimType = claimType;
            _claimValue = claimValue;
        }
        public JwtAuthFilter()
        {
            _claimType = null;
            _claimValue = null;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            string? accessToken;
            var _tokenService = (ITokenService)filterContext.HttpContext.RequestServices.GetService(typeof(ITokenService))!;
            if (!filterContext.HttpContext.Request.Cookies.TryGetValue("accessToken", out accessToken))
            {
                filterContext.HttpContext.Response.Redirect("/login");
            }
            else
            {
                var res = _tokenService.ValidateToken(accessToken);
                if (!res.Item1)
                {
                    filterContext.HttpContext.Response.Redirect("/login");
                }
                else
                {
                    
                    if (_claimType != null && _claimValue != null)
                    {
                        var hasClaim = res.Item2?.Any(c => c.Type == _claimType && c.Value == _claimValue);
                        if (!(bool)hasClaim!)
                        {
                            filterContext.Result = new ForbidResult();
                        }
                    }
                    
                    filterContext.HttpContext.User.AddIdentity(new ClaimsIdentity(res.Item2));
                }
            }
        }
    }
}
