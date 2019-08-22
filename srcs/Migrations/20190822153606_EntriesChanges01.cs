using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.Migrations
{
    public partial class EntriesChanges01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_v6_dataRecurrency",
                table: "v6_dataRecurrency");

            migrationBuilder.DropPrimaryKey(
                name: "PK_v6_dataPattern",
                table: "v6_dataPattern");

            migrationBuilder.DropPrimaryKey(
                name: "PK_v6_dataEntry",
                table: "v6_dataEntry");

            migrationBuilder.RenameTable(
                name: "v6_dataRecurrency",
                newName: "v6_dataRecurrencies");

            migrationBuilder.RenameTable(
                name: "v6_dataPattern",
                newName: "v6_dataPatterns");

            migrationBuilder.RenameTable(
                name: "v6_dataEntry",
                newName: "v6_dataEntries");

            migrationBuilder.RenameIndex(
                name: "v6_dataPattern_index_Search",
                table: "v6_dataPatterns",
                newName: "v6_dataPatterns_index_Search");

            migrationBuilder.AddColumn<long>(
                name: "CategoryID",
                table: "v6_dataEntries",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_v6_dataRecurrencies",
                table: "v6_dataRecurrencies",
                column: "RecurrencyID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_v6_dataPatterns",
                table: "v6_dataPatterns",
                column: "PatternID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_v6_dataEntries",
                table: "v6_dataEntries",
                column: "EntryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_v6_dataRecurrencies",
                table: "v6_dataRecurrencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_v6_dataPatterns",
                table: "v6_dataPatterns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_v6_dataEntries",
                table: "v6_dataEntries");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "v6_dataEntries");

            migrationBuilder.RenameTable(
                name: "v6_dataRecurrencies",
                newName: "v6_dataRecurrency");

            migrationBuilder.RenameTable(
                name: "v6_dataPatterns",
                newName: "v6_dataPattern");

            migrationBuilder.RenameTable(
                name: "v6_dataEntries",
                newName: "v6_dataEntry");

            migrationBuilder.RenameIndex(
                name: "v6_dataPatterns_index_Search",
                table: "v6_dataPattern",
                newName: "v6_dataPattern_index_Search");

            migrationBuilder.AddPrimaryKey(
                name: "PK_v6_dataRecurrency",
                table: "v6_dataRecurrency",
                column: "RecurrencyID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_v6_dataPattern",
                table: "v6_dataPattern",
                column: "PatternID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_v6_dataEntry",
                table: "v6_dataEntry",
                column: "EntryID");
        }
    }
}
