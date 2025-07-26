using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Models;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectorBankDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CollectorBankDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/CollectorBankDetails
        [HttpPost]
        public async Task<IActionResult> PostCollectorBankDetail(CollectorBankDetail detail)
        {
            _context.CollectorBankDetails.Add(detail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCollectorBankDetail), new { email = detail.CollectorEmail }, detail);
        }


        // GET: api/CollectorBankDetails/{email}
        [HttpGet("{email}")]
        public async Task<ActionResult<CollectorBankDetail>> GetCollectorBankDetail(string email)
        {
            var detail = await _context.CollectorBankDetails
                .FirstOrDefaultAsync(x => x.CollectorEmail == email);

            if (detail == null)
                return NotFound();

            return detail;
        }
    }
}
