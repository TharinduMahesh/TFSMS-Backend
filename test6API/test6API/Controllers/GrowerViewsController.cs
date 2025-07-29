using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test6API.Dtos;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrowerViewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GrowerViewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a list of pending orders for a specific grower.
        /// </summary>
        /// <param name="growerEmail">The email of the grower.</param>
        /// <returns>A list of pending orders with collector information.</returns>
        // GET: api/GrowerViews/pending/grower@example.com
        [HttpGet("pending/{growerEmail}")]
        public async Task<ActionResult<IEnumerable<GrowerPendingOrderDto>>> GetPendingOrdersForGrower(string growerEmail)
        {
            if (string.IsNullOrEmpty(growerEmail))
            {
                return BadRequest("Grower email cannot be empty.");
            }

            var pendingOrders = await (from payment in _context.Payments
                                       join order in _context.GrowerOrders on payment.GrowerOrderId equals order.GrowerOrderId
                                       join collector in _context.CollectorCreateAccounts on order.CollectorEmail equals collector.CollectorEmail
                                       where payment.GrowerEmail == growerEmail &&
                                             payment.PaymentStatus == "Pending"
                                       select new GrowerPendingOrderDto
                                       {
                                           GrowerOrderId = order.GrowerOrderId,
                                           CollectorName = collector.CollectorFirstName + " " + collector.CollectorLastName,
                                           CollectorCity = collector.CollectorCity,
                                           NetPayment = payment.GrossPayment
                                       }).ToListAsync();

            if (!pendingOrders.Any())
            {
                return NotFound("No pending orders found for this grower.");
            }

            return Ok(pendingOrders);
        }

        /// <summary>
        /// Gets the full details of a specific order from a grower's perspective.
        /// </summary>
        /// <param name="orderId">The ID of the grower order.</param>
        /// <returns>Detailed information about the order and the collector.</returns>
        // GET: api/GrowerViews/details/5
        [HttpGet("details/{orderId}")]
        public async Task<ActionResult<GrowerOrderDetailDto>> GetOrderDetailsForGrower(int orderId)
        {
            var orderDetails = await (from payment in _context.Payments
                                      join order in _context.GrowerOrders on payment.GrowerOrderId equals order.GrowerOrderId
                                      join collector in _context.CollectorCreateAccounts on order.CollectorEmail equals collector.CollectorEmail
                                      where order.GrowerOrderId == orderId
                                      select new GrowerOrderDetailDto
                                      {
                                          TotalAmount = payment.GrossPayment,
                                          SuperTeaQuantity = order.SuperTeaQuantity,
                                          GreenTeaQuantity = order.GreenTeaQuantity,
                                          CollectorFirstName = collector.CollectorFirstName,
                                          CollectorLastName = collector.CollectorLastName,
                                          CollectorPhoneNum = collector.CollectorPhoneNum,
                                          CollectorVehicleNum = collector.CollectorVehicleNum
                                      }).FirstOrDefaultAsync();

            if (orderDetails == null)
            {
                return NotFound("Order details not found.");
            }

            return Ok(orderDetails);
        }
    }
}
