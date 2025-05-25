using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyBuddy.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskIdToChatRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "ChatRooms",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_TaskId",
                table: "ChatRooms",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRooms_StudyTasks_TaskId",
                table: "ChatRooms",
                column: "TaskId",
                principalTable: "StudyTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRooms_StudyTasks_TaskId",
                table: "ChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_ChatRooms_TaskId",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "ChatRooms");
        }
    }
}
