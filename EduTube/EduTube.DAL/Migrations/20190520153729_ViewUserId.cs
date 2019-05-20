using Microsoft.EntityFrameworkCore.Migrations;

namespace EduTube.DAL.Migrations
{
    public partial class ViewUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Views_AspNetUsers_UserId1",
                table: "Views");

            migrationBuilder.DropIndex(
                name: "IX_Views_UserId1",
                table: "Views");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Views");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Views",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Views_UserId",
                table: "Views",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Views_AspNetUsers_UserId",
                table: "Views",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Views_AspNetUsers_UserId",
                table: "Views");

            migrationBuilder.DropIndex(
                name: "IX_Views_UserId",
                table: "Views");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Views",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Views",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Views_UserId1",
                table: "Views",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Views_AspNetUsers_UserId1",
                table: "Views",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
