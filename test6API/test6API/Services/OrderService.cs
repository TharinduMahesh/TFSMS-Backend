// File Path: ./Services/OrderService.cs
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.DTOs;
using test6API.Models;

namespace test6API.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICollectorService _collectorService;

        public OrderService(ApplicationDbContext context, ICollectorService collectorService)
        {
            _context = context;
            _collectorService = collectorService;
        }

        public async Task<GrowerOrder> PlaceOrderAndCreateConversationsAsync(CreateOrderDto orderDto)
        {
            // 1. Find the Grower's Account object from their email.
            var grower = await _context.GrowerCreateAccounts
                                       .FirstOrDefaultAsync(g => g.GrowerEmail == orderDto.GrowerEmail);

            if (grower == null)
            {
                throw new Exception($"Grower with email {orderDto.GrowerEmail} not found.");
            }

            // 2. Create the GrowerOrder object
            var order = new GrowerOrder
            {
                SuperTeaQuantity = orderDto.SuperTeaQuantity,
                GreenTeaQuantity = orderDto.GreenTeaQuantity,
                PlaceDate = DateTime.UtcNow,
                TransportMethod = orderDto.TransportMethod,
                PaymentMethod = orderDto.PaymentMethod,
                GrowerEmail = orderDto.GrowerEmail,
                OrderStatus = "Pending"
            };
            _context.GrowerOrders.Add(order);

            // 3. Get all Collector Account objects using our updated service
            var collectors = await _collectorService.GetAllCollectorsAsync();

            // 4. Create a new conversation for each collector, now with emails
            foreach (var collector in collectors) // Loop through the full collector object
            {
                var conversation = new Conversation
                {
                    GrowerAccountId = grower.GrowerAccountId,
                    GrowerEmail = grower.GrowerEmail,         // <-- FIX: Add the grower's email
                    CollectorAccountId = collector.CollectorAccountId,
                    CollectorEmail = collector.CollectorEmail, // <-- FIX: Add the collector's email
                    CreatedAt = DateTime.UtcNow
                };
                _context.Conversations.Add(conversation);
            }

            // 5. Save everything to the database at once
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
