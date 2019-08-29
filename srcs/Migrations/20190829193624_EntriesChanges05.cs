using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.Migrations
{
    public partial class EntriesChanges05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "v6_dataBalance_index_Search",
                table: "v6_dataBalance",
                columns: new[] { "RowStatus", "ResourceID", "Date", "AccountID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "v6_dataBalance_index_Search",
                table: "v6_dataBalance");
        }
    }
}
