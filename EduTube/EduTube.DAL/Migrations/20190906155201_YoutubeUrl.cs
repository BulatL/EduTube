using Microsoft.EntityFrameworkCore.Migrations;

namespace EduTube.DAL.Migrations
{
    public partial class YoutubeUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YoutubeId",
                table: "Videos",
                newName: "YoutubeUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YoutubeUrl",
                table: "Videos",
                newName: "YoutubeId");
        }
    }
}
