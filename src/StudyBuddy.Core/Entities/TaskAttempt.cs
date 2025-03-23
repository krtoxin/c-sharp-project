namespace StudyBuddy.Core.Entities
{
    public class TaskAttempt
    {
        public int TaskAttemptId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public int TaskId { get; set; }
        public StudyTask Task { get; set; }

        public bool IsCorrect { get; set; }
        public DateTime AttemptTime { get; set; } = DateTime.UtcNow;
    }
}
