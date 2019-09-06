using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduTube.DAL.Migrations
{
    public partial class RefactoredNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Emoticons_EmoticonId",
                table: "Reactions");

            migrationBuilder.DropTable(
                name: "Emoticons");

            migrationBuilder.DropTable(
                name: "HashTagRelationships");

            migrationBuilder.DropTable(
                name: "Hashtags");

            migrationBuilder.RenameColumn(
                name: "EmoticonId",
                table: "Reactions",
                newName: "EmojiId");

            migrationBuilder.RenameIndex(
                name: "IX_Reactions_EmoticonId",
                table: "Reactions",
                newName: "IX_Reactions_EmojiId");

            migrationBuilder.CreateTable(
                name: "Emojis",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emojis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TagRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VideoId = table.Column<int>(nullable: true),
                    ChatId = table.Column<int>(nullable: true),
                    TagId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagRelationships_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TagRelationships_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TagRelationships_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Emojis",
                columns: new[] { "Id", "Deleted", "ImagePath", "Name" },
                values: new object[,]
                {
                    { 1, false, "likeEmoji.png", "Like" },
                    { 2, false, "dislikeEmoji.png", "Dislike" },
                    { 3, false, "loveEmoji.png", "Love" },
                    { 4, false, "wowEmoji.png", "WoW" },
                    { 5, false, "laughtEmoji.png", "Hahaha" },
                    { 6, false, "sadEmoji.png", "Sad" },
                    { 7, false, "angryEmoji.png", "Angry" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 11, "World code" },
                    { 10, "Android " },
                    { 9, "Code" },
                    { 8, "HTML" },
                    { 7, "Web development" },
                    { 3, "Backend" },
                    { 5, "Java" },
                    { 4, "C#" },
                    { 12, "Engineering " },
                    { 2, "Frontend" },
                    { 1, "Programming" },
                    { 6, "Python" },
                    { 13, "Artificial intelligence" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagRelationships_ChatId",
                table: "TagRelationships",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_TagRelationships_TagId",
                table: "TagRelationships",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TagRelationships_VideoId",
                table: "TagRelationships",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Emojis_EmojiId",
                table: "Reactions",
                column: "EmojiId",
                principalTable: "Emojis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Emojis_EmojiId",
                table: "Reactions");

            migrationBuilder.DropTable(
                name: "Emojis");

            migrationBuilder.DropTable(
                name: "TagRelationships");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.RenameColumn(
                name: "EmojiId",
                table: "Reactions",
                newName: "EmoticonId");

            migrationBuilder.RenameIndex(
                name: "IX_Reactions_EmojiId",
                table: "Reactions",
                newName: "IX_Reactions_EmoticonId");

            migrationBuilder.CreateTable(
                name: "Emoticons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    ImagePath = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emoticons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hashtags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hashtags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HashTagRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChatId = table.Column<int>(nullable: true),
                    HashTagId = table.Column<int>(nullable: true),
                    VideoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HashTagRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HashTagRelationships_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HashTagRelationships_Hashtags_HashTagId",
                        column: x => x.HashTagId,
                        principalTable: "Hashtags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HashTagRelationships_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Emoticons",
                columns: new[] { "Id", "Deleted", "ImagePath", "Name" },
                values: new object[,]
                {
                    { 1, false, "likeEmoticon.png", "Like" },
                    { 2, false, "dislikeEmoticon.png", "Dislike" },
                    { 3, false, "loveEmoticon.png", "Love" },
                    { 4, false, "wowEmoticon.png", "WoW" },
                    { 5, false, "laughtEmoticon.png", "Hahaha" },
                    { 6, false, "sadEmoticon.png", "Sad" },
                    { 7, false, "angryEmoticon.png", "Angry" }
                });

            migrationBuilder.InsertData(
                table: "Hashtags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 11, "World code" },
                    { 10, "Android " },
                    { 9, "Code" },
                    { 8, "HTML" },
                    { 7, "Web development" },
                    { 3, "Backend" },
                    { 5, "Java" },
                    { 4, "C#" },
                    { 12, "Engineering " },
                    { 2, "Frontend" },
                    { 1, "Programming" },
                    { 6, "Python" },
                    { 13, "Artificial intelligence" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HashTagRelationships_ChatId",
                table: "HashTagRelationships",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_HashTagRelationships_HashTagId",
                table: "HashTagRelationships",
                column: "HashTagId");

            migrationBuilder.CreateIndex(
                name: "IX_HashTagRelationships_VideoId",
                table: "HashTagRelationships",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Emoticons_EmoticonId",
                table: "Reactions",
                column: "EmoticonId",
                principalTable: "Emoticons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
