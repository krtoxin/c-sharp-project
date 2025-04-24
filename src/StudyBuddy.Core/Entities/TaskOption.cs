using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Core.Entities
{
    public class TaskOption
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        public int StudyTaskId { get; set; }
        public StudyTask StudyTask { get; set; } = null!;
    }

}
