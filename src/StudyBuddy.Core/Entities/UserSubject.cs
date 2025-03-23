namespace StudyBuddy.Core.Entities
{
    public class UserSubject
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
