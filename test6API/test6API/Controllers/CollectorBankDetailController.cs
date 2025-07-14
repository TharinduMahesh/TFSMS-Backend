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

        // GET: api/CollectorBankDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectorBankDetail>>> GetCollectorBankDetails()
        {
            return await _context.CollectorBankDetails.ToListAsync();
        }

        // GET: api/CollectorBankDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CollectorBankDetail>> GetCollectorBankDetail(int id)
        {
            var collectorBankDetail = await _context.CollectorBankDetails.FindAsync(id);

            if (collectorBankDetail == null)
            {
                return NotFound();
            }

            return collectorBankDetail;
        }

        // POST: api/CollectorBankDetails
        [HttpPost]
        public async Task<ActionResult<CollectorBankDetail>> PostCollectorBankDetail(CollectorBankDetail collectorBankDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CollectorBankDetails.Add(collectorBankDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCollectorBankDetail),
                new { id = collectorBankDetail.CollectorDetailId },
                collectorBankDetail);
        }
    }
}