using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Models;



[ApiController]
[Route("api/[controller]")]
public class ShowGrowerAccountController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ShowGrowerAccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ✅ GET grower details by email
    [HttpGet("by-email")]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
        var grower = await _context.GrowerCreateAccounts
            .FirstOrDefaultAsync(g => g.GrowerEmail == email);

        if (grower == null)
            return NotFound(new { message = "Grower not found." });

        return Ok(grower);
    }

    // ✅ PUT update grower details by email
    [HttpPut("update-by-email")]
    public async Task<IActionResult> UpdateByEmail([FromQuery] string email, [FromBody] GrowerCreateAccount updatedGrower)
    {
        var existingGrower = await _context.GrowerCreateAccounts
            .FirstOrDefaultAsync(g => g.GrowerEmail == email);

        if (existingGrower == null)
            return NotFound(new { message = "Grower not found." });

        // Update fields
        existingGrower.GrowerFirstName = updatedGrower.GrowerFirstName;
        existingGrower.GrowerLastName = updatedGrower.GrowerLastName;
        existingGrower.GrowerNIC = updatedGrower.GrowerNIC;
        existingGrower.GrowerAddressLine1 = updatedGrower.GrowerAddressLine1;
        existingGrower.GrowerAddressLine2 = updatedGrower.GrowerAddressLine2;
        existingGrower.GrowerCity = updatedGrower.GrowerCity;
        existingGrower.GrowerPostalCode = updatedGrower.GrowerPostalCode;
        existingGrower.GrowerGender = updatedGrower.GrowerGender;
        existingGrower.GrowerDOB = updatedGrower.GrowerDOB;
        existingGrower.GrowerPhoneNum = updatedGrower.GrowerPhoneNum;
        // Don't update email or ID

        await _context.SaveChangesAsync();

        return Ok(new { message = "Grower details updated successfully." });
    }
}
