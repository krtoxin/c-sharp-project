using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Entities;

namespace StudyBuddy.Core.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<SubTopic> SubTopics { get; set; } = null!;
        public DbSet<StudyTask> StudyTasks { get; set; } = null!;
        public DbSet<TaskOption> TaskOptions { get; set; } = null!;
        public DbSet<ChatRoom> ChatRooms { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public DbSet<UserProgress> UserProgresses { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Composite keys
            builder.Entity<UserSubject>()
                .HasKey(us => new { us.UserId, us.SubjectId });

            builder.Entity<ChatRoomMember>()
                .HasKey(x => new { x.ChatRoomId, x.UserId });

            // Chat relationships
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

            // Subject <-> Category
            builder.Entity<Subject>()
                .HasOne(s => s.Category)
                .WithMany(c => c.Subjects)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Category hierarchy
            builder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.Subcategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // User <-> Role
            builder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // SubTopic <-> Subject
            builder.Entity<SubTopic>()
                .HasOne(st => st.Subject)
                .WithMany(s => s.SubTopics)
                .HasForeignKey(st => st.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // StudyTask <-> SubTopic
            builder.Entity<StudyTask>()
                .HasOne(t => t.SubTopic)
                .WithMany(st => st.Tasks)
                .HasForeignKey(t => t.SubTopicId)
                .OnDelete(DeleteBehavior.Cascade);

            // TaskOption <-> StudyTask
            builder.Entity<TaskOption>()
                .HasOne(o => o.StudyTask)
                .WithMany(t => t.Options)
                .HasForeignKey(o => o.StudyTaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
