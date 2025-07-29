using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using test6API.Models;
using test6API.Data;

namespace test6API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(Message message)
        {
            message.Timestamp = DateTime.UtcNow;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            string receiverGroupName = $"{message.ReceiverType}-{message.ReceiverId}";
            await Clients.Group(receiverGroupName).SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"].ToString();
            var userType = httpContext.Request.Query["userType"].ToString();

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userType))
            {
                string groupName = $"{userType}-{userId}";
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }

            await base.OnConnectedAsync();
        }
    }
}