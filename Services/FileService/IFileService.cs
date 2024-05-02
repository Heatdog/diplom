using Electronic_document_management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_document_management.Services.FileService
{
    public interface IFileService
    {
        Task<Guid?> UploadFile(IFormFile file);
        Task<IActionResult?> GetFile(Guid id);
    }
}
