using Auth.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Interface
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginUserDto loginUser);
        Task<bool> Registration(RegistrationDto registration);
        Task<UserInfoDto> GetUser(string token);
        Task<LoginResponseDto> GetNewTokens(string refreshToken);
    }
}
