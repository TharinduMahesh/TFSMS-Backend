
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Required for DbUpdateException
using test6API.Data;
using test6API.DTOs;
using test6API.Models;
using test6API.Services;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrowerOrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ApplicationDbContext _context;

        public GrowerOrdersController(IOrderService orderService, ApplicationDbContext context)
        {
            _orderService = orderService;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<GrowerOrder>> PostGrowerOrder(CreateOrderDto orderDto)
        {
            try
            {
                var newOrder = await _orderService.PlaceOrderAndCreateConversationsAsync(orderDto);
                return CreatedAtAction(nameof(GetGrowerOrder), new { id = newOrder.GrowerOrderId }, newOrder);
            }
            // === THIS NEW CATCH BLOCK IS THE IMPORTANT PART ===
            // It specifically looks for database update errors.
            catch (DbUpdateException ex)
            {
                // This will print the detailed, real error to your backend's console window.
                Console.WriteLine($"DATABASE ERROR: {ex.InnerException?.Message}");

                // This sends a more helpful error message back to Swagger and your Flutter app.
                return StatusCode(500, $"Database Error: {ex.InnerException?.Message}");
            }
            // ===============================================
            catch (Exception ex)
            {
                // This catches other general errors.
                return BadRequest(ex.Message);
            }
        }

        // Your GET methods can remain the same.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GrowerOrder>>> GetGrowerOrders()
        {
            return await _context.GrowerOrders.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GrowerOrder>> GetGrowerOrder(int id)
        {
            var order = await _context.GrowerOrders.FindAsync(id);
            if (order == null) return NotFound();
            return order;
        }



    }
}
