using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyBuddy.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddDisplayNameAndPremiumToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPremiumUser",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsPremiumUser",
                table: "Users");
        }
    }
}
