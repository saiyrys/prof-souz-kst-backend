using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.Entities
{
    public partial class User 
    {
        public string userId { get; set; } = null!;
        public string userName { get; set; } = null!;
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string? middleName { get; set; }
        public string? email { get; set; }
        public string password { get; set; } = null!;
        public string salt { get; set; } = null!;
        public user_role role { get; set; } 
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
    }

    public enum user_role
    {
        USER,
        ADMIN,
        MODER

    }

}
