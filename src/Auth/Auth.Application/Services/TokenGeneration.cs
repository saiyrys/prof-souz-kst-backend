using Auth.Application.Constants;
using Auth.Application.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Application.Dto;
using Users.Domain.Entities;

namespace Auth.Application.Services
{
    public class TokenGeneration
    {
        private readonly IConfiguration Configuration;
        private readonly IHttpContextAccessor _httpContext;

        private byte[] key = Encoding.UTF8.GetBytes(Auth_constants.JWT_SECRET_KEY);

        public TokenGeneration(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            Configuration = configuration;
            _httpContext = httpContext;
        }

        private async Task<string> GenerateAccessToken(UserInfoDto user)
        {
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()),
                new Claim(ClaimTypes.Role, user.role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenJson = tokenHandler.WriteToken(token);

            return await Task.FromResult(tokenJson);
        }

        private async Task<string> GenerateRefreshToken(UserInfoDto user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
                );

            var refreshToken = new JwtSecurityTokenHandler().WriteToken(token);

            SetCookieRefresh(refreshToken);

            return refreshToken;
        }

        private void SetCookieRefresh(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.Lax,
                Secure = true,
            };

            _httpContext.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        public async Task<(string, string)> GenerateToken(UserInfoDto user)
        {
            var accessToken = await GenerateAccessToken(user);

            var refreshToken = await GenerateRefreshToken(user);

            return (accessToken, refreshToken);
        }
    }
}
