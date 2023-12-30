namespace Electronic_document_management.Services.FileService
{
    public interface IFileService
    {
        Task<bool> UploadFile(string path, IFormFile file);
    }
}
