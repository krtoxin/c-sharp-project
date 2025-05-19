using StudyBuddy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Services.IServices
{
    public interface IChatRoomService
    {
        Task<IEnumerable<ChatRoom>> GetRoomsForUserAsync(string userId);
        Task<int> CreateRoomAsync(ChatRoom room, string creatorId); 
    }

}
