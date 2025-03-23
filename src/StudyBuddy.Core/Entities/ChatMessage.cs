using StudyBuddy.Core.Enums;

namespace StudyBuddy.Core.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsEdited { get; set; }

        public int? TaskId { get; set; }
        public StudyTask? Task { get; set; }

        public string SenderId { get; set; }
        public User Sender { get; set; }

        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }

        public AttachmentType AttachmentType { get; set; } = AttachmentType.None;
        public string? AttachmentUrl { get; set; }
    }
}
