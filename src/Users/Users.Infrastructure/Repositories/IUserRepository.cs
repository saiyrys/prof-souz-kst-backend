using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Entities;

namespace Users.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetUserById(string Id);
        public Task<User> GetUserByToken(string token);
        public Task<ICollection<User>> GetAllUser();
        public Task<bool> CreateUser(User user);
        public Task<ICollection<User>> SearchUser(string search = null, string sort = null, string type = null, string f = null);
        public Task<bool> UpdateUser(User user);
        public Task<bool> DeleteUser(User user);


    }
}
