using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Entities;

namespace StudyBuddy.Core.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<SubTopic> SubTopics { get; set; } = null!;
        public DbSet<StudyTask> StudyTasks { get; set; } = null!;
        public DbSet<ChatRoom> ChatRooms { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public DbSet<UserProgress> UserProgresses { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserSubject>()
                .HasKey(us => new { us.UserId, us.SubjectId });

            builder.Entity<ChatRoomMember>()
                .HasKey(x => new { x.ChatRoomId, x.UserId });

            builder.Entity<ChatRoomMember>()
                .HasOne(x => x.User)
                .WithMany(x => x.ChatRooms)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChatRoom>()
                .HasMany(x => x.Messages)
                .WithOne(x => x.ChatRoom)
                .HasForeignKey(x => x.ChatRoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Subject>()
                .HasOne(s => s.Category)
                .WithMany(c => c.Subjects)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.Subcategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
