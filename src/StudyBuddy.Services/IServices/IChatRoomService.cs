using StudyBuddy.Core.DTOs;
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
        Task<int> CreateRoomWithMembersAsync(ChatRoom room, List<string> memberUserIds);
        Task<ChatRoom> GetOrCreateRoomForTaskAsync(int taskId, string creatorId);
        Task<IEnumerable<ChatMessageDto>> GetMessagesForRoomAsync(int roomId, int skip = 0, int take = 50);
    }
}
