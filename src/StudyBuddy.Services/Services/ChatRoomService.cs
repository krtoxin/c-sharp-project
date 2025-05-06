using StudyBuddy.Core.Entities;
using StudyBuddy.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.Repositories.Interfaces;
using StudyBuddy.Core.Enums;

namespace StudyBuddy.Services.Services
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly IChatRoomRepository _repo;
        private readonly IChatRoomMemberRepository _memberRepo;

        public ChatRoomService(IChatRoomRepository repo, IChatRoomMemberRepository memberRepo)
        {
            _repo = repo;
            _memberRepo = memberRepo;
        }

        public async Task<IEnumerable<ChatRoom>> GetRoomsForUserAsync(string userId)
        {
            return await _repo.GetRoomsForUserAsync(userId);
        }

        public async Task CreateRoomAsync(ChatRoom room, string creatorId)
        {
            room.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(room);

            var member = new ChatRoomMember
            {
                ChatRoomId = room.Id,
                UserId = creatorId,
                Role = Core.Enums.ChatRole.Admin,
                ChatRoom = null!, 
                User = null!
            };

            await _memberRepo.AddAsync(member);
        }
    }
}
