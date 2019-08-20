using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.Migrations
{
    public partial class Indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "v6_identityUsers_index_UserID",
                table: "v6_identityUsers",
                columns: new[] { "RowStatus", "UserID" });

            migrationBuilder.CreateIndex(
                name: "v6_identityUsers_index_Login",
                table: "v6_identityUsers",
                columns: new[] { "RowStatus", "UserName", "PasswordHash" });

            migrationBuilder.CreateIndex(
                name: "v6_dataCategories_index_Search",
                table: "v6_dataCategories",
                columns: new[] { "RowStatus", "ResourceID", "CategoryID", "HierarchyText" });

            migrationBuilder.CreateIndex(
                name: "v6_dataCategories_index_Parent",
                table: "v6_dataCategories",
                columns: new[] { "RowStatus", "ResourceID", "ParentID", "CategoryID", "Text" });

            migrationBuilder.CreateIndex(
                name: "v6_dataAccounts_index_Search",
                table: "v6_dataAccounts",
                columns: new[] { "RowStatus", "ResourceID", "AccountID", "Text" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "v6_identityUsers_index_UserID",
                table: "v6_identityUsers");

            migrationBuilder.DropIndex(
                name: "v6_identityUsers_index_Login",
                table: "v6_identityUsers");

            migrationBuilder.DropIndex(
                name: "v6_dataCategories_index_Search",
                table: "v6_dataCategories");

            migrationBuilder.DropIndex(
                name: "v6_dataCategories_index_Parent",
                table: "v6_dataCategories");

            migrationBuilder.DropIndex(
                name: "v6_dataAccounts_index_Search",
                table: "v6_dataAccounts");
        }
    }
}
