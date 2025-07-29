using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
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

    // This is your original endpoint, which can remain as is
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
            return Ok(new GrowerHarvestSummaryDto()); // Return an empty summary instead of NotFound
        }

        var summary = new GrowerHarvestSummaryDto
        {
            TotalSuperTeaQuantity = orders.Sum(o => o.SuperTeaQuantity),
            TotalGreenTeaQuantity = orders.Sum(o => o.GreenTeaQuantity),
            Orders = orders
        };

        return Ok(summary);
    }

    // New endpoint to get harvest summary by a specific time period
    [HttpGet("harvest-summary-by-period")]
    public async Task<ActionResult<GrowerHarvestSummaryDto>> GetHarvestSummaryByPeriod([FromQuery] string email, [FromQuery] string period = "thisweek")
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Grower email is required.");
        }

        var (startDate, endDate) = GetDateRangeFromPeriod(period);

        var orders = await _context.GrowerOrders
            .Where(o => o.GrowerEmail == email && o.PlaceDate >= startDate && o.PlaceDate < endDate)
            .OrderByDescending(o => o.PlaceDate) // Order by most recent
            .ToListAsync();

        if (!orders.Any())
        {
            return Ok(new GrowerHarvestSummaryDto()); // Return empty summary if no orders in the period
        }

        var summary = new GrowerHarvestSummaryDto
        {
            TotalSuperTeaQuantity = orders.Sum(o => o.SuperTeaQuantity),
            TotalGreenTeaQuantity = orders.Sum(o => o.GreenTeaQuantity),
            Orders = orders
        };

        return Ok(summary);
    }

    // Helper method to determine the date range based on the period string
    private (DateTime, DateTime) GetDateRangeFromPeriod(string period)
    {
        DateTime now = DateTime.UtcNow;
        DayOfWeek startOfWeek = DayOfWeek.Monday; // Or Sunday, depending on your business logic
        int diff = (7 + (now.DayOfWeek - startOfWeek)) % 7;
        DateTime startOfThisWeek = now.AddDays(-1 * diff).Date;

        switch (period.ToLower())
        {
            case "lastweek":
                DateTime startOfLastWeek = startOfThisWeek.AddDays(-7);
                return (startOfLastWeek, startOfThisWeek);

            case "thismonth":
                DateTime startOfThisMonth = new DateTime(now.Year, now.Month, 1);
                return (startOfThisMonth, startOfThisMonth.AddMonths(1));

            case "lastmonth":
                DateTime startOfLastMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
                return (startOfLastMonth, startOfLastMonth.AddMonths(1));

            case "last3months":
                DateTime startOfLast3Months = new DateTime(now.Year, now.Month, 1).AddMonths(-2);
                return (startOfLast3Months, startOfLast3Months.AddMonths(3));

            case "thisyear":
                DateTime startOfThisYear = new DateTime(now.Year, 1, 1);
                return (startOfThisYear, startOfThisYear.AddYears(1));

            case "nextweek":
                DateTime startOfNextWeek = startOfThisWeek.AddDays(7);
                return (startOfNextWeek, startOfNextWeek.AddDays(7));

            case "nextmonth":
                DateTime startOfNextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);
                return (startOfNextMonth, startOfNextMonth.AddMonths(1));

            case "next3months":
                DateTime startOfNext3Months = new DateTime(now.Year, now.Month, 1).AddMonths(1);
                return (startOfNext3Months, startOfNext3Months.AddMonths(3));

            case "thisweek":
            default:
                return (startOfThisWeek, startOfThisWeek.AddDays(7));
        }
    }
}