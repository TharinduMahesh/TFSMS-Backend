using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Dtos;

[ApiController]
[Route("api/[controller]")]
public class GrowerOrdersStatusController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GrowerOrdersStatusController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/GrowerOrders/pending
    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<PendingOrderDto>>> GetPendingOrders()
    {
        var orders = await (from order in _context.GrowerOrders
                            join grower in _context.GrowerCreateAccounts
                            on order.GrowerEmail equals grower.GrowerEmail
                            where order.TransportMethod == "By Collector" && order.OrderStatus == "Pending"
                            select new PendingOrderDto
                            {
                                GrowerOrderId = order.GrowerOrderId,
                                TotalTea = order.SuperTeaQuantity + order.GreenTeaQuantity,
                                GrowerName = grower.GrowerFirstName + " " + grower.GrowerLastName,
                                GrowerCity = grower.GrowerCity
                            }).ToListAsync();

        return Ok(orders);
    }

    // GET: api/GrowerOrders/details/5
    [HttpGet("details/{id}")]
    public async Task<ActionResult<OrderDetailsDto>> GetOrderDetails(int id)
    {
        var order = await (from o in _context.GrowerOrders
                           join g in _context.GrowerCreateAccounts
                           on o.GrowerEmail equals g.GrowerEmail
                           where o.GrowerOrderId == id
                           select new OrderDetailsDto
                           {
                               GrowerOrderId = o.GrowerOrderId,
                               SuperTeaQuantity = o.SuperTeaQuantity,
                               GreenTeaQuantity = o.GreenTeaQuantity,
                               PlaceDate = o.PlaceDate,
                               PaymentMethod = o.PaymentMethod,
                               GrowerName = g.GrowerFirstName + " " + g.GrowerLastName,
                               AddressLine1 = g.GrowerAddressLine1,
                               AddressLine2 = g.GrowerAddressLine2,
                               City = g.GrowerCity,
                               PostalCode = g.GrowerPostalCode,
                               NIC = g.GrowerNIC,
                               PhoneNumber = g.GrowerPhoneNum
                           }).FirstOrDefaultAsync();

        if (order == null)
            return NotFound();

        return Ok(order);
    }

    // PUT: api/GrowerOrders/accept
    [HttpPut("accept")]
    public async Task<IActionResult> AcceptOrder([FromBody] AcceptOrderDto dto)
    {
        var order = await _context.GrowerOrders.FindAsync(dto.OrderId);
        if (order == null)
            return NotFound();

        order.OrderStatus = "Accept";
        order.CollectorEmail = dto.CollectorEmail;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("accepted/{collectorEmail}")]
    public async Task<ActionResult<IEnumerable<AcceptedOrderDto>>> GetAcceptedOrders(string collectorEmail)
    {
        var orders = await (from order in _context.GrowerOrders
                            join grower in _context.GrowerCreateAccounts
                            on order.GrowerEmail equals grower.GrowerEmail
                            where order.OrderStatus == "Accept"
                                  && order.CollectorEmail == collectorEmail
                            select new AcceptedOrderDto
                            {
                                GrowerOrderId = order.GrowerOrderId,
                                TotalTea = order.SuperTeaQuantity + order.GreenTeaQuantity,
                                GrowerName = grower.GrowerFirstName + " " + grower.GrowerLastName,
                                GrowerCity = grower.GrowerCity
                            }).ToListAsync();

        return Ok(orders);
    }

}
