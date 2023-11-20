namespace Electronic_document_management.Services.FileService
{
    public class FileService : IFileService
    {
        public async Task<bool> UploadFile(string path, IFormFile file)
        {
            var directoryPath = Path.GetDirectoryName(path);
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return true;
            }catch (Exception)
            {
                return false;
            }
        }
    }
}
