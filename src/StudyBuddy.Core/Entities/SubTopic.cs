using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Core.Entities
{
    public class SubTopic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public List<StudyTask> Tasks { get; set; } = new();
        public List<UserProgress> Progress { get; set; } = new();
    }
}
