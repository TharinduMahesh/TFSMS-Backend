using System;
using Microsoft.AspNetCore.Mvc;
using test6API.Data;
using test6API.Models;


namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectorAccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CollectorAccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/CollectorAccounts
        [HttpPost]
        public async Task<IActionResult> PostCollectorAccount([FromBody] CollectorAccount collector)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.CollectorCreateAccounts.Add(collector);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCollectorAccount), new { id = collector.CollectorAccountId }, collector);
        }

        // GET (optional): To retrieve a single record
        [HttpGet("{id}")]
        public async Task<ActionResult<CollectorAccount>> GetCollectorAccount(int id)
        {
            var collector = await _context.CollectorCreateAccounts.FindAsync(id);
            if (collector == null)
                return NotFound();

            return collector;
        }
    }
}
