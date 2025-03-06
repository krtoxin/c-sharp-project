using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.Core.Entities;

namespace StudyBuddy.Respositories.Interfaces
{
    public interface IChatRepository : IBaseRepository<ChatRoom>
    {
        Task<IEnumerable<ChatMessage>> GetMessagesForRoomAsync(int roomId, int skip = 0, int take = 50);
        Task<IEnumerable<ChatRoom>> GetRecentChatsForUserAsync(string userId);
    }
}
