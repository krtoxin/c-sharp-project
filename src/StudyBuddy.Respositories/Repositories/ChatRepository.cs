using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;

namespace StudyBuddy.Repositories.Repositories
{
    public class ChatRepository : BaseRepository<ChatRoom>, IChatRepository
    {
        public ChatRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<ChatMessage>> GetMessagesForRoomAsync(int roomId, int skip = 0, int take = 50)
            => await _context.ChatMessages
                .Where(m => m.ChatRoomId == roomId)
                .OrderByDescending(m => m.SentAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

        public async Task<IEnumerable<ChatRoom>> GetRecentChatsForUserAsync(string userId)
            => await _context.ChatRooms
                .Where(cr => cr.Members.Any(m => m.UserId == userId))
                .OrderByDescending(cr => cr.CreatedAt)
                .ToListAsync();
    }
}
