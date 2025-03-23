using Microsoft.AspNetCore.Identity;

namespace StudyBuddy.Core.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string ProfileImage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsOnline { get; set; }
        public DateTime LastActive { get; set; }

        public List<UserSubject> UserSubjects { get; set; } = new();
        public List<UserProgress> Progresses { get; set; } = new();
        public List<ChatRoomMember> ChatRooms { get; set; } = new();
        public List<ChatMessage> SentMessages { get; set; } = new();
    }
}
