using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.Services.HashPassword
{
    public class HashingPassword : IHashingPassword
    {
        public async Task<(string HashedPassword, byte[] Salt)> HashPassword(string password)
        {
            using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                var salt = GenerateSalt();
                hasher.Salt = salt;
                hasher.DegreeOfParallelism = 8;
                hasher.MemorySize = 65536; // 64MB
                hasher.Iterations = 10;

                var hashedPassword = Convert.ToBase64String(hasher.GetBytes(32));

                return (hashedPassword, salt);
            }
        }
        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[64];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public async Task<bool> VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            byte[] salt = Convert.FromBase64String(storedSalt);

            using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(enteredPassword)))
            {
                hasher.Salt = salt;
                hasher.DegreeOfParallelism = 8;
                hasher.MemorySize = 65536; // 64MB
                hasher.Iterations = 10;

                var computedHash = Convert.ToBase64String(hasher.GetBytes(32));

                return storedHash.Equals(computedHash);
            }
        }
    }
}
