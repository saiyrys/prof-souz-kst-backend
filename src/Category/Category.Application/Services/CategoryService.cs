using AutoMapper;
using Category.Application.Dto;
using Category.Application.Interface;
using Category.Infrastructure.Data;
using Category.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Application.Services
{
    public class CategoryService : ICategoryService
    {
        ApplicationDbContext _context;
        IMapper _mapper;

        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateCategories(CategoryDto category)
        {
            if (category is null)
                throw new ArgumentException("Unfilled");

            var categoryDto = _mapper.Map<Categories>(category);

            categoryDto.categoriesId = Guid.NewGuid().ToString();

            _context.Add(categoryDto);

            await Save();

            return true;

        }
        public async Task<List<Categories>> GetAllCategories()
        {
            var categories = await _context.Categories.OrderBy(c => c.categoriesId).ToListAsync();

            if (categories is null)
                throw new ArgumentNullException("Categories not find");

            return categories;
        }

        public Task<CategoryDto> GetCategory(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateCategory(string id)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteCategories(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("uncorrect id");

            var deletedCategory = await _context.Categories.FirstOrDefaultAsync(c => c.categoriesId == id);

            if (deletedCategory is null)
                throw new ArgumentException("category undefined");
            
            await Save();

            var delete = _context.Remove(deletedCategory);

            return true;
        }

        private async Task<bool> Save()
        {
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
