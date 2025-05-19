using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyBuddy.Core.Entities;
using StudyBuddy.Core.Enums;
using StudyBuddy.Repositories.Interfaces;
using StudyBuddy.Services.IServices;

namespace StudyBuddy.Services.Services
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly IChatRoomRepository _repo;
        private readonly IChatRoomMemberRepository _memberRepo;
        private readonly IUserRepository _userRepository;

        public ChatRoomService(
            IChatRoomRepository repo,
            IChatRoomMemberRepository memberRepo,
            IUserRepository userRepository)
        {
            _repo = repo;
            _memberRepo = memberRepo;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ChatRoom>> GetRoomsForUserAsync(string userId)
        {
            return await _repo.GetRoomsForUserAsync(userId);
        }

        public async Task<int> CreateRoomWithMembersAsync(ChatRoom room, List<string> memberUserIds)
        {
            room.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(room);
            await _repo.SaveAsync();

            if (memberUserIds == null || memberUserIds.Count == 0)
                throw new ArgumentException("At least one member must be specified.");

            // First user in list will be Admin by default
            var adminUserId = memberUserIds[0];

            foreach (var userId in memberUserIds)
            {
                var exists = await _userRepository.ExistsAsync(userId);
                if (!exists)
                    throw new Exception($"User ID {userId} not found.");

                var member = new ChatRoomMember
                {
                    ChatRoomId = room.Id,
                    UserId = userId,
                    Role = (userId == adminUserId) ? ChatRole.Admin : ChatRole.Member
                };

                await _memberRepo.AddAsync(member);
            }

            await _repo.SaveAsync();

            return room.Id;
        }

        public async Task<ChatRoom> GetOrCreateRoomForTaskAsync(int taskId, string creatorId)
        {
            var existingRooms = await _repo.FindAsync(r => r.TaskId == taskId);

            if (existingRooms.Any())
                return existingRooms.First();

            var newRoom = new ChatRoom
            {
                TaskId = taskId,
                Name = $"Chat for Task #{taskId}",
                CreatedAt = DateTime.UtcNow
            };

            var roomId = await CreateRoomWithMembersAsync(newRoom, new List<string> { creatorId });

            // Assuming your repo has GetByIdAsync method to get the entity by Id
            return await _repo.GetByIdAsync(roomId);
        }
    }
}
