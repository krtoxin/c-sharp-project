using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyBuddy.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToStudyTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "StudyTasks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "StudyTasks");
        }
    }
}
