using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyBuddy.Core.Migrations
{
    public partial class AddCorrectAnswerToStudyTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "StudyTasks",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "StudyTasks");
        }
    }
}
