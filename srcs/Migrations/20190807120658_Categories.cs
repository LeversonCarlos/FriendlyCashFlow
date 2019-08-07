using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.Migrations
{
    public partial class Categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "v5_dataCategories",
                columns: table => new
                {
                    CategoryID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowStatus = table.Column<short>(nullable: false),
                    RowDate = table.Column<DateTime>(nullable: false),
                    ResourceID = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Text = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<short>(nullable: false),
                    ParentID = table.Column<long>(nullable: true),
                    HierarchyText = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v5_dataCategories", x => x.CategoryID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "v5_dataCategories");
        }
    }
}
