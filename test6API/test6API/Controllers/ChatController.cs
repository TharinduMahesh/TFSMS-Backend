// File Path: ./Controllers/ChatController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using test6API.Dtos;
using test6API.Hubs;
using test6API.Models;
using test6API.Services;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IChatService chatService, IHubContext<ChatHub> hubContext)
        {
            _chatService = chatService;
            _hubContext = hubContext;
        }

        // GET: api/chat/conversations/grower1@example.com
        [HttpGet("conversations/{userEmail}")]
        public async Task<ActionResult<List<Conversation>>> GetConversations(string userEmail)
        {
            var conversations = await _chatService.GetConversationsForUserAsync(userEmail);
            return Ok(conversations);
        }

        // GET: api/chat/messages/1
        [HttpGet("messages/{conversationId}")]
        public async Task<ActionResult<List<Message>>> GetMessages(int conversationId)
        {
            var messages = await _chatService.GetMessagesForConversationAsync(conversationId);
            return Ok(messages);
        }

        // POST: api/chat/messages
        [HttpPost("messages")]
        public async Task<ActionResult<Message>> SendMessage([FromBody] MessageCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1. Save the message to the database
            var newMessage = await _chatService.SendMessageAsync(dto);

            // 2. Use SignalR to push the new message to all connected clients in real-time
            // The group name is the conversation ID.
            // The method name "ReceiveMessage" is what your Flutter client will listen for.
            await _hubContext.Clients.Group(dto.ConversationId.ToString())
                             .SendAsync("ReceiveMessage", newMessage);

            return Ok(newMessage);
        }
    }
}
