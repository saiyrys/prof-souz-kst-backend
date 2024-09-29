using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Entities;

namespace UserControl
{
    public interface IControl<TUser> 
    {
        Task<TUser> FindByIdAsync(string userId);
        Task<TUser> FindByNameAsync(string userName);
        Task<TUser> FindByTokenAsync(string token);
        Task<string> VerifyByTokenAsync();
    }
}
