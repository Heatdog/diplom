using Electronic_document_management.Filters.Auhorization;
using Electronic_document_management.Models;
using Electronic_document_management.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_document_management.Controllers.Group
{
    [Controller, Route("groups"), JwtAuthFilter]
    public class GroupController : Controller
    {
        IRepository _repo;
        public GroupController(IRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> AllGroups()
        {
            return View(await _repo.GetDepartmentsAsync());
        }
        [HttpGet, Route("{id:int}")]
        public string GetGroup(int id)
        {
            return "get groupe: " + $"{id}";
        }
        [HttpGet, Route("invite")]
        public string InviteUser()
        {
            return "invte user in groupe";
        }
        [HttpGet, Route("create"), JwtAuthFilter("role", "Admin")]
        public IActionResult CreateGroup()
        {
            return View();
        }
        [HttpPost, Route("create"), JwtAuthFilter("role", "Admin")]
        public async Task<IActionResult> CreateGroup(string name)
        {
            var res = await _repo.AddDepartmentAsync(new Department(name));
            if (res != Errors.None)
                return View("CreateGroup", new ErrorMessage(res));
            else
                return Redirect("/groups");
        }
    }
}
