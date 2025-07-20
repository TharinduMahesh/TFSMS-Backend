using paymentManager.Data;
using paymentManager.DTOs;
using paymentManager.Models;
using Microsoft.EntityFrameworkCore;

namespace paymentManager.Services;

public class ChatService
{
    private readonly ApplicationDbContext _context;

    public ChatService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ChatMessage> SendMessageAsync(ChatMessageDto dto)
    {
        var message = new ChatMessage
        {
            SenderId = dto.SenderId,
            ReceiverId = dto.ReceiverId,
            MessageText = dto.MessageText
        };

        _context.ChatMessages.Add(message);
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<List<ChatMessage>> GetConversationAsync(int user1Id, int user2Id)
    {
        return await _context.ChatMessages
            .Where(m => (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                        (m.SenderId == user2Id && m.ReceiverId == user1Id))
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }
}
