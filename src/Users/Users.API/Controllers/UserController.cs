using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserControl;
using Users.Application.Interface;
using Users.Domain.Entities;

namespace Users.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserServices _userService;

        public UserController(IUserServices userService)
        {
            _userService = userService;

        }
        [HttpGet()]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetUsersByAdmin(int page, string? search = null, string? sort = null, string? type = null, string? f = null)
        {
            var (users, totalPages) = await _userService.GetUserForAdmin(page, search, sort, type, f);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new { Items = users, TotalPages = totalPages });

        }

        [HttpGet("profile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetProfile()
        {
            string token = HttpContext.Request.Cookies["accessToken"];

            var userProfile = await _userService.GetUserInfo(token);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(userProfile);
        }

        [HttpGet("userId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetUser(string userId)
        {
            var user = await _userService.GetUserById(userId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }

        [HttpDelete("userId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            var user = await _userService.DeleteUser(userId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(true);
        }
    }
}
