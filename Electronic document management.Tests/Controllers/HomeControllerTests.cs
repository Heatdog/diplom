using Xunit;
using Electronic_document_management.Controllers.Home;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Electronic_document_management.Services.Repository.Interfaces;
using Electronic_document_management.Models;
using System.Security.Claims;
using Electronic_document_management.
using Microsoft.AspNetCore.Http;

namespace Electronic_document_management.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void MainResultNotNull()
        {
            HomeController controller = new HomeController();
            var results = controller.Main() as ViewResult;
            Assert.NotNull(results);
        }
        [Fact]
        public void MainResultWithAListOfUsers() 
        {
            var user = new User("test", "Иван", "Иванов", "test@gmail.com", "1", Role.Worker);
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjIiLCJ1c2VybmFtZSI6IkhlYXRlciIsInJvbGUiOiJIZWFkT2ZEZXBhcnRtZW50IiwiZXhwIjoxNjk3ODg3ODQ5LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiQXV0aENsaWVudCJ9.66juDCP32Xt5qBEkkePKUrZd8ZFaevM0tGtWT6lgvuU";
            var controller = new HomeController();
            
            var identity = ClaimsPrincipal.Current?.Identities.First();
            var username = identity?.Claims.First(c => c.Type == "username");
            Assert.Null(username);
        }
    }
}
