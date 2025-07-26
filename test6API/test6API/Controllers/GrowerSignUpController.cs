using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using test6API.Data;
using test6API.Dtos;
using test6API.Models;
using test6API.Services;

namespace test6API.Controllers
{
    // --- NEW DTOs FOR PASSWORD RESET ---
    public record ForgotPasswordDto(string Email);
    public record ResetPasswordDto(string Token, string Password, string ConfirmPassword);

    [Route("api/[controller]")]
    [ApiController]
    public class GrowerSignUpController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public GrowerSignUpController(ApplicationDbContext context, IConfiguration config, IEmailService emailService)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
        }

        // --- Register, VerifyEmail, and Login endpoints remain the same ---
        [HttpPost("register")]
        public async Task<IActionResult> Register(GRegisterDto dto)
        {
            // ... existing registration logic ...
            if (dto.GPassword != dto.GConfirmPassword) return BadRequest("Passwords do not match.");
            if (await _context.GrowerSignUps.AnyAsync(u => u.GrowerEmail == dto.GEmail)) return BadRequest("Email already exists.");
            var verificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            var grower = new GrowerSignUp { GrowerEmail = dto.GEmail, IsEmailVerified = false, VerificationToken = verificationToken, VerificationTokenExpires = DateTime.UtcNow.AddHours(24) };
            var hasher = new PasswordHasher<GrowerSignUp>();
            grower.GrowerPassword = hasher.HashPassword(grower, dto.GPassword);
            _context.GrowerSignUps.Add(grower);
            await _context.SaveChangesAsync();
            var baseUrl = _config["ApiBaseUrl"] ?? $"{Request.Scheme}://{Request.Host}";
            var verificationLink = $"{baseUrl}/api/GrowerSignUp/verify-email?token={verificationToken}";
            var emailBody = $"<h1>Welcome!</h1><p>Please click the link to verify your email:</p><a href='{verificationLink}'>Verify Email</a>";
            await _emailService.SendEmailAsync(grower.GrowerEmail, "Verify Your Email", emailBody);
            return Ok("Registration successful. Please check your email.");
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            // ... existing verification logic ...
            var grower = await _context.GrowerSignUps.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if (grower == null || grower.VerificationTokenExpires < DateTime.UtcNow) return BadRequest("Invalid or expired token.");
            grower.IsEmailVerified = true;
            grower.VerificationToken = null;
            await _context.SaveChangesAsync();
            return Content("<h1>Email Verified Successfully!</h1>", "text/html");
        }

        [HttpGet("verification-status")]
        public async Task<IActionResult> GetVerificationStatus([FromQuery] string email)
        {
            var grower = await _context.GrowerSignUps.AsNoTracking().FirstOrDefaultAsync(u => u.GrowerEmail == email);
            return Ok(new { isVerified = grower?.IsEmailVerified ?? false });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(GLoginDto dto)
        {
            // ... existing login logic ...
            var grower = await _context.GrowerSignUps.FirstOrDefaultAsync(u => u.GrowerEmail == dto.GEmail);
            if (grower == null || !grower.IsEmailVerified) return Unauthorized("Invalid credentials or email not verified.");
            var hasher = new PasswordHasher<GrowerSignUp>();
            if (hasher.VerifyHashedPassword(grower, grower.GrowerPassword, dto.GPassword) == PasswordVerificationResult.Failed) return Unauthorized("Invalid credentials.");
            var token = GenerateJwtToken(grower);
            return Ok(new { token });
        }


        // --- START: NEW FORGOT PASSWORD ENDPOINTS ---

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var grower = await _context.GrowerSignUps.FirstOrDefaultAsync(u => u.GrowerEmail == dto.Email);
            if (grower == null)
            {
                // To prevent email enumeration, always return a success-like message.
                return Ok("If an account with this email exists, a password reset link has been sent.");
            }

            // Generate a password reset token
            var resetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            grower.PasswordResetToken = resetToken;
            grower.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1); // Token is valid for 1 hour

            await _context.SaveChangesAsync();

            // IMPORTANT: For a real app, this link should point to your Flutter app's reset password screen.
            // This requires setting up deep linking. For now, it's a placeholder.
            // --- REPLACE WITH THIS NEW CODE ---
            var emailBody = $"""
                    <h1>Password Reset Request</h1>
                    <p>You have requested to reset your password. Open your app and use the following token on the 'Reset Password' screen:</p>
                    <h2 style='text-align:center; letter-spacing: 2px; color: #0a4e41;'>{resetToken}</h2>
                    <p>This token will expire in one hour.</p>
                """;
            // --- END OF CHANGE ---

            await _emailService.SendEmailAsync(grower.GrowerEmail, "Reset Your Password", emailBody);

            return Ok("If an account with this email exists, a password reset link has been sent.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            var grower = await _context.GrowerSignUps.FirstOrDefaultAsync(u => u.PasswordResetToken == dto.Token);

            if (grower == null || grower.PasswordResetTokenExpires < DateTime.UtcNow)
            {
                return BadRequest("Invalid or expired password reset token.");
            }

            // Hash the new password and update the user
            var hasher = new PasswordHasher<GrowerSignUp>();
            grower.GrowerPassword = hasher.HashPassword(grower, dto.Password);

            // Clear the reset token fields
            grower.PasswordResetToken = null;
            grower.PasswordResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return Ok("Password has been reset successfully.");
        }

        // --- END: NEW FORGOT PASSWORD ENDPOINTS ---

        private string GenerateJwtToken(GrowerSignUp grower)
        {
            // ... existing token generation logic ...
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, grower.GrowerId.ToString()), new Claim(ClaimTypes.Email, grower.GrowerEmail) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: _config["Jwt:Issuer"], audience: _config["Jwt:Audience"], claims: claims, expires: DateTime.UtcNow.AddHours(2), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
