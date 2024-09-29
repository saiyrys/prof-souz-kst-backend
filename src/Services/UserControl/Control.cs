using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Entities;
using Users.Infrastructure.Data;

namespace UserControl
{
    public class Control<TUser> : IControl<TUser> where TUser : class
    {
        private readonly ApplicationDbContext Context;
        private readonly IHttpContextAccessor HttpContext;

        protected virtual CancellationToken CancellationToken => CancellationToken.None;
        public Control(ApplicationDbContext context, IHttpContextAccessor httpContext)
        {
            Context = context;

            HttpContext = httpContext;

        }
        public virtual async Task<TUser> FindByIdAsync(string userId)
        {
            return await Context.Set<TUser>().FirstOrDefaultAsync(u => EF.Property<string>(u, "userId") == userId, CancellationToken);
        }

        public virtual async Task<TUser> FindByNameAsync(string userName)
        {
            return await Context.Set<TUser>().FirstOrDefaultAsync(u => EF.Property<string>(u, "userName") == userName, CancellationToken);
        }

        public virtual async Task<TUser> FindByTokenAsync(string token)
        {
            string key = Constants.JWT_SECRET_KEY;

            var tokenHandler = new JwtSecurityTokenHandler();

            var readToken = tokenHandler.ReadJwtToken(token);

            var claims = readToken.Claims;

            string userId = claims.FirstOrDefault(x => x.Type == "nameid" || x.Type == "sub")?.Value;

            TUser user = await FindByIdAsync(userId);

            return user;
        }

        public async Task<string> VerifyByTokenAsync()
        {
            string token = HttpContext.HttpContext.Request.Headers["Authorization"];

            if (token.StartsWith("Bearer"))
                token = token.Substring("Bearer".Length).Trim();

            return token;
        }
    }
}
