using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Dto;
using Users.Domain.Entities;

namespace Auth.Application.Dto
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public UserInfoDto User { get; set; }
    }
}
