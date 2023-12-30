using Electronic_document_management.Models;
using Electronic_document_management.Services.RepositoryService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_document_management.Controllers.Group
{
    [Controller, Route("groups"), Authorize]
    public class GroupController : Controller
    {
        IDepartmentRepository _repo;
        public GroupController(IDepartmentRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public IActionResult AllGroups()
        {
            return View(_repo.GetDepartments());
        }
        [HttpGet, Route("{id:int}")]
        public IActionResult GetGroup(int id)
        {
            return View(_repo.GetDepartment(id));
        }
        [HttpGet, Route("create"), Authorize(Roles = "Admin")]
        public IActionResult CreateGroup()
        {
            return View();
        }
        [HttpPost, Route("create"), Authorize(Roles = "Admin")]
        public IActionResult CreateGroup(string name)
        {
            var res = _repo.SetDepartment(new Department(name));
            if (res != Errors.None)
                return View("CreateGroup", new ErrorMessage(res));
            else
                return Redirect("/groups");
        }
    }
}
