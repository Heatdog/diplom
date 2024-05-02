using Electronic_document_management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_document_management.Services.FileService
{
    public class FileService : IFileService
    {
        IConfiguration configs;
        private static readonly HttpClient _authClient = new HttpClient(); 
        public FileService(IConfiguration configuration){
            configs = configuration;
        }
        public async Task<Guid?> UploadFile(IFormFile file)
        {
           string? connection = configs.GetConnectionString("FileServer");
            if (connection == null)
            {
                Console.WriteLine("Connection did not specified");
                return null;
            }

            var respone = await _authClient.PostAsJsonAsync(connection+"/insert", file);

            if (!respone.IsSuccessStatusCode){
                return null;
            }
            var id = await respone.Content.ReadAsStringAsync();
            return Guid.Parse(id);
        }

        public async Task<IActionResult?> GetFile(Guid id){
            string? connection = configs.GetConnectionString("FileServer");
            if (connection == null)
            {
                Console.WriteLine("Connection did not specified");
                return null;
            }

            var respone = await _authClient.GetAsync(connection+"/get/"+id);
            if (!respone.IsSuccessStatusCode){
                return null;
            }

            var bytes = await respone.Content.ReadAsByteArrayAsync();
            return (IActionResult?)Results.File(bytes, "application/pdf");
        }
    }
}
