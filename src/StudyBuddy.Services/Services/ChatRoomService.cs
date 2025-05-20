using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatRoomService(
            IChatRoomRepository repo,
            IChatRoomMemberRepository memberRepo,
            IUserRepository userRepository,
            IChatRepository chatRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _memberRepo = memberRepo;
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<ChatRoom> GetByIdAsync(int id)
        {
            var room = await _repo.GetByIdAsync(id);
            if (room == null)
                throw new Exception($"Chat room with ID {id} not found.");

            return room;
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

        public async Task LeaveOrDeleteChatAsync(int roomId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User ID is required");

            var room = await _repo.GetByIdAsync(roomId);
            if (room == null) return;

            var members = await _memberRepo.FindAsync(m => m.ChatRoomId == roomId);
            room.Members = members.ToList();

            if (room.IsGroup)
            {
                var member = room.Members.FirstOrDefault(m => m.UserId == userId);
                if (member != null)
                {
                    await _memberRepo.DeleteAsync(member);
                    await _repo.SaveAsync();

                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user != null)
                    {
                        var systemMessage = new ChatMessage
                        {
                            ChatRoomId = roomId,
                            Content = $"user {user.FullName} left the group",
                            SenderId = "system",
                            SentAt = DateTime.UtcNow
                        };

                        await _chatRepository.AddMessageAsync(systemMessage);
                        await _repo.SaveAsync();
                    }

                    var remainingMembers = await _memberRepo.FindAsync(m => m.ChatRoomId == roomId);
                    if (!remainingMembers.Any())
                    {
                        await _repo.DeleteAsync(room);
                        await _repo.SaveAsync();
                    }
                }
            }
            else
            {
                await _repo.DeleteAsync(room);
                await _repo.SaveAsync();
            }
        }
        public async Task AddMemberToRoomAsync(int roomId, string userId)
        {
            var exists = await _userRepository.ExistsAsync(userId);
            if (!exists)
                throw new Exception("User not found");

            var existing = await _memberRepo.FindAsync(m => m.ChatRoomId == roomId && m.UserId == userId);
            if (!existing.Any())
            {
                var newMember = new ChatRoomMember
                {
                    ChatRoomId = roomId,
                    UserId = userId,
                    Role = ChatRole.Member
                };
                await _memberRepo.AddAsync(newMember);
                await _repo.SaveAsync();
            }
        }


    }
}
