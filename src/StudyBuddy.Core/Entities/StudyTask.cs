namespace StudyBuddy.Core.Entities
{
    public class StudyTask
    {
        public int Id { get; set; }
        public required string Question { get; set; }
        public required string CorrectAnswer { get; set; }
        public string? SolutionHint { get; set; }

        public int SubTopicId { get; set; }
        public SubTopic SubTopic { get; set; } = null!;

        public List<ChatMessage> MessagesWhereAttached { get; set; } = new();
    }
}
