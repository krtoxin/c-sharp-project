using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using StudyBuddy.Core.Entities;

namespace StudyBuddy.Core.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(int chatId, string message, int? taskId)
        {
            await Clients.Group(chatId.ToString())
                .SendAsync("NewMessage", message, taskId, Context.UserIdentifier);
        }

        public async Task JoinChat(int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            await Clients.Group(chatId.ToString())
                .SendAsync("UserJoined", Context.UserIdentifier);
        }

        public async Task UpdateOnlineStatus(bool isOnline)
        {
            await Clients.All.SendAsync("UserOnlineStatus", Context.UserIdentifier, isOnline);
        }
    }
}
