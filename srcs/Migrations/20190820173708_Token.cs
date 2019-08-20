using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.Migrations
{
    public partial class Token : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedResourceID",
                table: "v6_identityUsers",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "v6_identityUserTokens",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    RefreshToken = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v6_identityUserTokens", x => new { x.UserID, x.RefreshToken });
                });

            migrationBuilder.CreateIndex(
                name: "v6_identityUserTokens_index_Search",
                table: "v6_identityUserTokens",
                column: "RefreshToken");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "v6_identityUserTokens");

            migrationBuilder.DropColumn(
                name: "SelectedResourceID",
                table: "v6_identityUsers");
        }
    }
}
