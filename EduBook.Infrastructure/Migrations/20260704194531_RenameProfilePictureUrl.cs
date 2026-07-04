using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameProfilePictureUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePictureUrl",
                table: "users",
                newName: "ProfileImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImageUrl",
                table: "users",
                newName: "ProfilePictureUrl");
        }
    }
}
