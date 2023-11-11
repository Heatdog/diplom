using Electronic_document_management.Models;
using Electronic_document_management.Services.RepositoryService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Electronic_document_management.Controllers.Docs
{
    [Controller, Route("docs"), Authorize]
    public class DocsController : Controller
    {
        private readonly IDocumentRepository _docsRepo;
        private readonly IUserRepository _userRepo;
        public DocsController(IDocumentRepository docsRepo, IUserRepository userRepository)
        {
            _docsRepo = docsRepo;
            _userRepo = userRepository;
        }
        [HttpGet]
        public IActionResult AllDocs()
        {
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First().Value;
            if (userId == null)
                return new ForbidResult();
            var role = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Role).First().Value;
            if (role == null)
                return new ForbidResult();

            IEnumerable<Document> res;
            if (role == "Admin")
            {
                res = _docsRepo.GetDocuments();
            }
            else
            {
                res = _docsRepo.GetDocuments(Convert.ToInt32(userId));
            }
            return View(res);
        }
        [HttpGet, Route("create")]
        public IActionResult CreateDoc()
        {
            return View();
        }
        [HttpPost, Route("create")]
        public IActionResult CreateDoc(string name, string description)
        {
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First().Value;
            if (userId == null)
                return new ForbidResult();
            var user = _userRepo.GetUser(Convert.ToInt32(userId));
            if (user == null)
                return new ForbidResult();
            //var doc = new Document(name, file.Name, user, description);
            return View();
        }
    }
}
