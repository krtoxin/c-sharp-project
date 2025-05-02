using StudyBuddy.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Core.Entities
{
    public class StudyTask
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Question is required.")]
        public string Question { get; set; } = string.Empty;

        public string? SolutionHint { get; set; }
        [Required(ErrorMessage = "CorrectAnswer is required.")]
        public string CorrectAnswer { get; set; } = string.Empty;

        public int SubTopicId { get; set; }
        public SubTopic SubTopic { get; set; } = null!;

        public TaskType TaskType { get; set; } = TaskType.OpenEnded;

        public List<TaskOption> Options { get; set; } = new();
        public List<ChatMessage> MessagesWhereAttached { get; set; } = new();
    }

}
