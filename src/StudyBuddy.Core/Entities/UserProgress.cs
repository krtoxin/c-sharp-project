using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Core.Entities
{
    public class UserProgress
    {
        public int Id { get; set; }
        public int CompletedTasks { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int SubTopicId { get; set; }
        public SubTopic SubTopic { get; set; }
    }
}
