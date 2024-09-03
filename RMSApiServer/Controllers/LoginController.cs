using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMSApiServer.Models;
using System.Security.Cryptography;
using System.Text;

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
               .Where(u => u.Email == login.Email)
               .FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var hashedPassword = HashPassword(login.Password);

            if (user.PasswordHash != hashedPassword)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _tokenService.GenerateToken(user.UserName);
            return Ok(new { Token = token });
        }

        // POST: api/login/signup
        [HttpPost("signup")]
        public async Task<ActionResult<User>> CreateUser(User user)
        {

            bool isUserNameTaken = await _context.User.AnyAsync(u => u.UserName == user.UserName);
            bool isEmailTaken = await _context.User.AnyAsync(u => u.Email == user.Email);

            if (isUserNameTaken)
            {
                return BadRequest($"User name {user.UserName} is not available");
            }

            if (isEmailTaken)
            {
                return BadRequest($"An account with the email {user.Email} already exists");
            }

            // Hash the password before saving it to the database
            user.PasswordHash = HashPassword(user.PasswordHash);

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

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Compute the hash of the password
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a string
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
