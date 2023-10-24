using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Electronic_document_management.Services.RepositoryService.Interfaces;

namespace Electronic_document_management.Controllers.Queries
{
    [Controller, Route("/query"), Authorize(Roles = "Admin, HeadOfDepartment")]
    public class QueryController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IQueryRepository _queryRepository;
        public QueryController(IUserRepository userRepository, IQueryRepository queryRepository)
        {
            _userRepository = userRepository;
            _queryRepository = queryRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllQuery()
        {
            if (HttpContext.User.FindFirst(ClaimTypes.Role)?.Value == "Admin")
            {
                return View(await _queryRepository.GetQueriesAsync());
            }
            var id = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userRepository.GetUserAsync(id);
            int depId = (int)user!.DepartmentId!;
            return View(await _queryRepository.GetQueriesByDepartmentAsync(depId));
        }
        [HttpPost]
        public IActionResult UpdateQuery()
        {
            var role = HttpContext.Request.Form["role"];
            if (role == "Admin" && HttpContext.User.FindFirst(ClaimTypes.Role)?.Value != "Admin")
            {
                return Redirect("/query");
            }
            var resp = HttpContext.Request.Form.Keys
                .FirstOrDefault(el => el.StartsWith("save") || el.StartsWith("cancel"))?
                .Split(":");
            if (resp?[0] == "save")
            {
                _queryRepository.ConfirmUser(Convert.ToInt32(resp?[1]), role);
            }else if (resp?[0] == "cancel")
            {
                _queryRepository.CancelUser(Convert.ToInt32(resp?[1])); 
            }
            return Redirect("/query");
        }
    }
}
