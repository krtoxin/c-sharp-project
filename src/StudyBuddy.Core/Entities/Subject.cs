namespace StudyBuddy.Core.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Icon { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<SubTopic> SubTopics { get; set; } = new();
        public List<UserSubject> UserSubjects { get; set; } = new();
    }
}
