using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcceptedPaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AcceptedPaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a list of accepted (paid) cash payments for a specific collector.
        /// This is for the first page of the payment history view.
        /// </summary>
        /// <param name="collectorEmail">The email of the collector.</param>
        /// <returns>A list of paid payments.</returns>
        // GET: api/AcceptedPayments/paid/cash/collector@example.com
        [HttpGet("paid/cash/{collectorEmail}")]
        public async Task<ActionResult<IEnumerable<AcceptedPaymentDto>>> GetPaidCashPayments(string collectorEmail)
        {
            if (string.IsNullOrEmpty(collectorEmail))
            {
                return BadRequest("Collector email cannot be empty.");
            }

            var paidPayments = await (from payment in _context.Payments
                                      join order in _context.GrowerOrders on payment.GrowerOrderId equals order.GrowerOrderId
                                      join grower in _context.GrowerCreateAccounts on order.GrowerEmail equals grower.GrowerEmail
                                      where payment.CollectorEmail == collectorEmail &&
                                            payment.PaymentStatus == "Paid" &&
                                            order.PaymentMethod == "Cash"
                                      select new AcceptedPaymentDto
                                      {
                                          PaymentId = payment.PaymentId,
                                          GrowerOrderId = payment.GrowerOrderId,
                                          GrowerName = grower.GrowerFirstName + " " + grower.GrowerLastName,
                                          GrowerCity = grower.GrowerCity,
                                          NetPayment = payment.GrossPayment,
                                          PaymentDate = payment.PaymentDate
                                      }).ToListAsync();

            if (!paidPayments.Any())
            {
                return NotFound("No paid cash payments found for this collector.");
            }

            return Ok(paidPayments);
        }

        /// <summary>
        /// Gets the full details of a specific paid order.
        /// This reuses the same logic as the pending payment details endpoint.
        /// </summary>
        /// <param name="orderId">The ID of the grower order.</param>
        /// <returns>Detailed information about the order and grower.</returns>
        // GET: api/AcceptedPayments/details/5
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
    }
}
