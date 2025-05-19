using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Entities;
using StudyBuddy.Core.Data;

namespace StudyBuddy.Core.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _dbContext;

        public ChatHub(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendMessage(int chatId, string message, int? taskId)
        {
            string? senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? Context.User?.FindFirst("nameid")?.Value;

            if (string.IsNullOrEmpty(senderId))
            {
                Console.WriteLine("❌ SenderId is null or empty");
                return;
            }

            if (senderId.Contains("@"))
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == senderId);
                if (user == null)
                {
                    Console.WriteLine($"❌ No user found in DB for email {senderId}");
                    return;
                }
                senderId = user.Id;
            }

            Console.WriteLine($"🟢 Hub Received: {message} from UserId: {senderId}");

            var chatMessage = new ChatMessage
            {
                ChatRoomId = chatId,
                Content = message,
                SenderId = senderId,
                SentAt = DateTime.UtcNow,
                TaskId = taskId
            };

            _dbContext.ChatMessages.Add(chatMessage);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error saving message: {ex.Message}");
                return;
            }

            await Clients.Group(chatId.ToString())
                .SendAsync("NewMessage", message, taskId?.ToString() ?? "", senderId);
        }

        public async Task JoinChat(int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
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
