using BookStore.Application.DTOs.Auth;
using BookStore.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenService _jwt;

        public AuthController(UserManager<AppUser> userManager, IJwtTokenService jwt)
        {
            _userManager = userManager;
            _jwt = jwt;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return Unauthorized();
            var valid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!valid) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _jwt.CreateTokenAsync(user, roles);

            return Ok(new { token });
        }
    }
}
