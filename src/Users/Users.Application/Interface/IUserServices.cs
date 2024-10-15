using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Dto;
using Users.Domain.Entities;

namespace Users.Application.Interface
{
    public interface IUserServices
    {
        Task<(IEnumerable<GetUserDto> user, int totalPage)> GetUserForAdmin(int page, string search = null, string sort = null, string type = null, string T = null);
        Task<GetUserDto> GetUserById(string id);
        Task<GetUserDto> GetUserInfo(string token);
        Task<bool> UpdateUser(string userId);
        Task<bool> DeleteUser(string userId);
    }
}
