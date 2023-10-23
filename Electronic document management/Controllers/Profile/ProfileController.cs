using Electronic_document_management.Filters.Auhorization;
using Electronic_document_management.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Electronic_document_management.Controllers.Profile
{
    [Controller, Route("profile"), JwtAuthFilter]
    public class ProfileController : Controller
    {
        private IRepository _repo;
        public ProfileController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return View(await _repo.GetUsersWithDepAsync());
        }
        [HttpGet, Route("{id:int}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            var user = await _repo.GetUserAsync(id);
            return View(user);
        }

        [HttpGet, Route("{id:int}/settings")]
        public async Task<IActionResult> GetSttings(int id)
        {
            if (HttpContext.User.Claims.Any(claim => claim.Type == "id" && claim.Value != id.ToString()))
            {
                return new ForbidResult();
            }
            return View(await _repo.GetUserAsync(id));
        }
    }
}
