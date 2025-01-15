using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Extensions
{
    public static class HashExtension
    {
        public static string GenerateHash(this string code, out string salt)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            salt = Convert.ToBase64String(saltBytes);
            var combinedBytes = Encoding.UTF8.GetBytes(salt + code);
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
        public static bool VerifyHash( string code, string salt, string hashedCode)
        {
            var combinedBytes = Encoding.UTF8.GetBytes(salt + code);
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(combinedBytes);
                string computedHash = Convert.ToBase64String(hashBytes);
                return computedHash == hashedCode;
            }
        }
    }
}
