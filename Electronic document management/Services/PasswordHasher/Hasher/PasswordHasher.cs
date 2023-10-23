using Electronic_document_management.Services.PasswordHasher.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Electronic_document_management.Services.PasswordHasher.Hasher
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int _keySize = 32;
        private const int _saltSize = 16;
        private const int _iterations = 350000;
        private const char segmentDelimiter = ':';
        private static  readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;
        public string HashPassword(string password) 
        {
            byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                _iterations,
                _algorithm,
                _keySize);
            return string.Join(
                segmentDelimiter,
                Convert.ToHexString(hash),
                Convert.ToHexString(salt),
                _iterations,
                _algorithm);
        }
        public bool VerifyPassword(string input, string hashString)
        {
            string[] segments = hashString.Split(segmentDelimiter);
            var hash = Convert.FromHexString(segments[0]);
            var salt = Convert.FromHexString(segments[1]);
            int iterations = int.Parse(segments[2]);
            HashAlgorithmName algorithm = new HashAlgorithmName(segments[3]);
            var inputHash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                iterations,
                algorithm,
                hash.Length);
            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }
    }
}
