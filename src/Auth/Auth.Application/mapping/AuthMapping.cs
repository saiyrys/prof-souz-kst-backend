using Auth.Application.Dto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Dto;
using Users.Domain.Entities;

namespace Auth.Application.mapping
{
    public class AuthMapping : Profile
    {
        public AuthMapping()
        {
            CreateMap<LoginUserDto, User>();
            CreateMap<User, LoginUserDto>();

            CreateMap<LoginUserDto, GetUserDto>();
            CreateMap<GetUserDto, LoginUserDto>();

            CreateMap<UserInfoDto, User>();
            CreateMap<User, UserInfoDto>();

            CreateMap<GetUserDto, User>();
            CreateMap<User, GetUserDto>();

            /* CreateMap<RegistrationDto, GetUserDto>();
             CreateMap<GetUserDto, RegistrationDto>();*/

            CreateMap<RegistrationDto, User>()
                .ForMember(dest => dest.role, opt => opt.MapFrom(src => Enum.Parse<user_role>(src.role, true)));

            CreateMap<User, RegistrationDto>()
                .ForMember(dest => dest.role, opt => opt.MapFrom(src => src.role.ToString()));

            CreateMap<ReturnRole, User>();
            CreateMap<User, ReturnRole>();
        }

    }
}
