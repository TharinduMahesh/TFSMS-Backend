// Controllers/GrowerSignUpController.cs
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using test6API.Data;
using test6API.Dtos;
using test6API.Models;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrowerSignUpController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public GrowerSignUpController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(GRegisterDto dto)
        {
            // 🔐 Step 1: Check if Password and ConfirmPassword match
            if (dto.GPassword != dto.GConfirmPassword)
                return BadRequest("Password and Confirm Password do not match.");

            // Step 2: Check if the email is already registered
            if (await _context.GrowerSignUps.AnyAsync(u => u.GrowerEmail == dto.GEmail))
                return BadRequest("Email already exists.");

            // Step 3: Hash the password
            var hasher = new PasswordHasher<string>();
            var hashedPassword = hasher.HashPassword(null, dto.GPassword);

            // Step 4: Create new user and save
            var grower = new GrowerSignUp
            {
                GrowerEmail = dto.GEmail,
                GrowerPassword = hashedPassword
            };

            _context.GrowerSignUps.Add(grower);
            await _context.SaveChangesAsync();

            return Ok("Registered successfully.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(GLoginDto dto)
        {
            var grower = await _context.GrowerSignUps.FirstOrDefaultAsync(u => u.GrowerEmail == dto.GEmail);
            if (grower == null)
                return Unauthorized("Invalid email or password.");

            var hasher = new PasswordHasher<string>();
            var result = hasher.VerifyHashedPassword(null, grower.GrowerPassword, dto.GPassword);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid email or password.");

            var token = GenerateJwtToken(grower);
            return Ok(new { token });
        }

        private string GenerateJwtToken(GrowerSignUp grower)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, grower.GrowerId.ToString()),
            new Claim(ClaimTypes.Email, grower.GrowerEmail)
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
