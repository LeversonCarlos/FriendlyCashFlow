using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.Migrations
{
    public partial class EntriesChanges03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fixed",
                table: "v6_dataRecurrencies");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "v6_dataRecurrencies");

            migrationBuilder.DropColumn(
                name: "StateValue",
                table: "v6_dataRecurrencies");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "v6_dataPatterns",
                newName: "Count");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "v6_dataRecurrencies",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "v6_dataRecurrencies");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "v6_dataPatterns",
                newName: "Quantity");

            migrationBuilder.AddColumn<bool>(
                name: "Fixed",
                table: "v6_dataRecurrencies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "v6_dataRecurrencies",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "StateValue",
                table: "v6_dataRecurrencies",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
