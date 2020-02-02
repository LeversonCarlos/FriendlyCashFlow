using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.Migrations
{
    public partial class Entries_200202 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "v6_dataEntries_index_SearchRecurrency",
                table: "v6_dataEntries",
                column: "RecurrencyID");

            migrationBuilder.CreateIndex(
                name: "v6_dataEntries_index_SearchTransfer",
                table: "v6_dataEntries",
                columns: new[] { "RowStatus", "ResourceID", "TransferID" });

            migrationBuilder.CreateIndex(
                name: "v6_dataEntries_index_SearchDate",
                table: "v6_dataEntries",
                columns: new[] { "RowStatus", "ResourceID", "AccountID", "SearchDate" });

            migrationBuilder.CreateIndex(
                name: "v6_dataEntries_index_SearchText",
                table: "v6_dataEntries",
                columns: new[] { "RowStatus", "ResourceID", "AccountID", "Text" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "v6_dataEntries_index_SearchRecurrency",
                table: "v6_dataEntries");

            migrationBuilder.DropIndex(
                name: "v6_dataEntries_index_SearchTransfer",
                table: "v6_dataEntries");

            migrationBuilder.DropIndex(
                name: "v6_dataEntries_index_SearchDate",
                table: "v6_dataEntries");

            migrationBuilder.DropIndex(
                name: "v6_dataEntries_index_SearchText",
                table: "v6_dataEntries");
        }
    }
}
