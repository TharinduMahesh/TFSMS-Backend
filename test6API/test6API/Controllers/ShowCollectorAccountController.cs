using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Models;



[ApiController]
[Route("api/[controller]")]
public class ShowCollectorAccountController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ShowCollectorAccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ✅ GET collector details by email
    [HttpGet("by-email")]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
        var collector = await _context.CollectorCreateAccounts
            .FirstOrDefaultAsync(g => g.CollectorEmail == email);

        if (collector == null)
            return NotFound(new { message = "Collector not found." });

        return Ok(collector);
    }

    // ✅ PUT update collector details by email
    [HttpPut("update-by-email")]
    public async Task<IActionResult> UpdateByEmail([FromQuery] string email, [FromBody] CollectorAccount updatedCollector)
    {
        var existingCollector = await _context.CollectorCreateAccounts
            .FirstOrDefaultAsync(g => g.CollectorEmail == email);

        if (existingCollector == null)
            return NotFound(new { message = "Collector not found." });

        // Update fields
        existingCollector.CollectorFirstName = updatedCollector.CollectorFirstName;
        existingCollector.CollectorLastName = updatedCollector.CollectorLastName;
        existingCollector.CollectorNIC = updatedCollector.CollectorNIC;
        existingCollector.CollectorAddressLine1 = updatedCollector.CollectorAddressLine1;
        existingCollector.CollectorAddressLine2 = updatedCollector.CollectorAddressLine2;
        existingCollector.CollectorCity = updatedCollector.CollectorCity;
        existingCollector.CollectorPostalCode = updatedCollector.CollectorPostalCode;
        existingCollector.CollectorGender = updatedCollector.CollectorGender;
        existingCollector.CollectorDOB = updatedCollector.CollectorDOB;
        existingCollector.CollectorPhoneNum = updatedCollector.CollectorPhoneNum;
        existingCollector.CollectorVehicleNum = updatedCollector.CollectorVehicleNum;
        existingCollector.CollectorEmail = updatedCollector.CollectorEmail;
        // Don't update email or ID

        await _context.SaveChangesAsync();

        return Ok(new { message = "Collector details updated successfully." });
    }
}
