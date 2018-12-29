using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lyp.BlogCore.Repository.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    UserName = table.Column<string>(nullable: true),
                    PassWord = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admins", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "blogArticles",
                columns: table => new
                {
                    bID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    bAuthor = table.Column<string>(nullable: true),
                    bTitle = table.Column<string>(nullable: true),
                    bContent = table.Column<string>(nullable: true),
                    bCreateTime = table.Column<DateTime>(nullable: false),
                    bReadNum = table.Column<int>(nullable: false),
                    bCommentNum = table.Column<int>(nullable: false),
                    cID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blogArticles", x => x.bID);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    cID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    cName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.cID);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    cmID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    cmCommentator = table.Column<string>(nullable: true),
                    cmCommentBody = table.Column<string>(nullable: true),
                    cmCreateTime = table.Column<DateTime>(nullable: false),
                    bID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.cmID);
                });

            migrationBuilder.CreateTable(
                name: "guestBooks",
                columns: table => new
                {
                    gID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    gUserName = table.Column<string>(nullable: true),
                    gBody = table.Column<string>(nullable: true),
                    gCreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guestBooks", x => x.gID);
                });

            migrationBuilder.CreateTable(
                name: "userInfos",
                columns: table => new
                {
                    uID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    uUserName = table.Column<string>(nullable: true),
                    uPassWord = table.Column<string>(nullable: true),
                    uIsAdministrators = table.Column<short>(type: "bit", nullable: false),
                    uCreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userInfos", x => x.uID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admins");

            migrationBuilder.DropTable(
                name: "blogArticles");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "guestBooks");

            migrationBuilder.DropTable(
                name: "userInfos");
        }
    }
}
