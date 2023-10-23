using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Electronic_document_management.Filters.Auhorization;

namespace Electronic_document_management.Controllers.Home
{
    [Controller, Route("/"), JwtAuthFilter]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Main()
        {
            return View();
        }
    }
}
