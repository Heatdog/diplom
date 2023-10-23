using Electronic_document_management.Filters.Auhorization;
using Electronic_document_management.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return View(await _repo.GetUsersAsync());
        }
        [HttpGet, Route("{id:int}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            var user = await _repo.GetUserAsync(id);
            return View(user);
        }

        [HttpGet, Route("{id:int}/settings")]
        public string GetSttings(int id)
        {
            return "settings of profile: " + $"{id}";
        }
    }
}
