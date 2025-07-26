using test6API.Dtos;
using test6API.Models;

namespace test6API.Services
{
    public interface IChatService
    {
        // Gets all conversations for a specific user (by their email)
        Task<List<Conversation>> GetConversationsForUserAsync(string userEmail);

        // Gets all messages for a specific conversation
        Task<List<Message>> GetMessagesForConversationAsync(int conversationId);

        // Creates a new message
        Task<Message> SendMessageAsync(MessageCreateDto dto);
    }
}