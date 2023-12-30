using Electronic_document_management.Models;

namespace Electronic_document_management.ViewModels.Profile
{
    public class ProfileModel
    {
        public ProfileModel(User user, string? msg)
        {
            User = user;
            Msg = msg;
        }
        public User User { get; set; }
        public string? Msg { get; set; }
    }
}
