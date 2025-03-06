using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.Core.Enums;

namespace StudyBuddy.Core.Entities
{
    public class ChatRoomMember
    {
        public int ChatRoomId { get; set; }
        public required ChatRoom ChatRoom { get; set; }

        public required string UserId { get; set; }
        public required User User { get; set; }

        public ChatRole Role { get; set; } = ChatRole.Member; 
    }
}
