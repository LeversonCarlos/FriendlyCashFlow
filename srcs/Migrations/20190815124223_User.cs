using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.Migrations
{
    public partial class User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "v6_identityUserRoles",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    RoleID = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    RowStatus = table.Column<short>(nullable: false),
                    RowDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v6_identityUserRoles", x => new { x.UserID, x.RoleID });
                });

            migrationBuilder.CreateTable(
                name: "v6_identityUsers",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    RowStatus = table.Column<short>(nullable: false),
                    RowDate = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(max)", nullable: false),
                    Text = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    JoinDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v6_identityUsers", x => x.UserID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "v6_identityUserRoles");

            migrationBuilder.DropTable(
                name: "v6_identityUsers");
        }
    }
}
