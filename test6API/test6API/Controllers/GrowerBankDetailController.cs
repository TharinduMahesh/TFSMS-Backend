using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Models;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrowerBankDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GrowerBankDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/GrowerBankDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GrowerBankDetail>>> GetGrowerBankDetails()
        {
            return await _context.GrowerBankDetails.ToListAsync();
        }

        // GET: api/GrowerBankDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GrowerBankDetail>> GetGrowerBankDetail(int id)
        {
            var growerBankDetail = await _context.GrowerBankDetails.FindAsync(id);

            if (growerBankDetail == null)
            {
                return NotFound();
            }

            return growerBankDetail;
        }

        // POST: api/GrowerBankDetails
        [HttpPost]
        public async Task<ActionResult<GrowerBankDetail>> PostGrowerBankDetail(GrowerBankDetail growerBankDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GrowerBankDetails.Add(growerBankDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGrowerBankDetail),
                new { id = growerBankDetail.BankDetailId },
                growerBankDetail);
        }
    }
}