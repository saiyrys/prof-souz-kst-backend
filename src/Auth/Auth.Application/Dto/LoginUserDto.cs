using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Entities;

namespace Auth.Application.Dto
{
    public class LoginUserDto 
    {
        public string userName { get; set; }

        public string password { get; set; }
    }
}
