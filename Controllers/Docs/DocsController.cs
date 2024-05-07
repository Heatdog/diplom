using Electronic_document_management.Models;
using Electronic_document_management.Services.FileService;
using Electronic_document_management.Services.RepositoryService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Electronic_document_management.ViewModels.Docs;

namespace Electronic_document_management.Controllers.Docs
{
    [Controller, Route("docs"), Authorize]
    public class DocsController : Controller
    {
        private readonly IDocumentRepository _docsRepo;
        private readonly IUserRepository _userRepo;
        private readonly IFileRepository _fileRepo;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;
        public DocsController(IDocumentRepository docsRepo, IUserRepository userRepository, IFileRepository fileRepo,
            IFileService fileService, IWebHostEnvironment env)
        {
            _docsRepo = docsRepo;
            _userRepo = userRepository;
            _fileRepo = fileRepo;
            _fileService = fileService;
            _env = env;
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

            var user = _userRepo.GetUser(Convert.ToInt32(userId));
            if (user == null)
                return new ForbidResult();

            IEnumerable<Document> res;
            if (role == "Admin")
            {
                res = _docsRepo.GetDocuments();
            }
            else
            {
                res = _docsRepo.GetDocuments(user.DepartmentId);
            }
            return View(res);
        }

        [HttpPost]
        public IActionResult SearchDoc(string searchText)
        {
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First().Value;
            if (userId == null)
                return new ForbidResult();
            var role = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Role).First().Value;
            if (role == null)
                return new ForbidResult();

            var user = _userRepo.GetUser(Convert.ToInt32(userId));
            if (user == null)
                return new ForbidResult();

            IEnumerable<Document> res;
            if (role == "Admin")
            {
                res = _docsRepo.SearchDocumnets(searchText);
            }
            else
            {
                res = _docsRepo.SearchDocumnets(searchText, user.DepartmentId);
            }
            return View("AllDocs", res);
        }

        [HttpGet, Route("create")]
        public IActionResult CreateDoc()
        {
            return View();
        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> CreateDoc(string name, string description, IFormFile uploadedFile)
        {
            if (uploadedFile == null)
                return View("CreateDoc", new DocsMessage("Нет загружаемого файла"));
            if (uploadedFile.ContentType != "application/pdf")
                return View("CreateDoc", new DocsMessage("Поддерживаются только форматы .pdf"));

            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First().Value;
            if (userId == null)
                return new ForbidResult();

            var author = _userRepo.GetUser(Convert.ToInt32(userId));
            if (author == null)
                return new ForbidResult();

            var dep = author.Department;

            var fileGuid = await _fileService.UploadFile(uploadedFile);
            if (fileGuid == null){
                 return View("CreateDoc", new DocsMessage("Ошибка загрузки файла на файловый сервер"));
            }

            var document = new Document(name, author, description);
            var id = _docsRepo.InsertDocument(document);
            if (id == null)
            {
                return View("CreateDoc", new DocsMessage("Ошибка создания документа на сервере"));
            }

            var docFile = new DocumentFile((Guid)fileGuid, document);
            var err = _fileRepo.InsertFile(docFile);
            if (err != Errors.None)
            {
                return View("CreateDoc", new DocsMessage("Ошибка добавления документа на сервер"));
            }

            return Redirect("/docs");
        }

        [HttpGet, Route("{id:int}")]
        public IActionResult GetDocument(int id)
        {
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First().Value;
            if (userId == null)
                return new ForbidResult();
            var user = _userRepo.GetUser(Convert.ToInt32(userId));
            if (user == null)
                return new ForbidResult();

            var doc = _docsRepo.GetDocument(id);
            if (doc == null)
                return View("AllDocs");

            var role = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Role).First().Value;
            if (role == null)
                return new ForbidResult();


            if (!(role == "Admin" || doc.Author.DepartmentId == user.DepartmentId))
            {
                return new ForbidResult();
            }

            return View(new DocumentAndUser(doc, user));
        }

        [HttpPost, Route("{id:int}"), Authorize(Roles = "Admin, HeadOfDepartment")]
        public IActionResult UpdateStatus(int id, string status)
        {
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First().Value;
            if (userId == null)
                return new ForbidResult();
            var user = _userRepo.GetUser(Convert.ToInt32(userId));
            if (user == null)
                return new ForbidResult();

            var doc = _docsRepo.GetDocument(id);
            if (doc == null)
                return View("AllDocs");

            var role = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Role).First().Value;
            if (role == null)
                return new ForbidResult();

            if (role == "HeadOfDepartment" && doc.Author.DepartmentId != user.DepartmentId)
                return new ForbidResult();

            switch (status)
            {
                case "В разработке":
                    doc.Status = Status.InDeveloping;
                    break;
                case "На рассмотрении":
                    doc.Status = Status.OnConfirmation;
                    break;
                case "Готов":
                    doc.Status = Status.Ready;
                    break;
            }

            _docsRepo.UpdateDocumnet(doc);
            return View("GetDocument", new DocumentAndUser(doc, user));
        }

        [HttpGet, Route("{id:int}/{fileId:int}")]
        public async Task<IActionResult> OpenFile(int id, int fileId) 
        {
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First().Value;
            if (userId == null)
                return new ForbidResult();

            var user = _userRepo.GetUser(Convert.ToInt32(userId));
            if (user == null)
                return new ForbidResult();

            var file = _fileRepo.GetFile(fileId);
            if (file == null)
                return View($"/docs/{id}");

            if (!(user.Role == Role.Admin || user.DepartmentId == file.Doc.Author.DepartmentId))
            {
                return new ForbidResult();
            }

            var resFile = await _fileService.GetFile(file.FileID);
            if (resFile == null)
            {
                return new ForbidResult();
            }
            return File(resFile, "application/pdf", file.Version.ToString());
        }

        [HttpGet, Route("{id:int}/add")]
        public IActionResult AddNewVersion(int id)
        {
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First().Value;
            if (userId == null)
                return new ForbidResult();

            var doc = _docsRepo.GetDocument(id);
            if (doc == null)
                return View("AllDocs");

            if (doc.AuthorId.ToString() != userId)
            {
                return new ForbidResult();
            }

            return View();
        }

        [HttpPost, Route("{id:int}/add")]
        public async Task<IActionResult> AddNewVersion(int id, IFormFile uploadedFile)
        {
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First().Value;
            if (userId == null)
                return new ForbidResult();

            var user = _userRepo.GetUser(Convert.ToInt32(userId));
            if (user == null)
                return new ForbidResult();

            var doc = _docsRepo.GetDocument(id);
            if (doc == null)
                return View("AllDocs");

            if (doc.AuthorId.ToString() != userId)
            {
                return new ForbidResult();
            }

            doc.Version++;

            var uploadRes = await _fileService.UploadFile(uploadedFile);
            if (uploadedFile == null){
                return View("CreateDoc", new DocsMessage("Ошибка загрузки документа на сервер"));
            }
            
            var docFile = new DocumentFile((Guid)uploadRes, doc);
            var err = _fileRepo.InsertFile(docFile);
            if (err != Errors.None)
            {
                return View("AllDocs");
            }
            return Redirect($"/docs/{id}");
        }
    }
}
