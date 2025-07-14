using Microsoft.AspNetCore.Mvc;
using test6API.Data;
using test6API.Models;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectorCreateAccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CollectorCreateAccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(CollectorCreateAccount collectorAccount)
        {
            if (ModelState.IsValid)
            {
                _context.CollectorCreateAccounts.Add(collectorAccount);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Collector create account successfully" });
            }

            return BadRequest(ModelState);
        }

        // GET by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CollectorCreateAccount>> GetCollectorAccount(int id)
        {
            var collector = await _context.CollectorCreateAccounts.FindAsync(id);

            if (collector == null)
            {
                return NotFound();
            }

            return collector;
        }
    }
}
