using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Dto;
using Users.Domain.Entities;
using Users.Domain.Services.HashPassword;

namespace Users.Domain.Factories
{
    public abstract class UserFactory
    {
        public abstract Task<User> CreateUser(string userName, string firstName, string lastName, string middleName, string email, string password, string role);
        public abstract Task<UserDto> CreateUserForSendByEmail(string userName, string password, string role);
        protected string GenerationUniqueId()
            => Guid.NewGuid().ToString();
        /*protected string CheckUserRole(user_role role)
        {
            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException("Роль не была указана");

            if (role == "ADMIN" || role == "MODER")
                return role;

            return "USER";
        }*/
    }
}
