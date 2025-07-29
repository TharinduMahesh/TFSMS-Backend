using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a list of pending cash payments for a specific collector.
        /// This is for the first page in the mobile app.
        /// </summary>
        /// <param name="collectorEmail">The email of the collector.</param>
        /// <returns>A list of pending payments.</returns>
        // GET: api/payments/pending/cash/collector@example.com
        [HttpGet("pending/cash/{collectorEmail}")]
        public async Task<ActionResult<IEnumerable<PendingPaymentDto>>> GetPendingCashPayments(string collectorEmail)
        {
            if (string.IsNullOrEmpty(collectorEmail))
            {
                return BadRequest("Collector email cannot be empty.");
            }

            var pendingPayments = await (from payment in _context.Payments
                                         join order in _context.GrowerOrders on payment.GrowerOrderId equals order.GrowerOrderId
                                         join grower in _context.GrowerCreateAccounts on order.GrowerEmail equals grower.GrowerEmail
                                         where payment.CollectorEmail == collectorEmail &&
                                               payment.PaymentStatus == "Pending" &&
                                               order.PaymentMethod == "Cash"
                                         select new PendingPaymentDto
                                         {
                                             PaymentId = payment.PaymentId,
                                             GrowerOrderId = payment.GrowerOrderId,
                                             GrowerName = grower.GrowerFirstName + " " + grower.GrowerLastName,
                                             GrowerCity = grower.GrowerCity,
                                             NetPayment = payment.GrossPayment
                                         }).ToListAsync();

            if (!pendingPayments.Any())
            {
                return NotFound("No pending cash payments found for this collector.");
            }

            return Ok(pendingPayments);
        }

        /// <summary>
        /// Gets the full details of a specific order for payment confirmation.
        /// This is for the second page in the mobile app.
        /// </summary>
        /// <param name="orderId">The ID of the grower order.</param>
        /// <returns>Detailed information about the order and grower.</returns>
        // GET: api/payments/details/5
        [HttpGet("details/{orderId}")]
        public async Task<ActionResult<PaymentDetailDto>> GetPaymentDetails(int orderId)
        {
            var paymentDetails = await (from payment in _context.Payments
                                        join order in _context.GrowerOrders on payment.GrowerOrderId equals order.GrowerOrderId
                                        join grower in _context.GrowerCreateAccounts on order.GrowerEmail equals grower.GrowerEmail
                                        where payment.GrowerOrderId == orderId
                                        select new PaymentDetailDto
                                        {
                                            TotalAmount = payment.GrossPayment,
                                            SuperTeaQuantity = order.SuperTeaQuantity,
                                            GreenTeaQuantity = order.GreenTeaQuantity,
                                            GrowerName = grower.GrowerFirstName + " " + grower.GrowerLastName,
                                            GrowerAddressLine1 = grower.GrowerAddressLine1,
                                            GrowerAddressLine2 = grower.GrowerAddressLine2,
                                            GrowerCity = grower.GrowerCity,
                                            GrowerPostalCode = grower.GrowerPostalCode,
                                            GrowerPhoneNum = grower.GrowerPhoneNum
                                        }).FirstOrDefaultAsync();

            if (paymentDetails == null)
            {
                return NotFound("Payment details not found for this order.");
            }

            return Ok(paymentDetails);
        }

        /// <summary>
        /// Marks a payment as "Paid" and updates the payment date.
        /// This is triggered by the "Accept" button.
        /// </summary>
        /// <param name="paymentId">The ID of the payment to update.</param>
        /// <returns>A success response if the update is successful.</returns>
        // PUT: api/payments/accept/10
        [HttpPut("accept/{paymentId}")]
        public async Task<IActionResult> AcceptPayment(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);

            if (payment == null)
            {
                return NotFound("Payment not found.");
            }

            if (payment.PaymentStatus == "Paid")
            {
                return BadRequest("This payment has already been processed.");
            }

            // Update the status and date
            payment.PaymentStatus = "Paid";
            payment.PaymentDate = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // This exception is thrown if the record was modified by another user.
                // You can add more robust handling here if needed.
                return Conflict("The payment record was modified by another user. Please refresh and try again.");
            }

            return NoContent(); // HTTP 204 No Content is a standard success response for a PUT request.
        }
    }
}
