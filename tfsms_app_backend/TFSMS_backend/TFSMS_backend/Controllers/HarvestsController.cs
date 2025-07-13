using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TFSMS_app_backend.Data;
using TFSMS_app_backend.Models;

namespace TFSMS_app_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HarvestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HarvestController(AppDbContext context)
        {
            _context = context;
        }

        // POST-Harvest
        [HttpPost]
        public async Task<IActionResult> CreateHarvest([FromBody] Harvest harvest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Load the grower based on GrowerAccountId
            var grower = await _context.GrowerCreateAccounts
                .FirstOrDefaultAsync(g => g.GrowerAccountId == harvest.GrowerAccountId);

            if (grower == null)
                return BadRequest(new { message = "Grower not found." });

            // Auto-fill address from grower if transport is ByCollector
            if (harvest.TransportMethod == TransportMethodType.ByCollector)
            {
                harvest.Address = $"{grower.GrowerAddressLine1}, {grower.GrowerAddressLine2}, {grower.GrowerCity}";
            }

            // Bank validation
            if (harvest.PaymentMethod == PaymentMethodType.Bank)
            {
                if (string.IsNullOrWhiteSpace(harvest.BankName) || string.IsNullOrWhiteSpace(harvest.BankAccountNumber))
                {
                    return BadRequest(new { message = "Bank details are required for Bank payments." });
                }
            }

            harvest.Id = Guid.NewGuid();
            harvest.Status = "Pending";

            _context.Harvests.Add(harvest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHarvestById), new { id = harvest.Id }, harvest);
        }

        // GET-Harvest/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Harvest>> GetHarvestById(Guid id)
        {
            var harvest = await _context.Harvests.FindAsync(id);
            if (harvest == null)
                return NotFound();

            return harvest;
        }

        // PUT-Harvest/id/confirm
        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> ConfirmHarvest(Guid id)
        {
            var harvest = await _context.Harvests.FindAsync(id);
            if (harvest == null)
                return NotFound();

            harvest.Status = "Accepted";
            await _context.SaveChangesAsync();

            return Ok(harvest);
        }

        // GET-Harvest/pending
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Harvest>>> GetPendingHarvests()
        {
            return await _context.Harvests
                                 .Where(h => h.Status == "Pending")
                                 .ToListAsync();
        }
    }
}
