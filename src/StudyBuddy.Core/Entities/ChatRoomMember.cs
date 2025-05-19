using StudyBuddy.Core.Enums;

namespace StudyBuddy.Core.Entities
{
    public class ChatRoomMember
    {
        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; } = null!;

        public required string UserId { get; set; }
        public User User { get; set; } = null!;

        public ChatRole Role { get; set; } = ChatRole.Member;
    }
}
