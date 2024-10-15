using AutoMapper;
using Category.Application.Dto;
using Category.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Application.Mapping
{
    public class MappingCategory : Profile
    {
        public MappingCategory()
        {
            CreateMap<CategoryDto, Categories>()
                .ForMember(dest => dest.color, opt => opt.MapFrom(src => Enum.Parse<color_category>(src.color, true)));

            CreateMap<Categories, CategoryDto>()
                .ForMember(dest => dest.color, opt => opt.MapFrom(src => src.color.ToString()));

        }
    }
}
