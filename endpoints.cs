using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MyDbContext _context;

        public AuthController(MyDbContext context)
        {
            _context = context;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (await _context.Users.AnyAsync(x => x.Username == user.Username))
            {
                return BadRequest("Username already exists");
            }

            var newUser = new User
            {
                Username = user.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                Email = user.Email
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == user.Username);

            if (existingUser == null)
            {
                return Unauthorized("Invalid username or password");
            }

            if (!BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                new Claim(ClaimTypes.Name, existingUser.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}
