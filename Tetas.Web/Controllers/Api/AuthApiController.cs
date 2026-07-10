namespace Tetas.Web.Controllers.Api
{
    using Domain.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Tetas.Common.ViewModels;
    using Tetas.Web.Models.Api;

    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    [IgnoreAntiforgeryToken]
    public class AuthApiController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;

        public AuthApiController(IUserHelper userHelper, IConfiguration configuration)
        {
            _userHelper = userHelper;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var existing = await _userHelper.GetUserByEmailAsync(request.Email);
            if (existing != null)
            {
                return Conflict(new { message = "The email is already registered." });
            }

            var user = new ApplicationUser
            {
                Name = request.FirstName,
                Lastname = request.LastName,
                NickName = request.NickName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.Phone
            };

            var result = await _userHelper.AddUserAsync(user, request.Password);
            if (result != IdentityResult.Success)
            {
                return BadRequest(new { message = "The user couldn't be created.", errors = result.Errors });
            }

            return Ok(BuildToken(user));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userHelper.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            var result = await _userHelper.ValidatePasswordAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            return Ok(BuildToken(user));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                NickName = user.NickName,
                Phone = user.PhoneNumber,
                Bio = user.Bio
            });
        }

        private AuthResponse BuildToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(7);

            var token = new JwtSecurityToken(
                issuer: _configuration["Tokens:Issuer"],
                audience: _configuration["Tokens:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Email = user.Email,
                FullName = user.FullName
            };
        }
    }
}
