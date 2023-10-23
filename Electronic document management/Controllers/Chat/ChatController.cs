using Electronic_document_management.Filters.Auhorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_document_management.Controllers.Chat
{
    [Controller, Route("/chat"), JwtAuthFilter]
    public class ChatController : Controller
    {
        [HttpGet]
        public string GetAll()
        {
            return "all chats";
        }
        [HttpGet, Route("write")]
        public string Write()
        {
            return "form to write";
        }
        [HttpGet, Route("{id:int}")]
        public string ChatUser(int id)
        {
            return "chat with user: " + $"{id}";
        }
    }
}
