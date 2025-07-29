using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Dtos;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectorOrdersStatusController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CollectorOrdersStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/GrowerOrders/pending
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<PendingOrderByCDto>>> GetPendingOrders()
        {
            var orders = await (from order in _context.GrowerOrders
                                join grower in _context.GrowerCreateAccounts
                                on order.GrowerEmail equals grower.GrowerEmail
                                where order.TransportMethod == "By Collector" && order.OrderStatus == "Pending"
                                select new PendingOrderByCDto
                                {
                                    GrowerOrderId = order.GrowerOrderId,
                                    TotalTea = order.SuperTeaQuantity + order.GreenTeaQuantity,
                                }).ToListAsync();

            return Ok(orders);
        }

        // GET: api/GrowerOrders/details/5
        [HttpGet("details/{id}")]
        public async Task<ActionResult<OrderDetailsByCDto>> GetOrderCDetails(int id)
        {
            var order = await (from o in _context.GrowerOrders
                               join g in _context.GrowerCreateAccounts
                               on o.GrowerEmail equals g.GrowerEmail
                               where o.GrowerOrderId == id
                               select new OrderDetailsByCDto
                               {
                                   GrowerOrderId = o.GrowerOrderId,
                                   SuperTeaQuantity = o.SuperTeaQuantity,
                                   GreenTeaQuantity = o.GreenTeaQuantity,
                                   PlaceDate = o.PlaceDate,
                                   PaymentMethod = o.PaymentMethod,
                               }).FirstOrDefaultAsync();

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // PUT: api/CollectorOrders/accept

        [HttpGet("accepted/{growerEmail}")]
        public async Task<ActionResult<IEnumerable<AccpetedOrderByCDto>>> GetAcceptedOrders(string growerEmail)
        {
            var orders = await (from order in _context.GrowerOrders
                                join collector in _context.CollectorCreateAccounts
                                on order.CollectorEmail equals collector.CollectorEmail
                                where order.OrderStatus == "Accept"
                                      && order.GrowerEmail == growerEmail
                                select new AccpetedOrderByCDto
                                {
                                    GrowerOrderId = order.GrowerOrderId,
                                    TotalTea = order.SuperTeaQuantity + order.GreenTeaQuantity,
                                    CollectorName = collector.CollectorFirstName + " " + collector.CollectorLastName,
                                    CollectorCity = collector.CollectorCity
                                }).ToListAsync();

            return Ok(orders);
        }

        [HttpGet("accepteddetails/{id}")]
        public async Task<ActionResult<IEnumerable<AccpetedOrderDetailsByCDto>>> GetAcceptedOrdersDetails(int id)
        {
            var orders = await (from order in _context.GrowerOrders
                                join collector in _context.CollectorCreateAccounts
                                on order.CollectorEmail equals collector.CollectorEmail
                                where order.OrderStatus == "Accept"
                                      && order.GrowerOrderId == id
                                select new AccpetedOrderDetailsByCDto
                                {
                                    GrowerOrderId = order.GrowerOrderId,
                                    SuperTeaQuantity = order.SuperTeaQuantity,
                                    GreenTeaQuantity = order.GreenTeaQuantity,
                                    PaymentMethod = order.PaymentMethod,
                                    CollectorFirstName = collector.CollectorFirstName,
                                    CollectorSecondName = collector.CollectorLastName,
                                    CollectorAddressLine1 = collector.CollectorAddressLine1,
                                    CollectorAddressLine2 = collector.CollectorAddressLine2,
                                    CollectorCity = collector.CollectorCity,
                                    CollectorPostalCode = collector.CollectorPostalCode,
                                    CollectorVehicleNum = collector.CollectorVehicleNum,
                                    CollectorPhoneNum = collector.CollectorPhoneNum,
                                }).ToListAsync();

            return Ok(orders);
        }
        [HttpPut("updateweights/{orderId}")]
        public async Task<IActionResult> UpdateLeafWeights(int orderId, [FromBody] UpdateWeightsDto dto)
        {
            var order = await _context.GrowerOrders.FirstOrDefaultAsync(o => o.GrowerOrderId == orderId);
            if (order == null)
            {
                return NotFound(new { message = $"Order ID {orderId} not found." });
            }

            order.SuperTeaQuantity = (decimal)dto.SuperLeafWeight;
            order.GreenTeaQuantity = (decimal)dto.GreenLeafWeight;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Leaf weights updated successfully." });
        }


    }
}