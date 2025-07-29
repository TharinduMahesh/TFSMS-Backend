using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test6API.Data;
using test6API.Dtos;
using test6API.Models;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GPaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GPaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Payments/PendingCashPayments/{growerEmail}
        [HttpGet("PendingCashPayments/{growerEmail}")]
        public async Task<ActionResult<IEnumerable<GPaymentDto>>> GetPendingCashPayments(string growerEmail)
        {
            var pendingPayments = await (from p in _context.Payments
                                         join o in _context.GrowerOrders on p.GrowerOrderId equals o.GrowerOrderId
                                         join c in _context.CollectorCreateAccounts on o.CollectorEmail equals c.CollectorEmail
                                         where o.GrowerEmail == growerEmail &&
                                               o.PaymentMethod == "Cash" &&
                                               p.PaymentStatus == "Pending" &&
                                               o.OrderStatus == "Accept"
                                         select new GPaymentDto
                                         {
                                             GrowerOrderId = o.GrowerOrderId,
                                             CollectorFirstName = c.CollectorFirstName,
                                             CollectorLastName = c.CollectorLastName,
                                             CollectorCity = c.CollectorCity,
                                             GrossPayment = p.GrossPayment
                                         }).ToListAsync();

            if (pendingPayments == null || !pendingPayments.Any())
            {
                return NotFound("No pending cash payments found for this grower.");
            }

            return Ok(pendingPayments);
        }

        // NEW ENDPOINT FOR PAID PAYMENTS
        // GET: api/Payments/PaidCashPayments/{growerEmail}
        [HttpGet("PaidCashPayments/{growerEmail}")]
        public async Task<ActionResult<IEnumerable<GPaymentDto>>> GetPaidCashPayments(string growerEmail)
        {
            var paidPayments = await (from p in _context.Payments
                                      join o in _context.GrowerOrders on p.GrowerOrderId equals o.GrowerOrderId
                                      join c in _context.CollectorCreateAccounts on o.CollectorEmail equals c.CollectorEmail
                                      where o.GrowerEmail == growerEmail &&
                                            o.PaymentMethod == "Cash" &&
                                            p.PaymentStatus == "Paid" && // Filter for "Paid" status
                                            o.OrderStatus == "Accept"
                                      select new GPaymentDto
                                      {
                                          GrowerOrderId = o.GrowerOrderId,
                                          CollectorFirstName = c.CollectorFirstName,
                                          CollectorLastName = c.CollectorLastName,
                                          CollectorCity = c.CollectorCity,
                                          GrossPayment = p.GrossPayment
                                      }).ToListAsync();

            if (paidPayments == null || !paidPayments.Any())
            {
                return NotFound("No paid cash payments found for this grower.");
            }

            return Ok(paidPayments);
        }


        // GET: api/Payments/PaymentDetails/5
        [HttpGet("PaymentDetails/{growerOrderId}")]
        public async Task<ActionResult<GPaymentDetailDto>> GetPaymentDetails(int growerOrderId)
        {
            var paymentDetail = await (from o in _context.GrowerOrders
                                       join g in _context.GrowerCreateAccounts on o.GrowerEmail equals g.GrowerEmail
                                       join c in _context.CollectorCreateAccounts on o.CollectorEmail equals c.CollectorEmail
                                       where o.GrowerOrderId == growerOrderId
                                       select new GPaymentDetailDto
                                       {
                                           SuperTeaQuantity = o.SuperTeaQuantity,
                                           GreenTeaQuantity = o.GreenTeaQuantity,
                                           GrowerFirstName = g.GrowerFirstName,
                                           GrowerLastName = g.GrowerLastName,
                                           CollectorPhoneNum = c.CollectorPhoneNum,
                                           CollectorVehicleNum = c.CollectorVehicleNum
                                       }).FirstOrDefaultAsync();

            if (paymentDetail == null)
            {
                return NotFound("Payment details not found for this order.");
            }

            return Ok(paymentDetail);
        }
    }
}
