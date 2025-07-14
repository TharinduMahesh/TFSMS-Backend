using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using test6API.Data;
using test6API.Dtos;

[ApiController]
[Route("api/[controller]")]
public class GrowerOrderHarwestController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GrowerOrderHarwestController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/GrowerOrders/harvest-summary?email=kasun@gmail.com
    [HttpGet("harvest-summary")]
    public async Task<ActionResult<GrowerHarvestSummaryDto>> GetHarvestSummary([FromQuery] string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Grower email is required.");
        }

        var orders = await _context.GrowerOrders
            .Where(o => o.GrowerEmail == email)
            .ToListAsync();

        if (!orders.Any())
        {
            return NotFound("No harvest data found for this grower.");
        }

        var summary = new GrowerHarvestSummaryDto
        {
            TotalSuperTeaQuantity = orders.Sum(o => o.SuperTeaQuantity),
            TotalGreenTeaQuantity = orders.Sum(o => o.GreenTeaQuantity),
            Orders = orders
        };

        return Ok(summary);
    }
}
