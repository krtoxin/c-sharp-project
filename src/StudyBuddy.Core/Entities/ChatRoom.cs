using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Core.Entities
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsGroup { get; set; } 
        public string? GroupImage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<ChatRoomMember> Members { get; set; } = new();
        public List<ChatMessage> Messages { get; set; } = new();
    }
}
