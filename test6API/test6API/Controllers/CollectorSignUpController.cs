using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using test6API.Data;
using test6API.Dtos; // Ensure your DTOs are in this namespace
using test6API.Models;
using test6API.Services; // Ensure your IEmailService is in this namespace

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectorSignUpController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        // Update the constructor to inject IEmailService
        public CollectorSignUpController(ApplicationDbContext context, IConfiguration config, IEmailService emailService)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CResgisterDto dto)
        {
            if (dto.CPassword != dto.CConfirmPassword)
                return BadRequest("Password and Confirm Password do not match.");

            if (await _context.CollectorSignUps.AnyAsync(u => u.CollectorEmail == dto.CEmail))
                return BadRequest("An account with this email already exists.");

            var verificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

            var collector = new CollectorSignUp
            {
                CollectorEmail = dto.CEmail,
                IsEmailVerified = false,
                VerificationToken = verificationToken,
                VerificationTokenExpires = DateTime.UtcNow.AddHours(24)
            };

            var hasher = new PasswordHasher<CollectorSignUp>();
            collector.CollectorPassword = hasher.HashPassword(collector, dto.CPassword);

            _context.CollectorSignUps.Add(collector);
            await _context.SaveChangesAsync();

            var baseUrl = _config["ApiBaseUrl"] ?? $"{Request.Scheme}://{Request.Host}";
            var verificationLink = $"{baseUrl}/api/CollectorSignUp/verify-email?token={verificationToken}";
            var emailBody = $"<h1>Welcome, Collector!</h1><p>Please click the link to verify your email:</p><a href='{verificationLink}'>Verify Email</a>";

            await _emailService.SendEmailAsync(collector.CollectorEmail, "Verify Your Collector Account", emailBody);

            return Ok("Registration successful. Please check your email to verify your account.");
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var collector = await _context.CollectorSignUps.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if (collector == null || collector.VerificationTokenExpires < DateTime.UtcNow)
                return BadRequest("Invalid or expired token.");

            collector.IsEmailVerified = true;
            collector.VerificationToken = null;
            collector.VerificationTokenExpires = null;
            await _context.SaveChangesAsync();

            return Content("<h1>Email Verified Successfully! You can now return to the app.</h1>", "text/html");
        }

        [HttpGet("verification-status")]
        public async Task<IActionResult> GetVerificationStatus([FromQuery] string email)
        {
            var collector = await _context.CollectorSignUps.AsNoTracking().FirstOrDefaultAsync(u => u.CollectorEmail == email);
            return Ok(new { isVerified = collector?.IsEmailVerified ?? false });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(CLoginDto dto)
        {
            var collector = await _context.CollectorSignUps.FirstOrDefaultAsync(u => u.CollectorEmail == dto.CEmail);
            if (collector == null)
                return Unauthorized("Invalid email or password.");

            // Add the verification check
            if (!collector.IsEmailVerified)
                return Unauthorized("Please verify your email before logging in.");

            var hasher = new PasswordHasher<CollectorSignUp>();
            var result = hasher.VerifyHashedPassword(collector, collector.CollectorPassword, dto.CPassword);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid email or password.");

            var token = GenerateJwtToken(collector);
            return Ok(new { token });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var collector = await _context.CollectorSignUps.FirstOrDefaultAsync(u => u.CollectorEmail == dto.Email);
            if (collector != null)
            {
                var resetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
                collector.PasswordResetToken = resetToken;
                collector.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1);
                await _context.SaveChangesAsync();

                var emailBody = $"""
                    <h1>Password Reset Request</h1>
                    <p>To reset your password, open your app and use the following token:</p>
                    <h2 style='text-align:center;'>{resetToken}</h2>
                """;
                await _emailService.SendEmailAsync(collector.CollectorEmail, "Reset Your Collector Password", emailBody);
            }
            return Ok("If an account with this email exists, a password reset token has been sent.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return BadRequest("Passwords do not match.");

            var collector = await _context.CollectorSignUps.FirstOrDefaultAsync(u => u.PasswordResetToken == dto.Token);
            if (collector == null || collector.PasswordResetTokenExpires < DateTime.UtcNow)
                return BadRequest("Invalid or expired password reset token.");

            var hasher = new PasswordHasher<CollectorSignUp>();
            collector.CollectorPassword = hasher.HashPassword(collector, dto.Password);
            collector.PasswordResetToken = null;
            collector.PasswordResetTokenExpires = null;
            await _context.SaveChangesAsync();

            return Ok("Password has been reset successfully.");
        }

        private string GenerateJwtToken(CollectorSignUp collector)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, collector.CollectorId.ToString()),
                new Claim(ClaimTypes.Email, collector.CollectorEmail)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
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
