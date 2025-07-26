using System.ComponentModel.DataAnnotations;

namespace test6API.Models
{
    public class GrowerSignUp
    {
        [Key]
        public int GrowerId { get; set; }

        [Required]
        [EmailAddress]
        public string GrowerEmail { get; set; } = string.Empty;

        [Required]
        public string GrowerPassword { get; set; } = string.Empty;

        // --- Properties for Email Verification ---
        public bool IsEmailVerified { get; set; } = false;
        public string? VerificationToken { get; set; }
        public DateTime? VerificationTokenExpires { get; set; }

        // --- ADD THESE PROPERTIES FOR PASSWORD RESET ---
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }
    }
}
