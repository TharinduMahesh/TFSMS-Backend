using System.ComponentModel.DataAnnotations;

namespace test6API.Models
{
    public class CollectorSignUp
    {
        [Key]
        public int CollectorId { get; set; }

        [Required, EmailAddress]
        public string CollectorEmail { get; set; } = string.Empty;

        [Required]
        public string CollectorPassword { get; set; } = string.Empty;

        // This property is not needed for the authentication flow, but you can keep it if you use it elsewhere.
        // public DateTime CollectorSignUpdate { get; set; } = DateTime.Now;

        // --- ADD THESE PROPERTIES FOR AUTHENTICATION ---
        public bool IsEmailVerified { get; set; } = false;
        public string? VerificationToken { get; set; }
        public DateTime? VerificationTokenExpires { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }
    }
}