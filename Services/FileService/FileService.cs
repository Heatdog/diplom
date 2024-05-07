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

            using (var content = new MultipartFormDataContent()){
                var fileStream = file.OpenReadStream();
                content.Add(new StreamContent(fileStream), "file", file.FileName);

                var request = new HttpRequestMessage(HttpMethod.Post, connection+"/insert"){
                    Content = content
                };

                var respone = await _authClient.SendAsync(request);

                if (!respone.IsSuccessStatusCode){
                    Console.WriteLine(respone.StatusCode);
                    return null;
                }
                var id = await respone.Content.ReadAsStringAsync();

                id = id[1..^1];
                Console.WriteLine(id);

                return new Guid(id);
            }
        }

        public async Task<byte[]?> GetFile(Guid id){
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

            return bytes;
        }
    }
}
