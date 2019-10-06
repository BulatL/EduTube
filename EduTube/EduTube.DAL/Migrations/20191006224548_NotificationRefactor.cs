using Microsoft.EntityFrameworkCore.Migrations;

namespace EduTube.DAL.Migrations
{
    public partial class NotificationRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seen",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "UserProfileImage",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserProfileImage",
                table: "Notifications");

            migrationBuilder.AddColumn<bool>(
                name: "Seen",
                table: "Notifications",
                nullable: false,
                defaultValue: false);
        }
    }
}
