using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMSApiServer.Models;

namespace RMSApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _tokenService;

        public LoginController(AppDbContext context, JwtTokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // POST: api/Login/SignUp
        [HttpPost("SignUp")]
        public async Task<ActionResult<User>> CreateUser(User user)
        {

            if (user == null)
            {
                return BadRequest("Login data is null.");
            }

            user.LastLogin = DateTime.Now;
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            var response = new
            {
                user.UserId,
                user.UserName,
                user.Email,
                LastLogin = user.LastLogin.ToString("yyyy/MM/dd HH:mm:ss"),
                CreatedAt = user.CreatedAt.ToString("yyyy/MM/dd HH:mm:ss"),
                UpdatedAt = user.UpdatedAt.ToString("yyyy/MM/dd HH:mm:ss")
            };

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, response);

        }

        // GET: api/Login/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {

            var user = await _context.User
               .Where(u => u.Email == login.Email && u.PasswordHash == login.Password)
               .FirstOrDefaultAsync();

            if (user != null)
            {
                var token = _tokenService.GenerateToken(user.UserName);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
