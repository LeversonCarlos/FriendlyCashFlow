using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.Migrations
{
    public partial class Entries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "v6_dataBalance",
                columns: table => new
                {
                    ResourceID = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    AccountID = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    RowStatus = table.Column<short>(nullable: false),
                    RowDate = table.Column<DateTime>(nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    PaidValue = table.Column<decimal>(type: "decimal(15,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v6_dataBalance", x => new { x.ResourceID, x.AccountID, x.Date });
                });

            migrationBuilder.CreateTable(
                name: "v6_dataEntries",
                columns: table => new
                {
                    EntryID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowStatus = table.Column<short>(nullable: false),
                    RowDate = table.Column<DateTime>(nullable: false),
                    ResourceID = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Type = table.Column<short>(nullable: false),
                    AccountID = table.Column<long>(nullable: true),
                    SearchDate = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    CategoryID = table.Column<long>(nullable: true),
                    PatternID = table.Column<long>(nullable: true),
                    RecurrencyID = table.Column<long>(nullable: true),
                    RecurrencyItem = table.Column<short>(nullable: true),
                    RecurrencyTotal = table.Column<short>(nullable: true),
                    TransferID = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    DueDate = table.Column<DateTime>(nullable: false),
                    EntryValue = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    Paid = table.Column<bool>(nullable: false),
                    PayDate = table.Column<DateTime>(nullable: true),
                    Sorting = table.Column<decimal>(type: "decimal(20,10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v6_dataEntries", x => x.EntryID);
                });

            migrationBuilder.CreateTable(
                name: "v6_dataPatterns",
                columns: table => new
                {
                    PatternID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowStatus = table.Column<short>(nullable: false),
                    RowDate = table.Column<DateTime>(nullable: false),
                    ResourceID = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Type = table.Column<short>(nullable: false),
                    CategoryID = table.Column<long>(nullable: false),
                    Text = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Count = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v6_dataPatterns", x => x.PatternID);
                    table.ForeignKey(
                        name: "FK_v6_dataPatterns_v6_dataCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "v6_dataCategories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "v6_dataRecurrencies",
                columns: table => new
                {
                    RecurrencyID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowStatus = table.Column<short>(nullable: false),
                    RowDate = table.Column<DateTime>(nullable: false),
                    ResourceID = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    PatternID = table.Column<long>(nullable: false),
                    AccountID = table.Column<long>(nullable: false),
                    EntryValue = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    Type = table.Column<short>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    InitialDate = table.Column<DateTime>(nullable: false),
                    LastDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v6_dataRecurrencies", x => x.RecurrencyID);
                });

            migrationBuilder.CreateIndex(
                name: "v6_dataBalance_index_Search",
                table: "v6_dataBalance",
                columns: new[] { "RowStatus", "ResourceID", "Date", "AccountID" });

            migrationBuilder.CreateIndex(
                name: "IX_v6_dataPatterns_CategoryID",
                table: "v6_dataPatterns",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "v6_dataPatterns_index_Search",
                table: "v6_dataPatterns",
                columns: new[] { "RowStatus", "ResourceID", "Type", "CategoryID", "Text" });

            migrationBuilder.CreateIndex(
                name: "v6_dataRecurrencies_index_Search",
                table: "v6_dataRecurrencies",
                columns: new[] { "RowStatus", "ResourceID", "RecurrencyID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "v6_dataBalance");

            migrationBuilder.DropTable(
                name: "v6_dataEntries");

            migrationBuilder.DropTable(
                name: "v6_dataPatterns");

            migrationBuilder.DropTable(
                name: "v6_dataRecurrencies");
        }
    }
}
