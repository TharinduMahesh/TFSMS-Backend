using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Models;

namespace test6API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConversationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userType}/{userId}")]
        public async Task<IActionResult> GetConversations(string userType, int userId)
        {
            IQueryable<Conversation> query = null;

            if (userType.ToLower() == "grower")
                query = _context.Conversations.Where(c => c.GrowerAccountId == userId);
            else if (userType.ToLower() == "collector")
                query = _context.Conversations.Where(c => c.CollectorAccountId == userId);
            else
                return BadRequest("Invalid userType");

            var conversations = await query
                .Include(c => c.Grower)
                .Include(c => c.Collector)
                .ToListAsync();

            return Ok(conversations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateConversation([FromBody] Conversation conversation)
        {
            // Optionally check if conversation exists already between these two
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
            return Ok(conversation);
        }
    }

}
