using Auth.Application.Dto;
using Auth.Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserControl;
using Users.Domain.Entities;

namespace Auth.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(204, Type = typeof(LoginResponseDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> LoginUser(LoginUserDto loginUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(loginUser.password))
            {
                ModelState.AddModelError(" ", "Пароль не может быть пустым");
                return StatusCode(422, ModelState);
            }

            var login = await _authService.Login(loginUser);

            return Ok(login);
        }

        [HttpPost("register")]
        //[Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RegistrationUser(RegistrationDto registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(registration.userName))
            {
                ModelState.AddModelError(" ", "Пoле имени не может быть пустым");
                return StatusCode(422, ModelState);
            }

            if (string.IsNullOrEmpty(registration.password))
            {
                ModelState.AddModelError(" ", "Пoле пароля не может быть пустым");
                return StatusCode(422, ModelState);
            }

            await _authService.Registration(registration);

            return Ok(true);
        }
        [HttpGet("access-token")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserInfoByToken()
        {
            var token = HttpContext.Request.Cookies["accessToken"];
            var user = await _authService.GetUser(token);

            var role = new ReturnRole
            {
                role = user.role
            };

            return Ok(role);
        }

        [HttpPost("login/access-token")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetNewToken()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = HttpContext.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Unathorized");

            var response = await _authService.GetNewTokens(token);

            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> logout()
        {
            Response.Cookies.Delete("refreshToken");

            return Ok(true);
        }
    }
}
