using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Entities;

namespace StudyBuddy.Core.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<SubTopic> SubTopics { get; set; } = null!;
        public DbSet<StudyTask> Tasks { get; set; } = null!;
        public DbSet<ChatRoom> ChatRooms { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public DbSet<UserProgress> UserProgresses { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            );

            builder.Entity<ChatRoomMember>()
                .HasKey(x => new { x.ChatRoomId, x.UserId });

            builder.Entity<ChatRoomMember>()
                .HasOne(x => x.User)
                .WithMany(x => x.ChatRooms)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChatRoom>()
                .HasMany(x => x.Messages)
                .WithOne(x => x.ChatRoom)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}