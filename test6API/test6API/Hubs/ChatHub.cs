// File Path: ./Hubs/ChatHub.cs
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace test6API.Hubs
{
    public class ChatHub : Hub
    {
        // A client (your Flutter app) will call this method to join a "group"
        // for a specific conversation. This ensures they only receive messages
        // for the chat they have open.
        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }

        // A client can call this to leave the group when they close a chat window.
        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        }
    }
}
