using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.Core.DTOs;

namespace StudyBuddy.Services.Services
{
    public class ChatStateService
    {
        private readonly Dictionary<int, List<ChatMessageDto>> _messagesByRoom = new();

        public List<ChatMessageDto> GetMessages(int roomId)
        {
            if (!_messagesByRoom.TryGetValue(roomId, out var messages))
            {
                messages = new List<ChatMessageDto>();
                _messagesByRoom[roomId] = messages;
            }
            return messages;
        }

        public void AddMessage(int roomId, ChatMessageDto message)
        {
            GetMessages(roomId).Add(message);
        }

        public void ClearMessages(int roomId)
        {
            if (_messagesByRoom.ContainsKey(roomId))
                _messagesByRoom[roomId].Clear();
        }
    }

}
