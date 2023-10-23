using Electronic_document_management.Filters.Auhorization;
using Electronic_document_management.Services.Auth.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_document_management.Controllers.Login
{
    [Controller, Route("{action}")]
    public class LoginController : Controller
    {
        private IAuthService _authService;
        public LoginController(IAuthService authClaims)
        {
            _authService = authClaims;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var res = await _authService.SignIn(userName, password);
            if (!res.Success)
            {
                return View("Login", res);
            }
            Response.Cookies.Append("accessToken", res.AccessToken!);
            return Redirect("/");
        }
        [HttpGet, JwtAuthFilter]
        public IActionResult Logout()
        {
            _authService.SignOut("accessToken", Response.Cookies.Delete);
            return Redirect("/login");
        }
    }
}
