using Electronic_document_management.Services.AuthService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
            await HttpContext.SignInAsync(res.ClaimsPrincipal!);
            return Redirect("/");
        }
        [HttpGet, Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/login");
        }
    }
}
