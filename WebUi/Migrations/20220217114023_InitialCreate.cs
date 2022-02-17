using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebUi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Future",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalSymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Echange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinTick = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Multiplier = table.Column<int>(type: "int", nullable: false),
                    MarketRule = table.Column<int>(type: "int", nullable: false),
                    LastTradeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InstumentType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Future", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Strike = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TradingClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionType = table.Column<int>(type: "int", nullable: false),
                    FutureId = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalSymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Echange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinTick = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Multiplier = table.Column<int>(type: "int", nullable: false),
                    MarketRule = table.Column<int>(type: "int", nullable: false),
                    LastTradeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InstumentType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Option_Future_FutureId",
                        column: x => x.FutureId,
                        principalTable: "Future",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Option_FutureId",
                table: "Option",
                column: "FutureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropTable(
                name: "Future");
        }
    }
}
