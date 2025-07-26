using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Dtos;

[ApiController]
[Route("api/[controller]")]
public class CollectorPaymentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CollectorPaymentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/CollectorPayments/unpaid
    [HttpGet("unpaid")]
    public async Task<ActionResult<IEnumerable<CollectorPaymentDto>>> GetUnpaidPayments()
    {
        var payments = await (from p in _context.CollectorPayments
                              join g in _context.GrowerCreateAccounts
                              on p.GrowerNIC equals g.GrowerNIC
                              where p.PaymentStatus != "Paid"
                              select new CollectorPaymentDto
                              {
                                  RefNumber = p.RefNumber,
                                  Amount = p.Amount,
                                  GrowerFullName = g.GrowerFirstName + " " + g.GrowerLastName,
                                  GrowerCity = g.GrowerCity
                              }).ToListAsync();

        return Ok(payments);
    }

    // GET: api/CollectorPayments/detail/5
    [HttpGet("detail/{refNumber}")]
    public async Task<ActionResult<CollectorPaymentDetailDto>> GetPaymentDetail(int refNumber)
    {
        var detail = await (from p in _context.CollectorPayments
                            join g in _context.GrowerCreateAccounts
                            on p.GrowerNIC equals g.GrowerNIC
                            where p.RefNumber == refNumber
                            select new CollectorPaymentDetailDto
                            {
                                RefNumber = p.RefNumber,
                                Amount = p.Amount,
                                FirstName = g.GrowerFirstName,
                                LastName = g.GrowerLastName,
                                AddressLine1 = g.GrowerAddressLine1,
                                AddressLine2 = g.GrowerAddressLine2,
                                City = g.GrowerCity,
                                PostalCode = g.GrowerPostalCode,
                                NIC = g.GrowerNIC,
                                PhoneNumber = g.GrowerPhoneNum,
                                PaymentStatus = p.PaymentStatus,
                                PaymentDate = p.PaymentDate
                            }).FirstOrDefaultAsync();

        if (detail == null)
            return NotFound();

        return Ok(detail);
    }
    // PUT: api/CollectorPayments/complete/5
    [HttpPut("complete/{refNumber}")]
    public async Task<IActionResult> CompletePayment(int refNumber)
    {
        var payment = await _context.CollectorPayments.FirstOrDefaultAsync(p => p.RefNumber == refNumber);

        if (payment == null)
        {
            return NotFound();
        }

        payment.PaymentStatus = "Paid";
        payment.PaymentDate = DateTime.UtcNow; // optional
        await _context.SaveChangesAsync();

        return NoContent(); // 204 response
    }

}
