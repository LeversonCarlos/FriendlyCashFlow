using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.Migrations
{
    public partial class AddAccountsClosingDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "ClosingDay",
                table: "v5_dataAccounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingDay",
                table: "v5_dataAccounts");
        }
    }
}
