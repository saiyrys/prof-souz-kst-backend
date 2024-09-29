using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Dto;
using Users.Domain.Entities;

namespace Users.Application.Mapping
{
    public class MappingUser : Profile
    {
        public MappingUser()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<User, CreateUserDto>();

            CreateMap<GetUserDto, User>();
            CreateMap<User, GetUserDto>();

            CreateMap<UserDto, User>()
               /* .ForMember(dest => dest.role, opt => opt.MapFrom(src => Enum.Parse<user_role>(src.role, true)))*/;

            CreateMap<User, UserDto>()
                /*.ForMember(dest => dest.role, opt => opt.MapFrom(src => src.role.ToString()))*/;

        }
    }
}
