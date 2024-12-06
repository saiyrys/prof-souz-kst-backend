using Category.Application.Dto;
using Category.Application.Interface;
using Category.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;

namespace Category.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        ICategoryService _category;
        
        public CategoryController(ICategoryService category)
        {
            _category = category;
        }

        [HttpPost]
        //[Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Create(CategoryDto category)
        {
            if (category is null)
                throw new BadRequestException("unfilled");

            var categories = await _category.CreateCategories(category);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(categories);
        }

        [HttpGet]
        //[Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var categories = await _category.GetAllCategories();

            return Ok(categories);
        }

        [HttpDelete("categoryId")]
        //[Authorize(Roles = "ADMIN, MODER")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory(string categoryId)
        {
            var deletedCategory = await _category.DeleteCategories(categoryId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(true);
        }
    }
}
