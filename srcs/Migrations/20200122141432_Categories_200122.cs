using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.Migrations
{
    public partial class Categories_200122 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "v6_dataCategories_index_Search",
                table: "v6_dataCategories");

            migrationBuilder.DropIndex(
                name: "v6_dataCategories_index_Parent",
                table: "v6_dataCategories");

            migrationBuilder.CreateIndex(
                name: "v6_dataCategories_index_Search",
                table: "v6_dataCategories",
                columns: new[] { "RowStatus", "ResourceID", "Type", "CategoryID", "HierarchyText" });

            migrationBuilder.CreateIndex(
                name: "v6_dataCategories_index_Parent",
                table: "v6_dataCategories",
                columns: new[] { "RowStatus", "ResourceID", "Type", "ParentID", "CategoryID", "Text" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "v6_dataCategories_index_Search",
                table: "v6_dataCategories");

            migrationBuilder.DropIndex(
                name: "v6_dataCategories_index_Parent",
                table: "v6_dataCategories");

            migrationBuilder.CreateIndex(
                name: "v6_dataCategories_index_Search",
                table: "v6_dataCategories",
                columns: new[] { "RowStatus", "ResourceID", "CategoryID", "HierarchyText" });

            migrationBuilder.CreateIndex(
                name: "v6_dataCategories_index_Parent",
                table: "v6_dataCategories",
                columns: new[] { "RowStatus", "ResourceID", "ParentID", "CategoryID", "Text" });
        }
    }
}
