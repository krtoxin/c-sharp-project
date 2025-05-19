using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyBuddy.Core.DTOs;
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
        private readonly IChatRepository _chatRepository;

        public ChatRoomService(
            IChatRoomRepository repo,
            IChatRoomMemberRepository memberRepo,
            IUserRepository userRepository,
            IChatRepository chatRepository)
        {
            _repo = repo;
            _memberRepo = memberRepo;
            _userRepository = userRepository;
            _chatRepository = chatRepository;
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

            return await _repo.GetByIdAsync(roomId);
        }

        public async Task<IEnumerable<ChatMessageDto>> GetMessagesForRoomAsync(int roomId, int skip = 0, int take = 50)
        {
            var messages = await _chatRepository.GetMessagesForRoomAsync(roomId, skip, take);

            return messages.Select(m => new ChatMessageDto
            {
                Id = m.Id,
                Content = m.Content,
                SentAt = m.SentAt,
                IsEdited = m.IsEdited,
                TaskId = m.TaskId,
                SenderId = m.SenderId,
                SenderName = m.Sender.FullName,  
                ChatRoomId = m.ChatRoomId,
                AttachmentType = (int)m.AttachmentType,
                AttachmentUrl = m.AttachmentUrl
            });
        }
    }
}
