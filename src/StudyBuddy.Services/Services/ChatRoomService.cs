using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.Core.Entities;
using StudyBuddy.Core.Enums;
using StudyBuddy.Repositories.Interfaces;
using StudyBuddy.Repositories.Repositories;
using StudyBuddy.Services.IServices;

namespace StudyBuddy.Services.Services
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly IChatRoomRepository _repo;
        private readonly IChatRoomMemberRepository _memberRepo;
        private readonly IUserRepository _userRepository;

        public ChatRoomService(IChatRoomRepository repo, IChatRoomMemberRepository memberRepo, IUserRepository userRepository)
        {
            _repo = repo;
            _memberRepo = memberRepo;
            _userRepository = userRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ChatRoom>> GetRoomsForUserAsync(string userId)
        {
            return await _repo.GetRoomsForUserAsync(userId);
        }

        public async Task<int> CreateRoomAsync(ChatRoom room, string creatorId)
        {
            room.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(room);
            await _repo.SaveAsync();

            var isUserExists = await _userRepository.ExistsAsync(creatorId);
            Console.WriteLine($"DEBUG: user exists = {isUserExists}"); // ← Ось сюди вставляєш

            if (!isUserExists)
                throw new Exception("User ID not found — Google login might have failed.");

            var member = new ChatRoomMember
            {
                ChatRoomId = room.Id,
                UserId = creatorId,
                Role = ChatRole.Admin
            };

            await _memberRepo.AddAsync(member);
            await _repo.SaveAsync();

            return room.Id;
        }

    }
}
