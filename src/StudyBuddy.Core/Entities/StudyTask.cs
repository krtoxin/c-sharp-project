using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Core.Entities
{
    public class StudyTask
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string? SolutionHint { get; set; }

        public int SubTopicId { get; set; }
        public SubTopic SubTopic { get; set; }

        public List<ChatMessage> MessagesWhereAttached { get; set; } = new();
    }

}
