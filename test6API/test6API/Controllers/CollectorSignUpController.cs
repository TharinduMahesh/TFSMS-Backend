using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using test6API.Data;
using test6API.Dtos;
using test6API.Models;
using Microsoft.EntityFrameworkCore;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectorSignUpController: ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public CollectorSignUpController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CResgisterDto dto)
        {
            // 🔐 Step 1: Check if Password and ConfirmPassword match
            if (dto.CPassword != dto.CConfirmPassword)
                return BadRequest("Password and Confirm Password do not match.");

            // Step 2: Check if the email is already registered
            if (await _context.CollectorSignUps.AnyAsync(u => u.CollectorEmail == dto.CEmail))
                return BadRequest("Email already exists.");

            // Step 3: Hash the password
            var hasher = new PasswordHasher<string>();
            var hashedPassword = hasher.HashPassword(null, dto.CPassword);

            // Step 4: Create new user and save
            var collector = new CollectorSignUp
            {
                CollectorEmail = dto.CEmail,
                CollectorPassword = hashedPassword
            };

            _context.CollectorSignUps.Add(collector);
            await _context.SaveChangesAsync();

            return Ok("Registered successfully.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(CLoginDto dto)
        {
            var collector = await _context.CollectorSignUps.FirstOrDefaultAsync(u => u.CollectorEmail == dto.CEmail);
            if (collector == null)
                return Unauthorized("Invalid email or password.");

            var hasher = new PasswordHasher<string>();
            var result = hasher.VerifyHashedPassword(null, collector.CollectorPassword, dto.CPassword);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid email or password.");

            var token = GenerateJwtToken(collector);
            return Ok(new { token });
        }

        private string GenerateJwtToken(CollectorSignUp collector)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, collector.CollectorId.ToString()),
            new Claim(ClaimTypes.Email, collector.CollectorEmail)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
