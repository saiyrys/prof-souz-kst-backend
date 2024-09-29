using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.Sevices.GenerationPassword
{
    public class GenerationPwd : IGenerationPwd
    {
        private static readonly char[] Symbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        public async Task<string> GeneratePassword(int length)
        {
            Random random = new Random();

            var passwordSymbols = new char[length];

            using(var rng = new RSACryptoServiceProvider())
            {
                var data = new byte[length];
                for(int i = 0; i < length; i++)
                {
                    passwordSymbols[i] = Symbols[data[i] % Symbols.Length];
                }
            }

            return new string(passwordSymbols);
        }
    }
}
