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

        // POST: api/GrowerBankDetails
        [HttpPost]
        public async Task<IActionResult> PostGrowerBankDetail(GrowerBankDetail detail)
        {
            _context.GrowerBankDetails.Add(detail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGrowerBankDetail), new { email = detail.GrowerEmail }, detail);
        }


        // GET: api/GrowerBankDetails/{email}
        [HttpGet("{email}")]
        public async Task<ActionResult<GrowerBankDetail>> GetGrowerBankDetail(string email)
        {
            var detail = await _context.GrowerBankDetails
                .FirstOrDefaultAsync(x => x.GrowerEmail == email);

            if (detail == null)
                return NotFound();

            return detail;
        }
    }
}
