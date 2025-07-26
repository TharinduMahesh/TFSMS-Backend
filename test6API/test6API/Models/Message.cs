// File Path: ./Models/Message.cs
using System.ComponentModel.DataAnnotations.Schema; // <-- IMPORTANT: Add this using statement

namespace test6API.Models
{
    // We are explicitly telling EF Core which table this model maps to.
    [Table("Messages")]
    public class Message
    {
        public int MessageId { get; set; }
        public int ConversationId { get; set; }

        // The [Column] attribute maps a C# property to a specific database column name.
        // If your database column is named "Sender_Type" or something different,
        // change the name inside the quotes to match it exactly.
        [Column("SenderType")]
        public string SenderType { get; set; }

        [Column("SenderEmail")]
        public string SenderEmail { get; set; }

        [Column("MessageText")]
        public string MessageText { get; set; }

        public DateTime SentAt { get; set; }

        // This tells EF Core how the relationship to the Conversation table works.
        [ForeignKey("ConversationId")]
        public Conversation? Conversation { get; set; }
    }
}
