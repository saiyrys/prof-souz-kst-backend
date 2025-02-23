using Category.Application.Dto;
using Category.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Application.Interface
{
    public interface ICategoryService
    {
        Task<bool> CreateCategories(CategoryDto category);

        Task<List<Categories>> GetAllCategories();

        Task<CategoryDto> GetCategory(string id);

        Task<bool> UpdateCategory(string id);

        Task<bool> DeleteCategories(string id);
    }
}
