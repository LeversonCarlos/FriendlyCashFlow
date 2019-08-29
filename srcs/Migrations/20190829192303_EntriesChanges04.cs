using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.Migrations
{
    public partial class EntriesChanges04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "v6_dataEntries",
                newName: "EntryValue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EntryValue",
                table: "v6_dataEntries",
                newName: "Value");
        }
    }
}
