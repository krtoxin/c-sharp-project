using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyBuddy.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPhp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImage",
                table: "Users",
                newName: "ProfileImageMimeType");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImageData",
                table: "Users",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageData",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ProfileImageMimeType",
                table: "Users",
                newName: "ProfileImage");
        }
    }
}
