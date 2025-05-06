using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace StudyBuddy.Core.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(int chatId, string message, int? taskId)
        {
            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? Context.User?.FindFirst("nameid")?.Value;

            if (string.IsNullOrEmpty(senderId))
                return;

            await Clients.Group(chatId.ToString())
                .SendAsync("NewMessage", message, taskId, senderId);
        }

        public async Task JoinChat(int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());

            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? Context.User?.FindFirst("nameid")?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await Clients.Group(chatId.ToString())
                    .SendAsync("UserJoined", userId);
            }
        }

        public async Task UpdateOnlineStatus(bool isOnline)
        {
            var userId = Context.User?.FindFirst("nameid")?.Value;
            if (userId != null)
            {
                await Clients.All.SendAsync("UserOnlineStatus", userId, isOnline);
            }
        }
    }
}
