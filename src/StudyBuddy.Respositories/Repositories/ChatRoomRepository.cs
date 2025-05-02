using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Repositories.Interfaces;

namespace StudyBuddy.Repositories.Repositories
{
    public class ChatRoomRepository : BaseRepository<ChatRoom>, IChatRoomRepository
    {
        private readonly AppDbContext _context;

        public ChatRoomRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChatRoom>> GetRoomsForUserAsync(string userId)
        {
            return await _context.ChatRooms
                .Include(r => r.Members)
                .Include(r => r.Messages)
                .Where(r => r.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }
    }
}
