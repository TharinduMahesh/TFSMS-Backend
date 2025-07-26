// File Path: ./Models/Conversation.cs
using System.ComponentModel.DataAnnotations.Schema;

namespace test6API.Models
{
    public class Conversation
    {
        public int ConversationId { get; set; }

        public int GrowerAccountId { get; set; }
        public string GrowerEmail { get; set; }

        public int CollectorAccountId { get; set; }
        public string CollectorEmail { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties (optional, but good practice)
        [ForeignKey("GrowerAccountId")]
        public GrowerCreateAccount? Grower { get; set; }

        [ForeignKey("CollectorAccountId")]
        public CollectorAccount? Collector { get; set; }

        public List<Message>? Messages { get; set; }
    }
}
