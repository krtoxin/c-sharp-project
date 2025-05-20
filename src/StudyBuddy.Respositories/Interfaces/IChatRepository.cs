using StudyBuddy.Core.Entities;

namespace StudyBuddy.Repositories.Interfaces
{
    public interface IChatRepository : IBaseRepository<ChatRoom>
    {
        Task<IEnumerable<ChatMessage>> GetMessagesForRoomAsync(int roomId, int skip = 0, int take = 50);
        Task<IEnumerable<ChatRoom>> GetRecentChatsForUserAsync(string userId);
        Task AddMessageAsync(ChatMessage message);
    }
}
