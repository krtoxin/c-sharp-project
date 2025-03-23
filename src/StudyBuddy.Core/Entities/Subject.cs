namespace StudyBuddy.Core.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public List<SubTopic> SubTopics { get; set; } = new();
        public List<UserSubject> UserSubjects { get; set; } = new();
    }
}
