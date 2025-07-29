using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Dtos;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("details")]
        public async Task<ActionResult<ChatUserDto>> GetUserDetails([FromQuery] string email, [FromQuery] string userType)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(userType))
            {
                return BadRequest("Email and userType are required.");
            }

            var cleanEmail = email.Trim().ToLower();

            if (userType.Equals("Grower", System.StringComparison.OrdinalIgnoreCase))
            {
                var grower = await _context.GrowerCreateAccounts
                    .Where(g => g.GrowerEmail.Trim().ToLower() == cleanEmail)
                    .Select(g => new ChatUserDto
                    {
                        Id = g.GrowerAccountId,
                        FullName = g.GrowerFirstName + " " + g.GrowerLastName,
                        UserType = "Grower"
                    })
                    .FirstOrDefaultAsync();

                if (grower != null) return Ok(grower);
            }
            else if (userType.Equals("Collector", System.StringComparison.OrdinalIgnoreCase))
            {
                var collector = await _context.CollectorCreateAccounts
                    .Where(c => c.CollectorEmail.Trim().ToLower() == cleanEmail)
                    .Select(c => new ChatUserDto
                    {
                        Id = c.CollectorAccountId,
                        FullName = c.CollectorFirstName + " " + c.CollectorLastName,
                        UserType = "Collector"
                    })
                    .FirstOrDefaultAsync();

                if (collector != null) return Ok(collector);
            }
            else
            {
                return BadRequest("Invalid userType specified.");
            }

            return NotFound(new { message = $"User with email {email} and type {userType} not found." });
        }

        // NEW ENDPOINT: Gets a list of all growers.
        [HttpGet("growers")]
        public async Task<ActionResult<IEnumerable<ChatUserDto>>> GetGrowers([FromQuery] string? search)
        {
            var query = _context.GrowerCreateAccounts.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var cleanSearch = search.Trim().ToLower();
                query = query.Where(g =>
                    g.GrowerFirstName.ToLower().Contains(cleanSearch) ||
                    g.GrowerLastName.ToLower().Contains(cleanSearch)
                );
            }

            var growers = await query
                .Select(g => new ChatUserDto
                {
                    Id = g.GrowerAccountId,
                    FullName = g.GrowerFirstName + " " + g.GrowerLastName,
                    UserType = "Grower"
                })
                .ToListAsync();

            return Ok(growers);
        }

        [HttpGet("collectors")]
        public async Task<ActionResult<IEnumerable<ChatUserDto>>> GetCollectors([FromQuery] string? search)
        {
            var query = _context.CollectorCreateAccounts.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var cleanSearch = search.Trim().ToLower();
                query = query.Where(c =>
                    c.CollectorFirstName.ToLower().Contains(cleanSearch) ||
                    c.CollectorLastName.ToLower().Contains(cleanSearch)
                );
            }

            var collectors = await query
                .Select(c => new ChatUserDto
                {
                    Id = c.CollectorAccountId,
                    FullName = c.CollectorFirstName + " " + c.CollectorLastName,
                    UserType = "Collector"
                })
                .ToListAsync();

            return Ok(collectors);
        }
    }
}
