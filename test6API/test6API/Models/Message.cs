using System.ComponentModel.DataAnnotations;

namespace test6API.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        // The ID of the user who sent the message.
        [Required]
        public int SenderId { get; set; }

        // The ID of the user who received the message.
        [Required]
        public int ReceiverId { get; set; }

        // The role of the sender ("Grower" or "Collector")
        [Required]
        public string SenderType { get; set; }

        // The role of the receiver ("Grower" or "Collector")
        [Required]
        public string ReceiverType { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
    }
}