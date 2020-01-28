using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.Migrations
{
    public partial class Resources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResourceID",
                table: "v6_identityUserRoles",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "v6_identityUserResources",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    ResourceID = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    RowStatus = table.Column<short>(nullable: false),
                    RowDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v6_identityUserResources", x => new { x.UserID, x.ResourceID });
                });

            migrationBuilder.CreateIndex(
                name: "v6_identityUserRoles_index_Search",
                table: "v6_identityUserRoles",
                columns: new[] { "RowStatus", "UserID", "ResourceID", "RoleID" });

            migrationBuilder.CreateIndex(
                name: "v6_identityUserResources_index_Search",
                table: "v6_identityUserResources",
                columns: new[] { "RowStatus", "UserID", "ResourceID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "v6_identityUserResources");

            migrationBuilder.DropIndex(
                name: "v6_identityUserRoles_index_Search",
                table: "v6_identityUserRoles");

            migrationBuilder.DropColumn(
                name: "ResourceID",
                table: "v6_identityUserRoles");
        }
    }
}
