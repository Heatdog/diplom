using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_document_management.Controllers.Docs
{
    [Controller, Route("docs"), Authorize]
    public class DocsController : Controller
    {
        [HttpGet]
        public string AllDocs()
        {
            return "all docs";
        }
        [HttpGet, Route("{id:int}")]
        public string GetDoc(int id)
        {
            return "doc with id: " + id.ToString();
        }
    }
}
