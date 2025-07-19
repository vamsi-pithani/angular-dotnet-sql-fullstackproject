using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotnetApi.Data;
using DotnetApi.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace DotnetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("userId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("SIGN_UP")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return BadRequest(new { message = "Email already exists" });
            }

            user.Email = user.Email.ToLower();
            user.Username = user.Username;
            user.Password = HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                message = "Account Created",
                status = true,
                data = new
                {
                    userId = user.Id,
                    username = user.Username,
                    email = user.Email,
                    token = token,
                    expiredToken = 3600
                }
            });
        }

        [HttpPost("LOGIN")]
        public async Task<IActionResult> Login([FromBody] User loginUser)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginUser.Email.ToLower());
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid Email Address" });
            }

            var hashedPassword = HashPassword(loginUser.Password);
            if (user.Password != hashedPassword)
            {
                return Unauthorized(new { message = "Invalid Email Address or Password" });
            }

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                message = "Login Successfully!",
                status = true,
                data = new
                {
                    userId = user.Id,
                    username = user.Username,
                    email = user.Email,
                    token = token,
                    expiredToken = 3600
                }
            });
        }
    }
}
