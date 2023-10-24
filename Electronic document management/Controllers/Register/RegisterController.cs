using Microsoft.AspNetCore.Mvc;
using Electronic_document_management.ViewModels.Register;
using Electronic_document_management.Services.AuthService;
using Electronic_document_management.Services.Repository;
using Microsoft.AspNetCore.Authentication;

namespace Electronic_document_management.Controllers.Register
{
    [Controller, Route("register")]
    public class RegisterController : Controller
    {
        private IRepository _repo;
        private IAuthService _authService;
        public RegisterController(IRepository repo, IAuthService authService)
        {
            _repo = repo;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var list = await _repo.GetDepartmentsAsync();
            return View(new RegisterModel(null, list));
        }

        [HttpPost]
        public async Task<IActionResult> Register(string userName, string email, string name,
            string surname, string department, string password)
        {
            var res = await _authService.SignUp(userName, email, name, surname, department, password);
            if (!res.Success) 
            {
                var list = await _repo.GetDepartmentsAsync();
                return View(new RegisterModel(res, list));
            }
            await HttpContext.SignInAsync(res.ClaimsPrincipal!);
            return Redirect("/");
        }
    }
}
