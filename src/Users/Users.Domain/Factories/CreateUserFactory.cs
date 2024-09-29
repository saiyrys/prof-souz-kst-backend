using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Dto;
using Users.Domain.Entities;
using Users.Domain.Services;
using Users.Domain.Services.HashPassword;
using Users.Domain.Sevices.GenerationPassword;

namespace Users.Domain.Factories
{
    public class CreateUserFactory : UserFactory
    {
        public override async Task<User> CreateUser(string userName, string firstName, string lastName, string middleName, string email, string password, string role)
        {
            HashingPassword hashing = new();

            var (hashedPassword, salt) = await hashing.HashPassword(password);

            return new User
            {
                userId = GenerationUniqueId(),
                userName = userName,
                firstName = firstName,
                lastName = lastName,
                middleName = middleName,
                email = email,
                password = hashedPassword,
                salt = Convert.ToBase64String(salt),
                role = Enum.Parse<user_role>(role)
            };
        }
        public override async Task<UserDto> CreateUserForSendByEmail(string userName, string password, string role)
        {
            return new UserDto
            {
                userId = GenerationUniqueId(),
                userName = userName,
                password = password,
               /* role = CheckUserRole(role)*/
            };
        }
    }
}
