using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Dtos;
using test6API.Models;

namespace test6API.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;

        public ChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Conversation>> GetConversationsForUserAsync(string userEmail)
        {
            // Find conversations where the user is either the grower or the collector
            return await _context.Conversations
                .Where(c => c.GrowerEmail == userEmail || c.CollectorEmail == userEmail)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Message>> GetMessagesForConversationAsync(int conversationId)
        {
            return await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<Message> SendMessageAsync(MessageCreateDto dto)
        {
            var message = new Message
            {
                ConversationId = dto.ConversationId,
                SenderType = dto.SenderType,
                SenderEmail = dto.SenderEmail,
                MessageText = dto.MessageText,
                SentAt = DateTime.UtcNow // Use UtcNow for consistency
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }
    }
}