using test6API.DTOs; // We will create this DTO in the next step
using test6API.Models;

namespace test6API.Services
{
    public interface IOrderService
    {
        // This contract says: "There must be a function to place an order."
        // It takes the order details (DTO) and returns the final GrowerOrder.
        Task<GrowerOrder> PlaceOrderAndCreateConversationsAsync(CreateOrderDto orderDto);
    }
}