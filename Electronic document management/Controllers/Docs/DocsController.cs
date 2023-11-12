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
        private readonly IFileRepository _fileRepo;
        public DocsController(IDocumentRepository docsRepo, IUserRepository userRepository, IFileRepository fileRepo)
        {
            _docsRepo = docsRepo;
            _userRepo = userRepository;
            _fileRepo = fileRepo;
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
        public IActionResult CreateDoc(string name, string description, IFormFile uploadedFile)
        {
            if (uploadedFile == null)
                return View("CreateDoc");

            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First().Value;
            if (userId == null)
                return new ForbidResult();

            var author = _userRepo.GetUser(Convert.ToInt32(userId));
            if (author == null)
                return new ForbidResult();

            var dep = author.Department;

            var document = new Document(name, author, description);
            var id = _docsRepo.InsertDocument(document);
            if (id == null)
            {
                return View("CreateDoc");
            }

            string path = $"/Files/{dep.Name}/{id}/{uploadedFile.Name}";
            var docFile = new DocumentFile(path, document);
            var err = _fileRepo.InsertFile(docFile);
            if (err != Errors.None)
            {
                return View("CreateDoc");
            }

            return Redirect("/docs");
        }
    }
}
