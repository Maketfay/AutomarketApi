using AutomarketApi.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace AutomarketApi.Services.Implementation
{
    internal class HashGenerator : IHashGenerator
    {
        public string HashPassword(string password)
        {
            SHA512 hash = SHA512.Create();

            var passwordBytes = Encoding.Default.GetBytes(password);

            var hashedPassword = hash.ComputeHash(passwordBytes);

            return Convert.ToHexString(hashedPassword);
        }

        public string GenerateSalt()
        {
            var randomNumber = new byte[128];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
