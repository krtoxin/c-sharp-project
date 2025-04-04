namespace StudyBuddy.Core.Entities
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? ProfileImage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsOnline { get; set; }
        public DateTime LastActive { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public List<UserSubject> UserSubjects { get; set; } = new();
        public List<UserProgress> Progresses { get; set; } = new();
        public List<ChatRoomMember> ChatRooms { get; set; } = new();
        public List<ChatMessage> SentMessages { get; set; } = new();
    }
}
