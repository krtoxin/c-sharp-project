using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudyBuddy.Repositories.Repositories
{
    public class ChatRoomMemberRepository : BaseRepository<ChatRoomMember>, IChatRoomMemberRepository
    {
        private readonly AppDbContext _context;

        public ChatRoomMemberRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChatRoomMember>> GetMembersAsync(int roomId)
        {
            return await _context.Set<ChatRoomMember>()
                .Include(m => m.User)
                .Where(m => m.ChatRoomId == roomId)
                .ToListAsync();
        }
    }
}
