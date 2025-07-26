using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Dtos;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectorPaidController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CollectorPaidController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/payments/history?collectorEmail=test@example.com
        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<CollectorPaymentHistoryDto>>> GetPaymentHistory([FromQuery] string collectorEmail)
        {
            if (string.IsNullOrWhiteSpace(collectorEmail))
                return BadRequest("Collector email is required.");

            var payments = await _context.CollectorPayments
                .Where(p => p.PaymentStatus == "Paid" && p.CollectorEmail == collectorEmail)
                .Include(p => p.Grower)
                .OrderByDescending(p => p.PaymentDate)
                .Select(p => new CollectorPaymentHistoryDto
                {
                    RefNumber = p.RefNumber,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    GrowerName = p.Grower.GrowerFirstName + " " + p.Grower.GrowerLastName
                })
                .ToListAsync();

            return Ok(payments);
        }

        // GET: api/payments/history/detail/{refNumber}?collectorEmail=test@example.com
        [HttpGet("history/detail/{refNumber}")]
        public async Task<ActionResult<CollectorPaymentHistoryDetailDto>> GetPaymentDetail(int refNumber, [FromQuery] string collectorEmail)
        {
            if (string.IsNullOrWhiteSpace(collectorEmail))
                return BadRequest("Collector email is required.");

            var payment = await _context.CollectorPayments
                .Include(p => p.Grower)
                .FirstOrDefaultAsync(p => p.RefNumber == refNumber && p.PaymentStatus == "Paid" && p.CollectorEmail == collectorEmail);

            if (payment == null || payment.Grower == null)
                return NotFound();

            var dto = new CollectorPaymentHistoryDetailDto
            {
                RefNumber = payment.RefNumber,
                FirstName = payment.Grower.GrowerFirstName,
                LastName = payment.Grower.GrowerLastName,
                NIC = payment.Grower.GrowerNIC,
                PhoneNumber = payment.Grower.GrowerPhoneNum,
                AddressLine1 = payment.Grower.GrowerAddressLine1,
                AddressLine2 = payment.Grower.GrowerAddressLine2,
                City = payment.Grower.GrowerCity,
                PostalCode = payment.Grower.GrowerPostalCode,
                PaymentDate = payment.PaymentDate,
                Amount = payment.Amount
            };

            return Ok(dto);
        }


    }
}
