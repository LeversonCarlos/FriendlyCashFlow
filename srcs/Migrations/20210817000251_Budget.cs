using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.Migrations
{
    public partial class Budget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "v6_dataBudget",
                columns: table => new
                {
                    BudgetID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowStatus = table.Column<short>(nullable: false),
                    RowDate = table.Column<DateTime>(nullable: false),
                    ResourceID = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    PatternID = table.Column<long>(nullable: false),
                    Value = table.Column<decimal>(type: "decimal(15,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v6_dataBudget", x => x.BudgetID);
                    table.ForeignKey(
                        name: "FK_v6_dataBudget_v6_dataPatterns_PatternID",
                        column: x => x.PatternID,
                        principalTable: "v6_dataPatterns",
                        principalColumn: "PatternID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_v6_dataBudget_PatternID",
                table: "v6_dataBudget",
                column: "PatternID");

            migrationBuilder.CreateIndex(
                name: "v6_dataBudget_index_Search",
                table: "v6_dataBudget",
                columns: new[] { "RowStatus", "ResourceID", "PatternID" })
                .Annotation("SqlServer:Include", new[] { "BudgetID", "Value" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "v6_dataBudget");
        }
    }
}
