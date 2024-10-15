using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.Sevices.GenerationPassword
{
    public interface IGenerationPwd
    {
        public Task<string> GeneratePassword(int length);
    }
}
