namespace StudyBuddy.Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }

        public List<Category> Subcategories { get; set; } = new();
        public List<Subject> Subjects { get; set; } = new();
    }
}
