using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Dto
{
    public class UserInfoDto
    {
        public string userId { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string role { get; set; }
    }
}
