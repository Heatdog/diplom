using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Electronic_document_management.Services.RepositoryService.Interfaces;
using Electronic_document_management.Services.QueryService;

namespace Electronic_document_management.Controllers.Queries
{
    [Controller, Route("/query"), Authorize(Roles = "Admin, HeadOfDepartment")]
    public class QueryController : Controller
    {
        private readonly IQueryService _queryService;
        public QueryController(IQueryService queryService)
        {
            _queryService = queryService;
        }
        [HttpGet]
        public IActionResult GetAllQuery()
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == null)
                return new ForbidResult();
            var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
                return new ForbidResult();
            var res = _queryService.GetAllQueries(role, Convert.ToInt32(id));
            if (res == null)
                return new ForbidResult();
            return View(res);
        }
        [HttpPost]
        public IActionResult UpdateQuery()
        {
            var role = HttpContext.Request.Form["role"];
            if (role == "HeadOfDepartment" && HttpContext.User.FindFirst(ClaimTypes.Role)?.Value != "Admin")
            {
                return Redirect("/query");
            }
            var resp = HttpContext.Request.Form.Keys
                .FirstOrDefault(el => el.StartsWith("save") || el.StartsWith("cancel"))?
                .Split(":");
            if (resp?[0] == "save")
            {
                _queryService.ConfirmUser(Convert.ToInt32(resp?[1]), role);
            }else if (resp?[0] == "cancel")
            {
                _queryService.CancelUser(Convert.ToInt32(resp?[1])); 
            }
            return Redirect("/query");
        }
    }
}
