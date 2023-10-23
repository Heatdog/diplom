namespace Electronic_document_management.Services.PasswordHasher.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string input, string hashString);
    }
}
