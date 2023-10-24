using Microsoft.AspNetCore.Mvc;
using Electronic_document_management.ViewModels.Register;
using Electronic_document_management.Services.AuthService;
using Electronic_document_management.Services.RepositoryService.Interfaces;

namespace Electronic_document_management.Controllers.Register
{
    [Controller, Route("register")]
    public class RegisterController : Controller
    {
        private IDepartmentRepository _repo;
        private IAuthService _authService;
        public RegisterController(IDepartmentRepository repo, IAuthService authService)
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
            var error = await _authService.SignUp(userName, email, name, surname, department, password);
            if (error != Models.Errors.None)
            {
                var list = await _repo.GetDepartmentsAsync();
                return View(new RegisterModel(new Models.AuthResult(error), list));
            }
            return View("Wait", email);
        }
    }
}
