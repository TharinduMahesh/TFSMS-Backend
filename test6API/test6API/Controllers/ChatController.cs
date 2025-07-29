using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("history/{senderId}/{senderType}/{receiverId}/{receiverType}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetChatHistory(int senderId, string senderType, int receiverId, string receiverType)
        {
            var messages = await _context.Messages
                .Where(m =>
                    (m.SenderId == senderId && m.SenderType == senderType && m.ReceiverId == receiverId && m.ReceiverType == receiverType) ||
                    (m.SenderId == receiverId && m.SenderType == receiverType && m.ReceiverId == senderId && m.ReceiverType == senderType)
                )
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return Ok(messages);
        }
    }
}
