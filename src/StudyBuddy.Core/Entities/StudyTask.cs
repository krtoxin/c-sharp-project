using StudyBuddy.Core.Enums;

namespace StudyBuddy.Core.Entities
{
    public class StudyTask
    {
        public int Id { get; set; }
        public required string Question { get; set; }
        public string? SolutionHint { get; set; }

        public int SubTopicId { get; set; }
        public SubTopic SubTopic { get; set; } = null!;

        public TaskType TaskType { get; set; } = TaskType.OpenEnded;

        public List<TaskOption> Options { get; set; } = new();
        public List<ChatMessage> MessagesWhereAttached { get; set; } = new();
    }

}
