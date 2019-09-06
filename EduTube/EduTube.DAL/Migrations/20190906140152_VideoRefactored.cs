using Microsoft.EntityFrameworkCore.Migrations;

namespace EduTube.DAL.Migrations
{
    public partial class VideoRefactored : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YoutubeUrl",
                table: "Videos",
                newName: "YoutubeId");

            migrationBuilder.RenameColumn(
                name: "IvniteCode",
                table: "Videos",
                newName: "InviteCode");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Videos",
                newName: "FileName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YoutubeId",
                table: "Videos",
                newName: "YoutubeUrl");

            migrationBuilder.RenameColumn(
                name: "InviteCode",
                table: "Videos",
                newName: "IvniteCode");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Videos",
                newName: "FilePath");
        }
    }
}
