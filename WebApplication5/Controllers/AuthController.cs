using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApplication5.DTO;
using WebApplication5.DTO_Put;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly DataContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthController(DataContext dbcontext, IConfiguration configuration)
        {
            _dbContext = dbcontext;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(AuthDTO req)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Username == req.Username))
            {
                return BadRequest("Username already exists");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(req.Password);

            var newUser = new User
            {
                Username = req.Username,
                PasswordHash = passwordHash,
                Role= "User",
            };
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
            return Ok(newUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(AuthDTO req)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == req.Username);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password");
            }
            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken (User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        [HttpGet]
        public async Task<List<User>> GetUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        [HttpPut]
        [Route("role/{id}")]
 
        public async Task<ActionResult<User>> ChangeRole(int id, RoleDTO req)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Role = req.Role;

            try
            {
                await _dbContext.SaveChangesAsync();
                return user;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> PutUser(int id, AuthPutDTO req)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Fullname= req.Fullname;
            user.PhoneNumber = req.PhoneNumber;
            user.Address = req.Address;

            try
            {
                await _dbContext.SaveChangesAsync();
                return user;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool UserExists(int id)
        {
            return _dbContext.Users.Any(e => e.Id == id);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
        [HttpGet]
        [Route("Users/current")]
        [Authorize]
        public async Task<IActionResult> getLoggedInUserID()
        {
            var usernameClaim = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (usernameClaim != null)
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == usernameClaim);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            return BadRequest();
        }
    }
}
