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
                name: "v6_dataEntry",
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
                    PatternID = table.Column<long>(nullable: false),
                    RecurrencyID = table.Column<long>(nullable: true),
                    TransferID = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    Value = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    Paid = table.Column<bool>(nullable: false),
                    PayDate = table.Column<DateTime>(nullable: true),
                    Sorting = table.Column<decimal>(type: "decimal(20,10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v6_dataEntry", x => x.EntryID);
                });

            migrationBuilder.CreateTable(
                name: "v6_dataPattern",
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
                    Quantity = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v6_dataPattern", x => x.PatternID);
                });

            migrationBuilder.CreateTable(
                name: "v6_dataRecurrency",
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
                    Fixed = table.Column<bool>(nullable: false),
                    Quantity = table.Column<int>(nullable: true),
                    InitialDate = table.Column<DateTime>(nullable: false),
                    LastDate = table.Column<DateTime>(nullable: true),
                    StateValue = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v6_dataRecurrency", x => x.RecurrencyID);
                });

            migrationBuilder.CreateIndex(
                name: "v6_dataPattern_index_Search",
                table: "v6_dataPattern",
                columns: new[] { "RowStatus", "ResourceID", "Type", "CategoryID", "Text" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "v6_dataBalance");

            migrationBuilder.DropTable(
                name: "v6_dataEntry");

            migrationBuilder.DropTable(
                name: "v6_dataPattern");

            migrationBuilder.DropTable(
                name: "v6_dataRecurrency");
        }
    }
}
