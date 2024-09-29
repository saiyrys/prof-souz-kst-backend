using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Entities;

namespace Auth.Application.Dto
{
    public class RegistrationDto
    {
        public string? userName { get; set; }
        public string? firstName { get; set; }
        public string? middleName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string role { get; set; }
        public string? password { get; set; }
    }
    
}
