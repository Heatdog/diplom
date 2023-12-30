using Electronic_document_management.Models;
using Electronic_document_management.Services.PasswordHasher;
using Electronic_document_management.Services.RepositoryService.Interfaces;
using Electronic_document_management.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Electronic_document_management.Controllers.Profile
{
    [Controller, Route("profile"), Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserRepository _repo;
        private readonly IPasswordHasher _passwordHasher;
        public ProfileController(IUserRepository repo, IPasswordHasher passwordHasher)
        {
            _repo = repo;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return View(_repo.GetUsers().Where(user => user.IsConfirmed == true));
        }
        [HttpGet, Route("{id:int}")]
        public IActionResult GetProfile(int id)
        {
            return View(_repo.GetUser(id));
        }

        [HttpGet, Route("{id:int}/password")]
        public IActionResult GetPassword(int id)
        {
            if (HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First().Value != id.ToString())
            {
                return new ForbidResult();
            }

            var user = _repo.GetUser(id);
            if (user == null) return new ForbidResult();

            return View(new ProfileModel(user, null));
        }

        [HttpPost, Route("{id:int}/password")]
        public IActionResult ChangePassword(int id, string oldPassword, string newPassword, string newPasswordRepeat)
        {
            if (HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First().Value != id.ToString())
            {
                return new ForbidResult();
            }

            var user = _repo.GetUser(id);
            if (user == null)
                return new ForbidResult();

            if (string.IsNullOrWhiteSpace(oldPassword) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(newPasswordRepeat))
                return View("GetPassword", new ProfileModel(user, "Пустые значения полей"));

            if (newPassword != newPasswordRepeat)
            {
                return View("GetPassword", new ProfileModel(user, "Старые пароли не сопадают"));
            }

            if (!_passwordHasher.VerifyPassword(oldPassword, user.Password))
            {
                return View("GetPassword", new ProfileModel(user, "Пароль не верен"));
            }

            user.Password = _passwordHasher.HashPassword(newPassword);
            _repo.UpdateUser(user);
            return View("GetPassword", new ProfileModel(user, "Пароль изменён"));
        }

        [HttpGet, Route("{id:int}/email")]
        public IActionResult GetEmail(int id)
        {
            if (HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First().Value != id.ToString())
            {
                return new ForbidResult();
            }
            var user = _repo.GetUser(id);
            if (user == null) return new ForbidResult();

            return View(new ProfileModel(user, null));
        }

        [HttpPost, Route("{id:int}/email")]
        public IActionResult ChangeEmail(int id, string newEmail)
        {
            if (HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First().Value != id.ToString())
            {
                return new ForbidResult();
            }
            var user = _repo.GetUser(id);
            if (user == null) return new ForbidResult();

            if (string.IsNullOrWhiteSpace(newEmail))
                return View("GetEmail", new ProfileModel(user, "Пустые значения полей"));

            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(newEmail))
                return View("GetEmail", new ProfileModel(user, "Некорректная почта"));

            var checkEmail = _repo.GetUserByEmail(newEmail);
            if (checkEmail != null)
                return View("GetEmail", new ProfileModel(user, "Почта уже зарегестрирована"));

            user.Email = newEmail;
            _repo.UpdateUser(user);
            return View("GetEmail", new ProfileModel(user, "Почта успешно изменена"));
        }
    }
}
