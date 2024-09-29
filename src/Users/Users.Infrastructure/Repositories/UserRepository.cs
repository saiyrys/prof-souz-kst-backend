using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Entities;
using Users.Infrastructure.Data;

namespace Users.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<User>> GetAllUser()
        {
            return await _context.users.OrderBy(u => u.userId).ToListAsync(); 
        }

        public async Task<User> GetUserById(string Id)
        {
            return await _context.users.Where(u => u.userId == Id).FirstOrDefaultAsync();
        }

        public Task<User> GetUserByToken(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateUser(User user)
        {
            _context.Add(user);
            return await Save();
        }

        public async Task<ICollection<User>> SearchUser(string search = null, string sort = null, string type = null, string f = null)
        {
            IQueryable<User> query = _context.users;

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();

                query = query.Where(u =>
                    u.userName.ToLower().Contains(search) ||
                    u.firstName.ToLower().Contains(search) ||
                    u.lastName.ToLower().Contains(search) ||
                    u.middleName.ToLower().Contains(search) ||
                    u.email.ToLower().Contains(search) 
                    );
            }

            /*if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(type))
            {
                switch (sort.ToLower())
                {
                    case "alphabetic":
                        if (type.ToLower() == "asc")
                            query = query.OrderBy(u => u.userName);
                        else if (type.ToLower() == "desc")
                            query = query.OrderByDescending(u => u.userName);
                        break;
                    case "role":
                        if (type.ToLower() == "asc")
                            query = query.OrderBy(u => u.role == "USER");
                        else if (type.ToLower() == "desc")
                            query = query.OrderBy(u => u.role == "ADMIN");
                        break;
                    case "createdata":
                        if (type.ToLower() == "asc")
                            query = query.OrderBy(u => u.createdAt);
                        else if (type.ToLower() == "desc")
                            query = query.OrderByDescending(u => u.createdAt);
                        break;
                    case "updatedata":
                        if (type.ToLower() == "asc")
                            query = query.OrderBy(u => u.updatedAt);
                        else if (type.ToLower() == "desc")
                            query = query.OrderByDescending(u => u.updatedAt);
                        break;
                }
            }*/

            return await query.ToListAsync();
        }

        public async Task<bool> UpdateUser(User user)
        {
            _context.Update(user);

            return await Save();
        } 
        public async Task<bool> DeleteUser(User user)
        {
            _context.Remove(user);

            return await Save();
        }

        private async Task<bool> Save()
        {
           await _context.SaveChangesAsync();

           return true;
        }


    }
}
