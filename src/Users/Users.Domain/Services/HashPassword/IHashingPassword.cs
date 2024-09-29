using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.Services.HashPassword
{
    public interface IHashingPassword
    {
        public Task<(string HashedPassword, byte[] Salt)> HashPassword(string password);

        public byte[] GenerateSalt();

        public Task<bool> VerifyPassword(string enteredPassword, string storedHash, string storedSalt);
    }
}
