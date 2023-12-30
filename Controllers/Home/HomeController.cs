using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Electronic_document_management.Controllers.Home
{
    [Controller, Route("/")]
    public class HomeController : Controller
    {
        [HttpGet, Authorize]
        public IActionResult Main()
        {
            return View();
        }
        [HttpGet, Route("/accessdenied")]
        public IActionResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View();
        }
    }
}
