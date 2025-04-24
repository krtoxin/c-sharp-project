using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Core.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }
        public required string Icon { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<SubTopic> SubTopics { get; set; } = new();
        public List<UserSubject> UserSubjects { get; set; } = new();
    }
}
